using Microsoft.VisualBasic;
using MongoDB.Driver;
using SharpCompress.Readers;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Sockets;
using tcpServer;
using Tracker.Application.Shared;
using Tracker.Domain.Dtos;
using Tracker.Domain.Provider;
using Tracker.GT06N.Models;
using Tracker.GT06N.Shared;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Tracker.GT06N
{
    public class GT06NService
    {
        #region Variable Declaration
        TcpServer myServer;
        // private ITrackLog clogger;
        public static ConcurrentDictionary<string, string> activeIMEI = new ConcurrentDictionary<string, string>();
        public static ConcurrentDictionary<string, LastRecievedData> imeiLastData = new ConcurrentDictionary<string, LastRecievedData>();
        public static ConcurrentDictionary<TcpClient, string> activeDevices = new ConcurrentDictionary<TcpClient, string>();
        private string mailRecipitents;
        private string serviceName;
        private string unmappedEmailAlertIds = "naveentony93@gmail.com";
        // public ITrackLog CLogger { get { return clogger; } }
        private readonly ILogger<GT06NService> _logger;
        private readonly IConfiguration _configuration;
        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates object and sets email id and server details.
        /// </summary>
        /// <param name="adminMailIds"> Admin email ids. </param>
        /// <param name="serviceName"> Service name. </param>
        public GT06NService(string adminMailIds, string serviceName, ILogger<GT06NService> logger,
            IConfiguration configuration)
        {
            this.serviceName = serviceName;
            this.mailRecipitents = adminMailIds;
            _logger = logger;
            myServer = new TcpServer();
            _configuration = configuration;
        }

        ///// <summary>
        ///// Instantiates object and sets email id and server details.
        ///// </summary>
        ///// <param name="adminMailIds"> Admin email ids. </param>
        ///// <param name="logger"> Logger instance. </param>
        ///// <param name="serviceName"> Service name. </param>
        //public GT06NService(string adminMailIds, ITrackLog logger, string serviceName)
        //{
        //    this.serviceName = serviceName;
        //    this.mailRecipitents = adminMailIds;
        //    clogger = logger;
        //    myServer = new TcpServer();
        //}

        #endregion

        #region Public methods

        /// <summary>
        /// Starts GT06 server.
        /// </summary>
        /// <param name="port"> Port number. </param>
        public void Start(int port)
        {
            myServer.OnDataAvailable += myServer_OnDataAvailable;
            myServer.Port = port;
            myServer.Open();
            GT06NService.activeIMEI.Clear();
            try
            {
                unmappedEmailAlertIds = _configuration.GetSection("Appsettings")["UnmappedEmailAlertIds"].ToString();
                MessageAPI mapi = new MessageAPI(_configuration);
                _logger.LogInformation(mapi.SendEmail(serviceName + " Started", "This is a system generated mail to notify start of later service at port " + port + " on " + System.Environment.MachineName + ".", mailRecipitents));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        /// <summary>
        /// Stops GT06 server.
        /// </summary>
        public void Stop()
        {
            if (myServer.IsOpen)
                myServer.Close();

            try
            {
                MessageAPI mapi = new MessageAPI(_configuration);
                _logger.LogInformation(mapi.SendEmail(serviceName + " Stopped", "This is a system generated mail to notify stop of later service on " + System.Environment.MachineName + ".", mailRecipitents));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Event fires when there is any data available for particular connection
        /// </summary>
        /// <param name="connection">Instance of TcpServerConnection which is a rap-around for TcpClient</param>
        private void myServer_OnDataAvailable(TcpServerConnection connection)
        {
            DateTime start = DateTime.Now;
            TimeSpan time;

            _logger.LogInformation("Process start at " + start.ToLongTimeString());

            // ITrackLog ll = null;
            ServiceUtility su = new ServiceUtility();
            byte[] recievedData;
            GT06CoreConverter gtCoreManager = null;
            bool isAnyError = false;

            try
            {
                su.command_expiry_time = Convert.ToInt32(_configuration.GetSection("Appsettings")["commandExpiryTime"]);
                if (su.command_expiry_time == 0)
                    su.command_expiry_time = 300;

                // Read bytes from network stream 
                recievedData = su.readStream(connection.Socket);

                DateTime startTime = DateTime.Now;
                _logger.LogInformation("Data Received: " + IOUtility.ConvertByteArrayToString(recievedData));
                _logger.LogInformation("Thread Id: " + Thread.CurrentThread.ManagedThreadId);

                List<byte[]> recievedDataList = new List<byte[]>();

                recievedDataList = GetMultiPacketBreakup(recievedData);
                _logger.LogDebug("Packets recieved: " + recievedDataList.Count);

                for (int i = 0; i < recievedDataList.Count; i++)
                {
                    // Parse packet data
                    GTMessageModel dataModel = new GTMessageModel();
                    dataModel.ParseMessage(recievedDataList[i]);

                    //-------------------------------------------------------------------------
                    // Store the connection object and IMEI mapping
                    // Or find IMEI from the connection object

                    string deviceImei = string.Empty;
                    if (string.IsNullOrEmpty(dataModel.IMEI) == true && activeDevices.TryGetValue(connection.Socket, out deviceImei))
                    {
                        _logger.LogDebug("Device connection mapped to IMEI: " + deviceImei);
                        if (string.IsNullOrEmpty(dataModel.IMEI))
                            dataModel.IMEI = deviceImei;
                    }
                    else if (string.IsNullOrEmpty(dataModel.IMEI) == false)
                    {
                        TcpClient oldObject = null;
                        foreach (KeyValuePair<TcpClient, string> keyValue in activeDevices)
                        {
                            if (string.Compare(keyValue.Value, dataModel.IMEI, true) == 0)
                            {
                                oldObject = keyValue.Key;
                                break;
                            }
                        }
                        // Remove old connection if found.
                        if (oldObject != null)
                        {
                            _logger.LogDebug("Removing old device connection IMEI: " + dataModel.IMEI);
                            activeDevices.TryRemove(oldObject, out deviceImei);
                        }

                        activeDevices.TryAdd(connection.Socket, dataModel.IMEI);
                        _logger.LogDebug(string.Format("Device connection added for IMEI: {0}, Device Count : {1}", deviceImei, activeDevices.Count));
                    }
                    //-------------------------------------------------------------------------

                    if (dataModel.CheckIfPacketDataValid() == false)
                    {
                        isAnyError = true;
                        _logger.LogError("Invalid Data Received: " + dataModel.ToString());
                    }

                    //-------------------------------------------------------------------------
                    // Protocol command processing starts
                    LastRecievedData lastRecData = null;

                    if (isAnyError == false)
                    {
                        switch (dataModel.CommandType)
                        {
                            case ProtocolCommandType.LoginMessage:
                                {
                                    //using (IMongoCollection<TrackData> db = new DBManager())
                                    //{
                                    string imei = dataModel.IMEI;
                                    Vehicle vehicle = ServiceCommonContainer.GetVehicleDetails(imei, db, clogger);

                                    if (vehicle != null)
                                    {
                                        ll = new Logger(vehicle.VehicleId, mailRecipitents, serviceName);
                                        ll.Debug("Login message received from IMEI: " + imei);
                                    }
                                    else
                                    {
                                        clogger.Info("Error: IMEI Couldn't be mapped to a vehicle. - " + imei);

                                        MessageAPI emailApi = new MessageAPI();
                                        string subject = "Unmapped IMEI - " + this.serviceName;
                                        string message = "Unmapped IMEI - " + imei;
                                        string toAddress = unmappedEmailAlertIds;
                                        emailApi.SendEmail(subject, message, toAddress);

                                        isAnyError = true;
                                    }
                                    //}
                                    break;
                                }
                            case ProtocolCommandType.Acknowledgement:
                                {
                                    //Acknowledgement processing
                                    break;
                                }
                            case ProtocolCommandType.LocationMessage:
                            case ProtocolCommandType.AlarmMessage:
                            case ProtocolCommandType.StatusMessage:
                                {
                                    using (DBManager db = new DBManager())
                                    {
                                        string imei = dataModel.IMEI;

                                        // Fetching Vehicle Information
                                        Vehicle vehicle = ServiceCommonContainer.GetVehicleDetails(imei, db, clogger);
                                        if (vehicle != null)
                                        {
                                            ll = new Logger(vehicle.VehicleId, mailRecipitents, serviceName);
                                            if (gtCoreManager == null)
                                                gtCoreManager = new GT06CoreConverter(ll);

                                            // If command is status message, get lat, long from previous logged message.
                                            if (dataModel.CommandType == ProtocolCommandType.StatusMessage)
                                            {
                                                if (imeiLastData.ContainsKey(dataModel.IMEI))
                                                {
                                                    lastRecData = imeiLastData[dataModel.IMEI];
                                                    // Get lat, long from previous logged message.
                                                    dataModel.Latitude = lastRecData.Latitude;
                                                    dataModel.Longitude = lastRecData.Longitude;
                                                }
                                                else
                                                {
                                                    TrackData previousLog = ServiceCommonContainer.GetPreviousLog(vehicle.VehicleId, dataModel.DateTimeOfLog, db, clogger);

                                                    if (previousLog != null)
                                                    {
                                                        // Get lat, long from previous logged message.
                                                        dataModel.Latitude = previousLog.Latitude;
                                                        dataModel.Longitude = previousLog.Longitude;
                                                    }
                                                    else
                                                    {
                                                        dataModel.Latitude = 13.04397f;
                                                        dataModel.Longitude = 77.48985f;
                                                    }
                                                }
                                                dataModel.GPSStatus = true;
                                                dataModel.NoOfSatellite = 10;
                                            }

                                            // Active List to handle only single record from one IMEI
                                            string ai = string.Empty;
                                            foreach (var item in Functionality.activeIMEI)
                                            {
                                                ai += item.Key + ";";
                                            }

                                            clogger.Debug("Active IMEI's: " + ai);
                                            clogger.Debug("Current IMEI:" + imei);

                                            if (Functionality.activeIMEI.ContainsKey(imei))
                                            {
                                                // Ignore Current string
                                                clogger.Debug("Current string skipped.");
                                            }
                                            else
                                            {
                                                // Adding IMEI to active list
                                                if (!Functionality.activeIMEI.TryAdd(imei, imei))
                                                    clogger.Error("Error adding IMEI:" + imei + " to active list.");

                                                try
                                                {
                                                    // Process Data and then remove IMEI from Active List
                                                    ProcessRecData(vehicle, ref ll, dataModel, gtCoreManager, db, su, ref connection);

                                                    // Store data model last lat, long into dictionary
                                                    if (lastRecData == null)
                                                        lastRecData = new LastRecievedData();
                                                    lastRecData.Latitude = dataModel.Latitude;
                                                    lastRecData.Longitude = dataModel.Longitude;
                                                    lastRecData.GPSStatus = dataModel.GPSStatus;
                                                    lastRecData.NoOfSatellite = dataModel.NoOfSatellite;

                                                    if (imeiLastData.ContainsKey(imei))
                                                        imeiLastData[imei] = lastRecData;
                                                    else
                                                        imeiLastData.TryAdd(imei, lastRecData);
                                                }
                                                catch (Exception ex)
                                                {
                                                    clogger.Error(ex);
                                                }
                                                finally
                                                {
                                                    // Removing IMEI from active list
                                                    string outVar;
                                                    if (!Functionality.activeIMEI.TryRemove(imei, out outVar))
                                                        clogger.Error("Error removing IMEI:" + imei + " from active list.");
                                                }
                                            }
                                        }
                                        else
                                        {
                                            clogger.Error("IMEI Couldn't be mapped to a vehicle.");
                                        }
                                    }
                                    clogger.Debug("Parsed Data: " + dataModel.ToString());
                                    break;
                                }
                            default:
                                {
                                    if (dataModel.Protocol == 255)
                                        clogger.Info("Command not mapped: " + dataModel.ToString());
                                    else
                                        clogger.Error("Command not mapped: " + dataModel.ToString());
                                    break;
                                }
                        }
                    }
                    // Protocol command processing ends
                    //-------------------------------------------------------------------------

                    //-------------------------------------------------------------------------
                    // Send server acknowledgement
                    if (isAnyError == false && dataModel.IsServerResponseRequired)
                    {
                        if (gtCoreManager == null)
                            gtCoreManager = new GT06CoreConverter(ll);

                        // Sending Acknowledgement / Server response
                        gtCoreManager.SendMessage(dataModel.GenerateServerResponseData(), connection.Socket);
                    }
                }
            }
            catch (Exception ex)
            {
                try
                {
                    if (ll != null)
                        ll.Error(ex.ToString());

                    clogger.Error(ex.ToString());
                }
                catch
                {
                    // Error while logging, suppress it
                }
            }
            finally
            {
                try
                {
                    if (ll != null)
                        ll.WriteToLog();

                    time = DateTime.Now - start;
                    clogger.Info("Process end at " + DateTime.Now.ToLongTimeString() + ". Took " + String.Format("{0}.{1}", time.Seconds, time.Milliseconds.ToString().PadLeft(3, '0')));
                    clogger.WriteToLog();
                }
                catch
                {
                    // Error while logging, suppress it
                }
            }
        }

        /// <summary>
        /// Processes Received Data, internally checks for violation and saves data
        /// </summary>
        /// <param name="vehicle">Vehicle Object</param>
        /// <param name="ll">Local Logger</param>
        /// <param name="rcvText">Received Text</param>
        /// <param name="db">DataContext Object</param>
        /// <param name="func">Extended Functionality Object</param>
        /// <param name="su">Service Utility Object</param>
        /// <param name="connection">TCP Connection</param>
        private void ProcessRecData(Vehicle vehicle, ref ITrackLog ll, GTMessageModel dataModel, GT06CoreConverter gtCoreManager, DBManager db, ServiceUtility su, ref TcpServerConnection connection)
        {
            DateTime start = DateTime.Now;
            TimeSpan time;
            #region Processing Received Data

            //Initializing Local Logger 
            if (ll == null)
                ll = new Logger(vehicle.VehicleId, mailRecipitents, serviceName);
            ll.Info("Time: " + start.ToString("dd-MM-yyyy hh:mm:ss ffffff"));
            ll.Info("Data Received: " + dataModel.ToString());

            dataModel.SaveData(ll, su, vehicle, db, gtCoreManager);

            time = DateTime.Now - start;
            ll.Info("Process end at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss ffffff") + ". Took " + String.Format("{0}.{1}", time.Seconds, time.Milliseconds.ToString().PadLeft(3, '0')));
            #endregion
        }


        /// <summary>
        /// Get the break up of packets if multiple packets are recieved.
        /// </summary>
        /// <param name="dataPacket"> Recieved data packet. </param>
        /// <returns> Packet list. </returns>
        private List<byte[]> GetMultiPacketBreakup(byte[] dataPacket)
        {
            List<byte[]> packetList = new List<byte[]>();
            List<byte> currPacket = new List<byte>();
            bool isStartPacketRecieved = false;

            for (int i = 0; i < dataPacket.Length; i++)
            {
                currPacket.Add(dataPacket[i]);

                if (isStartPacketRecieved == false && dataPacket[i] == 0x78 && i < dataPacket.Length - 1 && dataPacket[i + 1] == 0x78)
                {
                    // Start flag recieved
                    isStartPacketRecieved = true;
                }
                else if (isStartPacketRecieved == true && dataPacket[i] == 0x0D && i < dataPacket.Length - 1 && dataPacket[i + 1] == 0x0A
                    && (i + 1 == dataPacket.Length - 1 || dataPacket[i + 2] == 0x78))
                {
                    i = i + 1;
                    currPacket.Add(dataPacket[i]);
                    // End flag recieved
                    packetList.Add(currPacket.ToArray());
                    currPacket = new List<byte>();
                    isStartPacketRecieved = false;
                }
            }

            return packetList;
        }

        #endregion
    }
    public class GT06NDataService
    {
        private readonly CollectionProvider _provider;
        public GT06NDataService(CollectionProvider provider)
        {
            _provider = provider;
        }
        public async Task<TrackData> GetPreviousDeviceTrackData(string DeviceNo, DateTime datetimeOfLog)
        {
            var collection = _provider.GetCollection<TrackData>(CollectionNames.TrackData);
            var result = collection.Find(x => x.DeviceNo == DeviceNo
                                        && x.TrackDateTime < datetimeOfLog
                                        && x.SoftwareVersion == "18")
                                   .Sort(Builders<TrackData>.Sort.Descending("TrackDateTime")).FirstOrDefault();
            return result;
        }
        public async Task<DeviceVehiclesDto> GetVehicleDetailsSp(string DeviceNo)
        {
            var collection = _provider.GetCollection<DeviceVehiclesDto>(CollectionNames.DeviceVehicles);
            var result = await collection.Find(x => x.DeviceNo == DeviceNo).FirstOrDefaultAsync();
            return result;
        }
        public async Task<TrackData> InsertDeviceTrackData(TrackData trackData, CancellationToken cancellationToken)
        {
            var InsertedDatetime = DateTime.Now;
            var TrackData = _provider.GetCollection<TrackData>(CollectionNames.TrackData);
            var TrackDataLive = _provider.GetCollection<TrackData>(CollectionNames.TrackDataLive);
            var TrackDataNotExists = await TrackData.Find(x => x.DeviceNo == trackData.DeviceNo
                                        && x.TrackDateTime < trackData.TrackDateTime).FirstOrDefaultAsync();
            var TrackDataLiveExists = await TrackDataLive.Find(x => x.DeviceNo == trackData.DeviceNo).FirstOrDefaultAsync();
            if (TrackDataNotExists != null)
            {
                //Insert data into TrackData
                await TrackData.InsertOneAsync(trackData);
                //Insert or Update data in TrackDataLive for live tracking
                if (TrackDataLiveExists != null)
                {
                    var TrackDataLiveWithDateExists = await TrackData.Find(x => x.DeviceNo == trackData.DeviceNo
                                        && x.TrackDateTime < trackData.TrackDateTime).FirstOrDefaultAsync();
                    //Update data in live track only if date is greater that last date
                    if (TrackDataLiveWithDateExists != null)
                    {
                        if (trackData.SoftwareVersion == "18")
                        {
                            var filter = Builders<TrackData>.Filter.Eq(t => t.DeviceNo, trackData.DeviceNo);
                            var update = Builders<TrackData>.Update.Set(t => t.Latitude, trackData.Latitude)
                                 .Set(t => t.Longitude, trackData.Longitude)
                                 .Set(t => t.TrackDateTime, trackData.TrackDateTime)
                                 .Set(t => t.Altitude, trackData.Altitude)
                                 .Set(t => t.Satellite, trackData.Satellite)
                                 .Set(t => t.Speed, trackData.Speed)
                                 .Set(t => t.Direction, trackData.Direction)
                                 .Set(t => t.GPSFix, trackData.GPSFix)
                                 .Set(t => t.OdoMeter, Convert.ToInt64(trackData.OdoMeter))
                                 .Set(t => t.OdoinKM, trackData.OdoMeter)
                                 .Set(t => t.Ignition, trackData.Ignition)
                                 .Set(t => t.SoftwareVersion, trackData.SoftwareVersion)
                                 .Set(t => t.AlertType, trackData.AlertType)
                                 .Set(t => t.InsertedDate, trackData.InsertedDate);
                            await TrackDataLive.UpdateOneAsync(filter, update);
                        }
                        else
                        {
                            var filter = Builders<TrackData>.Filter.Eq(t => t.DeviceNo, trackData.DeviceNo);
                            var update = Builders<TrackData>.Update.Set(t => t.Latitude, trackData.Latitude)
                                 .Set(t => t.TrackDateTime, trackData.TrackDateTime)
                                 .Set(t => t.GPSFix, trackData.GPSFix)
                                 .Set(t => t.Ignition, trackData.Ignition)
                                 .Set(t => t.Altitude, trackData.Altitude)
                                 .Set(t => t.Satellite, trackData.Satellite)
                                 .Set(t => t.Speed, trackData.Speed)
                                 .Set(t => t.GPSFix, trackData.GPSFix)
                                 .Set(t => t.Direction, trackData.Direction)
                                 .Set(t => t.OdoMeter, Convert.ToInt64(trackData.OdoMeter))
                                 .Set(t => t.SoftwareVersion, trackData.SoftwareVersion)
                                 .Set(t => t.AlertType, trackData.AlertType)
                                 .Set(t => t.InsertedDate, trackData.InsertedDate);
                            await TrackDataLive.UpdateOneAsync(filter, update);
                        }
                    }
                    else
                    {
                        await TrackDataLive.InsertOneAsync(trackData);
                    }
                }
            }

        }

    }
}