using System.Collections;
using System.Net.Sockets;
using System.Text;
using Tracker.GT06N.Shared;

namespace Tracker.GT06N.Models
{
    public interface IDevice
    {
        /// <summary>
        /// Map string array to GSMDevicleLog object
        /// </summary>
        /// <param name="inputStringArray">string array of received string</param>
        /// <param name="vehicle">Vehicle for this string</param>
        /// <returns>Returns Mapped TrackData object</returns>
        TrackData InsertData(string[] inputStringArray, Vehicle vehicle);

        /// <summary>
        /// Map device data to TrackData object.
        /// </summary>
        /// <param name="deviceData"> Device data. </param>
        /// <param name="vehicle"> Vehicle </param>
        /// <returns> Returns mapped TrackData object. </returns>
        TrackData InsertData(DevicePacketData deviceData, Vehicle vehicle);
    }
    public class GT06CoreConverter : IDevice
    {
        private readonly ILogger<GT06CoreConverter> _logger;

        

        /// <summary>
        /// Constructor for GT06 Core.
        /// </summary>
        /// <param name="loggerInstance"> Logger instance. </param>
        public GT06CoreConverter(ILogger<GT06CoreConverter> logger)
        {
             _logger = logger;
        }

        /// <summary>
        /// Inserts data into GSMDevice log.
        /// </summary>
        /// <param name="inputStringArray"> input string array. </param>
        /// <param name="vehicle"> Vehicle instance. </param>
        /// <returns> Object for GSM device log. </returns>
        public TrackData InsertData(string[] inputStringArray, Vehicle vehicle)
        {
            throw new NotImplementedException();
        }

        public TrackData InsertData(DevicePacketData deviceData, Vehicle vehicle)
        {
            GTMessageModel deviceModel = (GTMessageModel)deviceData;
            TrackData log = new TrackData
            {
                DeviceNo = deviceModel.IMEI,
                ProtocolCommand = deviceModel.Protocol.ToString(),
                GPSStatus = deviceModel.GPSStatus,
                BatteryVoltage = deviceModel.BatteryVoltage,
                TrackDateTime = deviceModel.DateTimeOfLog,
                Latitude = deviceModel.Latitude,
                Longitude = deviceModel.Longitude,
                Altitude = deviceModel.Course,
                Speed = deviceModel.Speed,
                Direction = deviceModel.Direction,
                Satellite = deviceModel.NoOfSatellite,
                IgnitionOn = deviceModel.IgnitionOn,
                BatteryStatus = deviceModel.BatteryStatus
            };
            return log;
        }

        /// <summary>
        /// Sends data over TCP Connection to client.
        /// </summary>
        /// <param name="data">Data to be sent.</param>
        /// <param name="client">TCP client reference.</param>
        public void SendMessage(byte[] data, TcpClient client)
        {
            try
            {
                NetworkStream stream = client.GetStream();

                // Send the message to the connected TcpServer. 
                stream.Write(data, 0, data.Length);
                if (logger != null)
                    logger.Debug("Sent: " + IOUtility.ConvertByteArrayToString(data));
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }



    }
    public class TrackData
    {
        #region Properties
        public string DeviceNo { get; set; }

        public string ProtocolCommand { get; set; }

        public bool GPSStatus { get; set; }

        public string GPSFix
        {
            get
            {
                if (this.GPSStatus)
                    return "A";
                else
                    return "V";
            }
        }

        public DateTime TrackDateTime { get; set; }
        public Single Latitude { get; set; }
        public Single Longitude { get; set; }
        public int Altitude { get; set; }
        public int Speed { get; set; }
        public int Direction { get; set; }
        public int Satellite { get; set; }

        public Single OdoMeter { get; set; }
        public Single OdoinKM { get; set; }

        
        public string Analog1 { get; set; }
        public string Analog2 { get; set; }

        public string Digital1 { get; set; }
        public string Digital2 { get; set; }
        public string Digital3 { get; set; }
        public string Digital4 { get; set; }

        /// <summary>
        /// Indicates the battery voltage(received) of car battery.
        /// </summary>
        public double BatteryVoltage { get; set; }

        /// <summary>
        /// Indicates the status of car battery.
        /// </summary>
        public bool BatteryStatus { get; set; }

        /// <summary>
        /// Has a value indicating whether ignition of the vehicle is on or off. True for ON, False for OFF.
        /// </summary>
        public bool IgnitionOn { get; set; }

        public string Ignition
        {
            get
            {
                if (this.IgnitionOn)
                    return "1";
                else
                    return "0";
            }
        }

        public string SoftwareVersion { get; set; }

        public Int16 AlertType { get; set; }
        public DateTime InsertedDate { get; set; }

        #endregion

        #region Constructor

        public TrackData()
        {
            this.Digital1 = "0";
            this.Digital2 = "0";
            this.Digital3 = "0";
            this.Digital4 = "0";
            this.Analog1 = "0";
            this.Analog2 = "0";
        }

        #endregion
    }
    public class DevicePacketData
    {
    }
    public class GTMessageModel : DevicePacketData
    {
        #region Private Constant

        private ushort[] crctab16 = {
            0x0000, 0x1189, 0x2312, 0x329b, 0x4624, 0x57ad, 0x6536, 0x74bf,
            0x8c48, 0x9dc1, 0xaf5a, 0xbed3, 0xca6c, 0xdbe5, 0xe97e, 0xf8f7,
            0x1081, 0x0108, 0x3393, 0x221a, 0x56a5, 0x472c, 0x75b7, 0x643e,
            0x9cc9, 0x8d40, 0xbfdb, 0xae52, 0xdaed, 0xcb64, 0xf9ff, 0xe876,
            0x2102, 0x308b, 0x0210, 0x1399, 0x6726, 0x76af, 0x4434, 0x55bd,
            0xad4a, 0xbcc3, 0x8e58, 0x9fd1, 0xeb6e, 0xfae7, 0xc87c, 0xd9f5,
            0x3183, 0x200a, 0x1291, 0x0318, 0x77a7, 0x662e, 0x54b5, 0x453c,
            0xbdcb, 0xac42, 0x9ed9, 0x8f50, 0xfbef, 0xea66, 0xd8fd, 0xc974,
            0x4204, 0x538d, 0x6116, 0x709f, 0x0420, 0x15a9, 0x2732, 0x36bb,
            0xce4c, 0xdfc5, 0xed5e, 0xfcd7, 0x8868, 0x99e1, 0xab7a, 0xbaf3,
            0x5285, 0x430c, 0x7197, 0x601e, 0x14a1, 0x0528, 0x37b3, 0x263a,
            0xdecd, 0xcf44, 0xfddf, 0xec56, 0x98e9, 0x8960, 0xbbfb, 0xaa72,
            0x6306, 0x728f, 0x4014, 0x519d, 0x2522, 0x34ab, 0x0630, 0x17b9,
            0xef4e, 0xfec7, 0xcc5c, 0xddd5, 0xa96a, 0xb8e3, 0x8a78, 0x9bf1,
            0x7387, 0x620e, 0x5095, 0x411c, 0x35a3, 0x242a, 0x16b1, 0x0738,
            0xffcf, 0xee46, 0xdcdd, 0xcd54, 0xb9eb, 0xa862, 0x9af9, 0x8b70,
            0x8408, 0x9581, 0xa71a, 0xb693, 0xc22c, 0xd3a5, 0xe13e, 0xf0b7,
            0x0840, 0x19c9, 0x2b52, 0x3adb, 0x4e64, 0x5fed, 0x6d76, 0x7cff,
            0x9489, 0x8500, 0xb79b, 0xa612, 0xd2ad, 0xc324, 0xf1bf, 0xe036,
            0x18c1, 0x0948, 0x3bd3, 0x2a5a, 0x5ee5, 0x4f6c, 0x7df7, 0x6c7e,
            0xa50a, 0xb483, 0x8618, 0x9791, 0xe32e, 0xf2a7, 0xc03c, 0xd1b5,
            0x2942, 0x38cb, 0x0a50, 0x1bd9, 0x6f66, 0x7eef, 0x4c74, 0x5dfd,
            0xb58b, 0xa402, 0x9699, 0x8710, 0xf3af, 0xe226, 0xd0bd, 0xc134,
            0x39c3, 0x284a, 0x1ad1, 0x0b58, 0x7fe7, 0x6e6e, 0x5cf5, 0x4d7c,
            0xc60c, 0xd785, 0xe51e, 0xf497, 0x8028, 0x91a1, 0xa33a, 0xb2b3,
            0x4a44, 0x5bcd, 0x6956, 0x78df, 0x0c60, 0x1de9, 0x2f72, 0x3efb,
            0xd68d, 0xc704, 0xf59f, 0xe416, 0x90a9, 0x8120, 0xb3bb, 0xa232,
            0x5ac5, 0x4b4c, 0x79d7, 0x685e, 0x1ce1, 0x0d68, 0x3ff3, 0x2e7a,
            0xe70e, 0xf687, 0xc41c, 0xd595, 0xa12a, 0xb0a3, 0x8238, 0x93b1,
            0x6b46, 0x7acf, 0x4854, 0x59dd, 0x2d62, 0x3ceb, 0x0e70, 0x1ff9,
            0xf78f, 0xe606, 0xd49d, 0xc514, 0xb1ab, 0xa022, 0x92b9, 0x8330,
            0x7bc7, 0x6a4e, 0x58d5, 0x495c, 0x3de3, 0x2c6a, 0x1ef1, 0x0f78,
        };

        private const byte serverCommandProtocolNumber = 0x80;

        #endregion

        #region Properties

        private byte[] startBits;

        public byte[] StartBits
        {
            get { return startBits; }
            set { startBits = value; }
        }

        private int length;

        public int Length
        {
            get { return length; }
            set { length = value; }
        }

        private int protocol;

        public int Protocol
        {
            get { return protocol; }
            set { protocol = value; }
        }

        private string imei;

        public string IMEI
        {
            get { return imei; }
            set { imei = value; }
        }

        private ushort serialNumber;

        public ushort SerialNumber
        {
            get { return serialNumber; }
            set { serialNumber = value; }
        }

        private ushort errorCheck;

        public ushort ErrorCheck
        {
            get { return errorCheck; }
            set { errorCheck = value; }
        }

        private byte[] stopBits = new byte[2];

        public byte[] StopBits
        {
            get { return stopBits; }
            set { stopBits = value; }
        }

        private System.DateTime dateTimeOfLog;

        public System.DateTime DateTimeOfLog
        {
            get { return dateTimeOfLog; }
            set { dateTimeOfLog = value; }
        }

        private byte noOfSatellite;

        public byte NoOfSatellite
        {
            get { return noOfSatellite; }
            set { noOfSatellite = value; }
        }

        private Single latitude;

        public Single Latitude
        {
            get { return latitude; }
            set { latitude = value; }
        }

        private Single longitude;

        public Single Longitude
        {
            get { return longitude; }
            set { longitude = value; }
        }

        private int speed;

        public int Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        private ushort course;

        public ushort Course
        {
            get { return course; }
            set { course = value; }
        }

        private ushort mcc;

        public ushort MCC
        {
            get { return mcc; }
            set { mcc = value; }
        }

        private int mnc;

        public int MNC
        {
            get { return mnc; }
            set { mnc = value; }
        }

        private ushort lac;

        public ushort LAC
        {
            get { return lac; }
            set { lac = value; }
        }

        private string cellId;

        public string CellId
        {
            get { return cellId; }
            set { cellId = value; }
        }

        private int terminalInfo;

        public int TerminalInfo
        {
            get { return terminalInfo; }
            set { terminalInfo = value; }
        }

        private int vl;

        public int VL
        {
            get { return vl; }
            set { vl = value; }
        }

        private int gsmSignalStrength;

        public int GsmSignalStrength
        {
            get { return gsmSignalStrength; }
            set { gsmSignalStrength = value; }
        }

        private int alarm;

        public int Alarm
        {
            get { return alarm; }
            set { alarm = value; }
        }

        private bool isServerResponseRequired;

        public bool IsServerResponseRequired
        {
            get { return isServerResponseRequired; }
            set { isServerResponseRequired = value; }
        }

        private bool isPacketDataValid;

        private byte[] dataPacket;

        private byte[] serialNumberByte = new byte[2];

        public byte[] SerialNumberByte
        {
            get
            {
                return serialNumberByte;
            }
            set
            {
                serialNumberByte = value;
            }
        }

        private bool gpsStatus;

        public bool GPSStatus
        {
            get { return gpsStatus; }
            set { gpsStatus = value; }
        }

        private double batteryVoltage;

        public double BatteryVoltage
        {
            get { return batteryVoltage; }
            set { batteryVoltage = value; }
        }

        private short signalStrength;

        public short SignalStrength
        {
            get { return signalStrength; }
            set { signalStrength = value; }
        }

        private int direction;

        public int Direction
        {
            get { return direction; }
            set { direction = value; }
        }

        private bool ignitionOn;

        public bool IgnitionOn
        {
            get { return ignitionOn; }
            set { ignitionOn = value; }
        }

        private bool batteryStatus;

        public bool BatteryStatus
        {
            get { return batteryStatus; }
            set { batteryStatus = value; }
        }

        private ProtocolCommandType commandType = ProtocolCommandType.None;

        public ProtocolCommandType CommandType
        {
            get { return commandType; }
            set { commandType = value; }
        }

        private string sendCommandText;

        public string SendCommandText
        {
            get { return sendCommandText; }
            set { sendCommandText = value; }
        }

        private string recievedCommandText;

        public string RecievedCommandText
        {
            get { return recievedCommandText; }
            set { recievedCommandText = value; }
        }

        private int languageCode;

        public int LanguageCode
        {
            get { return languageCode; }
            set { languageCode = value; }
        }

        #endregion

        #region Constructors

        public GTMessageModel()
        {
            byte[] startBits = { 0x78, 0x78 };
            byte[] endBits = { 0x0D, 0x0A };

            this.BatteryStatus = false;
            this.BatteryVoltage = 0;
            this.startBits = startBits;
            this.stopBits = endBits;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Parse the packet data.
        /// </summary>
        /// <param name="data"> Packet data. </param>
        public void ParseMessage(byte[] data)
        {
            this.isServerResponseRequired = false;

            this.dataPacket = data;

            this.length = Convert.ToInt32(string.Format("{0:x2}", dataPacket[2]), 16) + 5;
            this.protocol = Convert.ToInt32(string.Format("{0:x2}", dataPacket[3]), 16);

            switch (this.protocol)
            {
                // Login message
                case 1:
                    {
                        commandType = ProtocolCommandType.LoginMessage;
                        ProcessLoginMessage(this.dataPacket);
                        break;
                    }
                // Location data message
                case 18:
                    {
                        commandType = ProtocolCommandType.LocationMessage;
                        ProcessLocationMessage(this.dataPacket);
                        break;
                    }
                // Status information message message
                case 19:
                    {
                        commandType = ProtocolCommandType.StatusMessage;
                        ProcessHeartbeatMessage(this.dataPacket);
                        break;
                    }
                // Command Acknowledgement data message
                case 21:
                    {
                        commandType = ProtocolCommandType.Acknowledgement;
                        ProcessAcknowledgementMessage(this.dataPacket);
                        break;
                    }
                // Alarm data message
                case 22:
                    {
                        commandType = ProtocolCommandType.AlarmMessage;
                        ProcessAlarmMessage(this.dataPacket);
                        break;
                    }
                default:
                    {
                        // Log protocol command not found message
                        commandType = ProtocolCommandType.None;
                        break;
                    }
            }

            // Calculate error crc 
            this.errorCheck = IOUtility.FlipEndian(BitConverter.ToUInt16(dataPacket, this.length - 4));

            // Get packet data error check crc
            ushort packetDataCrc = GetCrc16(dataPacket, 2, this.length - 6);

            if (this.errorCheck == packetDataCrc)
                isPacketDataValid = true;
            else
                isPacketDataValid = false;
        }

        /// <summary>
        /// Generates packet data for server response.
        /// </summary>
        /// <returns></returns>
        public byte[] GenerateServerResponseData()
        {
            int responseLength = 10;
            byte[] serverResponse = new byte[responseLength];
            // Set start bits
            serverResponse[0] = startBits[0];
            serverResponse[1] = startBits[1];
            // Set response length
            serverResponse[2] = Convert.ToByte((responseLength - 5).ToString(), 16);
            // Set protocol length
            serverResponse[3] = Convert.ToByte(protocol.ToString(), 16);
            // Set serial number
            serverResponse[4] = serialNumberByte[0];
            serverResponse[5] = serialNumberByte[1];

            // Set error CRC bytes
            ushort packetDataCrc = GetCrc16(serverResponse, 2, 4);
            var bytes = BitConverter.GetBytes(IOUtility.FlipEndian(packetDataCrc));
            serverResponse[6] = bytes[0];
            serverResponse[7] = bytes[1];

            // Set end bits bits
            serverResponse[8] = stopBits[0];
            serverResponse[9] = stopBits[1];
            return serverResponse;
        }

        public byte[] GenerateServerCommandResponseData()
        {
            Byte[] sendCommandData = System.Text.Encoding.ASCII.GetBytes(this.sendCommandText);
            int responseLength = 15 + sendCommandData.Length;
            int lengthOfCommand = 4 + sendCommandData.Length;

            byte[] serverResponse = new byte[responseLength];
            // Set start bits
            serverResponse[0] = startBits[0];
            serverResponse[1] = startBits[1];
            // Set response length
            string responseLengthHex = Convert.ToString((responseLength - 5), 16);
            serverResponse[2] = Convert.ToByte(responseLengthHex, 16);
            // Set protocol length
            serverResponse[3] = serverCommandProtocolNumber;
            // Set command length
            string lengthOfCommandHex = Convert.ToString(lengthOfCommand, 16);
            serverResponse[4] = Convert.ToByte(lengthOfCommandHex, 16);

            // Server Flag Bit
            serverResponse[5] = 0x00;
            serverResponse[6] = 0x01;
            serverResponse[7] = 0xA9;
            serverResponse[8] = 0x58;

            // Copy Command text
            Buffer.BlockCopy(sendCommandData, 0, serverResponse, 9, sendCommandData.Length);
            int nextByteLocation = 9 + sendCommandData.Length;

            // Set serial number
            serverResponse[nextByteLocation] = serialNumberByte[0];
            serverResponse[nextByteLocation + 1] = serialNumberByte[1];
            nextByteLocation = nextByteLocation + 2;

            // Set error CRC bytes, generate CRC from 2nd position to last 4 position (ignore last 4 position)
            ushort packetDataCrc = GetCrc16(serverResponse, 2, serverResponse.Length - 6);
            var bytes = BitConverter.GetBytes(IOUtility.FlipEndian(packetDataCrc));
            serverResponse[nextByteLocation] = bytes[0];
            serverResponse[nextByteLocation + 1] = bytes[1];
            nextByteLocation = nextByteLocation + 2;

            // Set end bits bits
            serverResponse[nextByteLocation] = stopBits[0];
            serverResponse[nextByteLocation + 1] = stopBits[1];
            return serverResponse;
        }

        /// <summary>
        /// Converts class data into string formatted data.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder packetString = new StringBuilder();
            packetString.AppendFormat("Length : {0}, Protocol Command : {1}, Serial Number: {2}, ", this.Length, this.Protocol, this.SerialNumber);
            packetString.AppendFormat("IMEI : {0}", this.IMEI);
            packetString.AppendFormat(", DateTime : {0} ", this.DateTimeOfLog);
            packetString.AppendFormat(", isPacketDataValid : {0} ", this.isPacketDataValid);

            switch (this.CommandType)
            {
                // Login message
                case ProtocolCommandType.LoginMessage:
                    {
                        break;
                    }
                // Location data message
                case ProtocolCommandType.LocationMessage:
                    {
                        packetString.AppendFormat(", NoOfSatellite : {0}", this.NoOfSatellite);
                        packetString.AppendFormat(", GPSStatus : {0}", this.GPSStatus);
                        packetString.AppendFormat(", Latitude : {0}", this.Latitude);
                        packetString.AppendFormat(", Longitude : {0}", this.Longitude);
                        packetString.AppendFormat(", Speed : {0}", this.Speed);
                        packetString.AppendFormat(", Direction : {0}", this.Direction);
                        packetString.AppendFormat(", CellId : {0}", this.CellId);
                        break;
                    }
                // Status information message message
                case ProtocolCommandType.StatusMessage:
                    {
                        packetString.AppendFormat(", Latitude : {0}", this.Latitude);
                        packetString.AppendFormat(", Longitude : {0}", this.Longitude);
                        packetString.AppendFormat(", IgnitionOn : {0}", this.IgnitionOn);
                        packetString.AppendFormat(", BatteryStatus : {0}", this.BatteryStatus);
                        packetString.AppendFormat(", BatteryVoltage : {0}", this.BatteryVoltage);
                        packetString.AppendFormat(", GsmSignalStrength : {0}", this.GsmSignalStrength);
                        break;
                    }
                // Acknowledgement data message
                case ProtocolCommandType.Acknowledgement:
                    {
                        packetString.AppendFormat(", CommandRecievedText : {0}", this.RecievedCommandText);
                        packetString.AppendFormat(", LanguageCode : {0}", this.LanguageCode);
                        break;
                    }
                // Alarm data message
                case ProtocolCommandType.AlarmMessage:
                    {
                        packetString.AppendFormat(", NoOfSatellite : {0}", this.NoOfSatellite);
                        packetString.AppendFormat(", GPSStatus : {0}", this.GPSStatus);
                        packetString.AppendFormat(", Latitude : {0}", this.Latitude);
                        packetString.AppendFormat(", Longitude : {0}", this.Longitude);
                        packetString.AppendFormat(", Direction : {0}", this.Direction);
                        packetString.AppendFormat(", CellId : {0}", this.CellId);
                        packetString.AppendFormat(", IgnitionOn : {0}", this.IgnitionOn);
                        packetString.AppendFormat(", BatteryStatus : {0}", this.BatteryStatus);
                        packetString.AppendFormat(", BatteryVoltage : {0}", this.BatteryVoltage);
                        packetString.AppendFormat(", GsmSignalStrength : {0}", this.GsmSignalStrength);
                        break;
                    }
                default:
                    {
                        // Log protocol command not found message
                        packetString.AppendFormat(", Protocol command not found: {0}", this.protocol);
                        break;
                    }
            }
            return packetString.ToString();
        }

        /// <summary>
        /// Save valid string
        /// </summary>
        /// <param name="data">valid string array</param>
        /// <param name="vehicle">Vehicle of received data</param>
        /// <param name="db">DbContext object</param>
        public void SaveData(ITrackLog logger, ServiceUtility su, Vehicle vehicle, DBManager db, IDevice device)
        {
            logger.Debug("Inside SaveData(ITrackLog, ServiceUtility, Vehicle, WheelTrackDb, IDevice) Method. VehicleId:" + vehicle.VehicleId);
            try
            {
                logger.Debug("Map input recieved data to TrackData Mapping Started.");
                TrackData currLog = device.InsertData(this, vehicle);

                // Skipping processing if Garbage record is found.
                if (!currLog.GPSStatus && currLog.Satellite == 0)
                {
                    logger.Info("Current data skipped. Reason: GPS Status-" + currLog.GPSStatus + "   No. of Satellites-" + currLog.Satellite);
                    return;
                }

                if (this.commandType == ProtocolCommandType.LocationMessage)
                {
                    HeartbeatStatus heartbeatStatus = GT06CacheUtility.GetPreviousHeartbeatStatus(currLog.VehicleId, db, logger, currLog.TrackDateTime);

                    if (heartbeatStatus != null)
                    {
                        logger.Debug("Setting ignition and battery from previous heartbeat. - " + currLog.VehicleId);

                        currLog.IgnitionOn = heartbeatStatus.IgnitionOn;
                        currLog.BatteryStatus = heartbeatStatus.BatteryStatus;
                    }

                    // If iginition is off and speed > 4 than consider ignition as on
                    if (currLog.IgnitionOn == false && currLog.Speed > 4)
                        currLog.IgnitionOn = true;
                }
                else if (this.commandType == ProtocolCommandType.StatusMessage
                    || this.commandType == ProtocolCommandType.AlarmMessage)
                {
                    HeartbeatStatus heartbeatStatus = new HeartbeatStatus();
                    heartbeatStatus.VehicleId = currLog.VehicleId;
                    heartbeatStatus.IgnitionOn = currLog.IgnitionOn;
                    heartbeatStatus.BatteryStatus = currLog.BatteryStatus;
                    GT06CacheUtility.SetPreviousHeartbeatStatus(heartbeatStatus);
                }

                TrackData previousLog = ServiceCommonContainer.GetPreviousLog(currLog, db, logger);

                if (previousLog != null
                    && previousLog.ProtocolCommand == "18" && currLog.ProtocolCommand == "19"
                    && previousLog.Speed > 0 && previousLog.TrackDateTime >= DateTime.Now.AddMinutes(-1))
                {
                    logger.Info("Ignoring heartbeat if last packet is moving with speed.");
                    return;
                }

                logger.Debug("Input string to Log Mapping Completed.");
                logger.Debug("Adding Data to TrackData.");

                currLog.OdoMeter = 0;

                //if(previousLog != null && currLog.ProtocolCommand == "18")
                //{
                //    try { 
                //        currLog.OdometerReading = CalculateDistance(previousLog.Latitude, previousLog.Longitude, currLog.Latitude, currLog.Longitude);
                //    }
                //    catch (Exception ex)
                //    {
                //        logger.Error(ex);
                //    }
                //}

                db.SaveTrackData(currLog);

                logger.Debug("Saved data to TrackData.");

                if (currLog.GPSStatus == true && currLog.ProtocolCommand == "18")
                {
                    // Sets current log as previous log.
                    ServiceCommonContainer.SetPreviousLog(currLog);
                }

            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            finally
            {
                logger.Debug("Leaving SaveData(ITrackLog, ServiceUtility, Vehicle, WheelTrackDb, IDevice) Method.");
            }
        }

        /// <summary>
        /// Calculates distance in KM between 2 points.
        /// </summary>
        /// <param name="lat1"></param>
        /// <param name="lon1"></param>
        /// <param name="lat2"></param>
        /// <param name="lon2"></param>
        /// <returns></returns>
        public Single CalculateDistance(Single lat1, Single lon1, Single lat2, Single lon2)
        {
            double theta = lon1 - lon2;
            double dist = Math.Sin(DegToRad(lat1)) * Math.Sin(DegToRad(lat2)) + Math.Cos(DegToRad(lat1)) * Math.Cos(DegToRad(lat2)) * Math.Cos(DegToRad(theta));
            dist = Math.Acos(dist);
            dist = RadToDeg(dist);
            dist = dist * 60 * 1.1515;
            // Convert distance to kms
            dist = dist * 1.609344;
            return (Single)dist;
        }

        private double DegToRad(double deg)
        {
            return (deg * Math.PI / 180.0);
        }
        private double RadToDeg(double rad)
        {
            return (rad / Math.PI * 180.0);
        }

        public bool CheckIfPacketDataValid()
        {
            if (this.isPacketDataValid && this.DateTimeOfLog <= DateTime.Now.AddMinutes(2))
                return true;
            else
                return false;
        }

        #endregion

        #region Private Methods

        #region Parsing packet data methods

        /// <summary>
        /// Process login message.
        /// </summary>
        /// <param name="data"> Packet data. </param>
        private void ProcessLoginMessage(byte[] data)
        {
            StringBuilder imeiBuilder = new StringBuilder();
            for (int i = 4; i <= 11; i++)
            {
                // Convert the number expressed in base-16 to an integer.
                int value = Convert.ToInt32(string.Format("{0:x2}", data[i]), 10);
                if (i > 4 && value < 10)
                    imeiBuilder.Append("0" + value);
                else
                    imeiBuilder.Append(value);
            }
            this.imei = imeiBuilder.ToString();

            serialNumberByte[0] = data[12];
            serialNumberByte[1] = data[13];

            // Get serial number from byte 13-14
            this.serialNumber = IOUtility.FlipEndian(BitConverter.ToUInt16(data, 12));

            this.isServerResponseRequired = true;
        }

        /// <summary>
        /// Processes location message.
        /// </summary>
        /// <param name="data"> Packet data. </param>
        private void ProcessLocationMessage(byte[] data)
        {
            // Byte 4-9 contains datetime data.
            int year = ConvertToByteWithHexBase(data[4]) + 2000;
            byte month = ConvertToByteWithHexBase(data[5]);
            byte day = ConvertToByteWithHexBase(data[6]);
            byte hour = ConvertToByteWithHexBase(data[7]);
            byte minute = ConvertToByteWithHexBase(data[8]);
            byte second = ConvertToByteWithHexBase(data[9]);
            this.DateTimeOfLog = new DateTime(year, month, day, hour, minute, second);
            this.DateTimeOfLog = this.DateTimeOfLog.AddHours(5).AddMinutes(30);

            // Byte 10 contains No of satellite
            this.NoOfSatellite = ConvertToByteWithHexBase(data[10]);

            // Byte 11-14 contains latitude
            byte[] latitudeBytes = new byte[4];
            latitudeBytes[0] = data[11];
            latitudeBytes[1] = data[12];
            latitudeBytes[2] = data[13];
            latitudeBytes[3] = data[14];
            CalculateLatitude(latitudeBytes);

            // Byte 15-18 contains longitude
            byte[] longitudeBytes = new byte[4];
            longitudeBytes[0] = data[15];
            longitudeBytes[1] = data[16];
            longitudeBytes[2] = data[17];
            longitudeBytes[3] = data[18];
            CalculateLongitude(longitudeBytes);

            // Byte 19 contains speed
            this.Speed = Convert.ToInt32(string.Format("{0:x2}", data[19]), 16);
            if (this.Speed > 4)
                this.IgnitionOn = true;

            // Byte 20-21 contains direction and GPS Status data
            this.GPSStatus = IOUtility.IsBitSet(data[20], 4);
            // Get data for direction into byte array
            byte[] bytearray = new byte[2];
            bytearray[0] = data[20];
            bytearray[1] = data[21];
            var bitArray = new BitArray(bytearray);
            // Set bit Bit7 to Bit 2 of first byte to false
            bitArray.Set(0, false);
            bitArray.Set(1, false);
            bitArray.Set(2, false);
            bitArray.Set(3, false);
            bitArray.Set(4, false);
            bitArray.Set(5, false);
            bitArray.CopyTo(bytearray, 0);
            // Get direction value from 10 bit value
            this.Direction = Convert.ToInt32(BitConverter.ToUInt16(bytearray, 0));

            // NOT Processing: Byte 22-26 is not required to process at it contains MCC, MNC and LAC

            // Byte 27-29 contains Cell ID
            StringBuilder cellidBuilder = new StringBuilder();
            for (int i = 27; i <= 29; i++)
            {
                // Convert the number expressed in base-16 to an integer.
                //int value = Convert.ToInt32(string.Format("{0:x2}", data[i]), 10);
                //cellidBuilder.Append(value);
                cellidBuilder.Append(string.Format("{0:x2}", data[i]));
            }
            this.CellId = cellidBuilder.ToString();

            // Byte 30-31 contains serial number
            this.serialNumber = IOUtility.FlipEndian(BitConverter.ToUInt16(data, 30));
            this.serialNumberByte[0] = data[30];
            this.serialNumberByte[1] = data[31];

            this.BatteryStatus = true;

            this.isServerResponseRequired = false;
        }

        /// <summary>
        /// Processes alarm message.
        /// </summary>
        /// <param name="data"> Packet data. </param>
        private void ProcessAlarmMessage(byte[] data)
        {
            // Byte 4-9 contains datetime data.
            int year = ConvertToByteWithHexBase(data[4]) + 2000;
            byte month = ConvertToByteWithHexBase(data[5]);
            byte day = ConvertToByteWithHexBase(data[6]);
            byte hour = ConvertToByteWithHexBase(data[7]);
            byte minute = ConvertToByteWithHexBase(data[8]);
            byte second = ConvertToByteWithHexBase(data[9]);
            this.DateTimeOfLog = new DateTime(year, month, day, hour, minute, second);
            this.DateTimeOfLog = this.DateTimeOfLog.AddHours(5).AddMinutes(30);

            // Byte 10 contains No of satellite
            this.NoOfSatellite = ConvertToByteWithHexBase(data[10]);

            // Byte 11-14 contains latitude
            byte[] latitudeBytes = new byte[4];
            latitudeBytes[0] = data[11];
            latitudeBytes[1] = data[12];
            latitudeBytes[2] = data[13];
            latitudeBytes[3] = data[14];
            CalculateLatitude(latitudeBytes);

            // Byte 15-18 contains longitude
            byte[] longitudeBytes = new byte[4];
            longitudeBytes[0] = data[15];
            longitudeBytes[1] = data[16];
            longitudeBytes[2] = data[17];
            longitudeBytes[3] = data[18];
            CalculateLongitude(longitudeBytes);

            // Byte 19 contains speed
            this.Speed = Convert.ToInt32(string.Format("{0:x2}", data[19]), 16);

            // Byte 20-21 contains direction and GPS Status data
            this.GPSStatus = IOUtility.IsBitSet(data[20], 4);
            // Get data for direction into byte array
            byte[] bytearray = new byte[2];
            bytearray[0] = data[20];
            bytearray[1] = data[21];
            var bitArray = new BitArray(bytearray);
            // Set bit Bit7 to Bit 2 of first byte to false
            bitArray.Set(0, false);
            bitArray.Set(1, false);
            bitArray.Set(2, false);
            bitArray.Set(3, false);
            bitArray.Set(4, false);
            bitArray.Set(5, false);
            bitArray.CopyTo(bytearray, 0);
            // Get direction value from 10 bit value
            this.Direction = Convert.ToInt32(BitConverter.ToUInt16(bytearray, 0));

            // NOT Processing: Byte 22-27 is not required to process at it contains MCC, MNC and LAC

            // Byte 28-30 contains Cell ID
            StringBuilder cellidBuilder = new StringBuilder();
            for (int i = 28; i <= 30; i++)
            {
                // Convert the number expressed in base-16 to an integer.
                //int value = Convert.ToInt32(string.Format("{0:x2}", data[i]), 10);
                //cellidBuilder.Append(value);
                cellidBuilder.Append(string.Format("{0:x2}", data[i]));
            }
            this.CellId = cellidBuilder.ToString();

            // Byte 31 contains Terminal info
            // IgnitionOn	Bit 1 of terminal info
            this.IgnitionOn = IOUtility.IsBitSet(data[31], 1);

            // Byte 33 contains GSM Signal strength
            this.GsmSignalStrength = ConvertToByteWithHexBase(data[33]);

            // Byte 34-35 contains alarm information
            // Not to process as of now

            // Byte 36-37 contains serial number
            this.serialNumber = IOUtility.FlipEndian(BitConverter.ToUInt16(data, 36));
            this.serialNumberByte[0] = data[36];
            this.serialNumberByte[1] = data[37];

            this.isServerResponseRequired = true;
        }

        /// <summary>
        /// Processes heartbeat message.
        /// </summary>
        /// <param name="data"> Packet data </param>
        private void ProcessHeartbeatMessage(byte[] data)
        {
            this.DateTimeOfLog = DateTime.Now;

            // Byte 4 contains Terminal info
            byte terminalInfo = data[4];
            // IgnitionOn	Bit 1 of terminal info
            this.IgnitionOn = IOUtility.IsBitSet(terminalInfo, 1);

            // Byte 6 contains GSM Signal strength
            this.GsmSignalStrength = ConvertToByteWithHexBase(data[6]);

            // Byte 7-8 contains alarm information
            // Not to process as of now

            // Byte 9-10 contains serial number
            this.serialNumber = IOUtility.FlipEndian(BitConverter.ToUInt16(data, 9));
            this.serialNumberByte[0] = data[9];
            this.serialNumberByte[1] = data[10];

            this.isServerResponseRequired = true;
        }

        /// <summary>
        /// Processes command acknowledgement message.
        /// </summary>
        /// <param name="data"> Packet data. </param>
        private void ProcessAcknowledgementMessage(byte[] data)
        {
            // Byte 4 Command length.
            int commandLength = ConvertToByteWithHexBase(data[4]);

            // byte 5-8 Server Flag Bit
            // It is reserved to the identification of the server. The binary data received by the terminal is returned without change.

            // Command string is Command length - 4 
            int commandFinish = 9 + commandLength - 4;
            byte[] commandByteArray = new byte[commandLength - 4];
            Buffer.BlockCopy(data, 9, commandByteArray, 0, commandLength - 4);
            this.recievedCommandText = Encoding.UTF8.GetString(commandByteArray);

            // Byte commandFinish will have 2 bytes for language
            // Just consider second 
            this.LanguageCode = ConvertToByteWithHexBase(data[commandFinish + 1]);
            commandFinish = commandFinish + 2;

            // serial number
            this.serialNumber = IOUtility.FlipEndian(BitConverter.ToUInt16(data, commandFinish));
            this.serialNumberByte[0] = data[commandFinish];
            this.serialNumberByte[1] = data[commandFinish + 1];
            commandFinish = commandFinish + 2;

            this.isServerResponseRequired = true;
        }

        #endregion

        #region Helper methods

        /// <summary>
        /// Convert to byte value with hex as base.
        /// </summary>
        /// <param name="data"> byte data in hex format. </param>
        /// <returns> byte data in converted format. </returns>
        private static byte ConvertToByteWithHexBase(byte data)
        {
            return Convert.ToByte(string.Format("{0:x2}", data), 16);
        }

        /// <summary>
        /// Gets the crc16 for data packet.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        private ushort GetCrc16(byte[] data, int offset, int length)
        {
            ushort fcs = 0xFFFF;
            for (int i = offset; i < length + offset; i++)
            {
                fcs = (ushort)((ushort)(fcs >> 8) ^ crctab16[(fcs ^ data[i]) & 0xFF]);
            }

            return (ushort)(~fcs);
        }


        /// <summary>
        /// Calculates longitude for the bytes.
        /// </summary>
        /// <param name="longitudeBytes"> Longitude bytes. </param>
        private void CalculateLongitude(byte[] longitudeBytes)
        {
            // If the system architecture is little-endian (that is, little end first),
            // reverse the byte array.
            if (BitConverter.IsLittleEndian)
                Array.Reverse(longitudeBytes);
            int lon = BitConverter.ToInt32(longitudeBytes, 0);
            this.Longitude = (Single)(lon / 1800000.0);
        }

        /// <summary>
        /// Calculates latitude for the bytes.
        /// </summary>
        /// <param name="longitudeBytes"> Latitude bytes. </param>
        private void CalculateLatitude(byte[] latitudeBytes)
        {
            // If the system architecture is little-endian (that is, little end first),
            // reverse the byte array.
            if (BitConverter.IsLittleEndian)
                Array.Reverse(latitudeBytes);
            int lat = BitConverter.ToInt32(latitudeBytes, 0);
            this.Latitude = (Single)(lat / 1800000.0);
        }

        #endregion

        #endregion
    }
}
