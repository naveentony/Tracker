using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Tracker.GT06N.Models;

namespace Tracker.GT06N.Shared
{
    public class ServiceUtility
    {
        public int command_expiry_time { get; set; }

        public ServiceUtility()
        {
            command_expiry_time = 300;
        }

        public byte[] readStream(TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            if (stream.DataAvailable)
            {
                byte[] data = new byte[client.Available];

                int bytesRead = 0;
                try
                {
                    bytesRead = stream.Read(data, 0, data.Length);
                }
                catch (IOException)
                {
                }

                if (bytesRead < data.Length)
                {
                    byte[] lastData = data;
                    data = new byte[bytesRead];
                    Array.ConstrainedCopy(lastData, 0, data, 0, bytesRead);
                }
                return data;
            }
            return null;
        }


        /// <summary>
        /// Reads string in UTF8 format
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public string readString(TcpClient client)
        {
            return Encoding.UTF8.GetString(readStream(client));
        }


        /// <summary>
        /// Convert received date and time string to DateTime object
        /// </summary>
        /// <param name="dateString"></param>
        /// <param name="timeString"></param>
        /// <returns></returns>
        public DateTime ConvertDate(string dateString, string timeString)
        {
            string dateTimeString = string.Empty;
            if (dateString.Length == 6)
            {
                string dd = dateString.Substring(0, 2);
                string mm = dateString.Substring(2, 2);
                string yy = dateString.Substring(4, 2);
                string HH = timeString.Substring(0, 2);
                string MM = timeString.Substring(2, 2);
                string SS = timeString.Substring(4, 2);
                dateString = "20" + yy + "-" + mm + "-" + dd;
                dateTimeString = "20" + yy + "-" + mm + "-" + dd + " " + HH + ":" + MM + ":" + SS;
            }
            if (dateString.Length == 8)
            {
                try
                {
                    string[] arr = dateString.Split('/');
                    if (int.Parse(arr[0]) > 12)
                        dateTimeString = "20" + arr[0] + "-" + arr[1] + "-" + arr[2] + " " + timeString;
                    else
                        dateTimeString = "20" + arr[2] + "-" + arr[1] + "-" + arr[0] + " " + timeString;
                }
                catch (Exception ex)
                {
                    dateTimeString = dateString + " " + timeString;
                }
            }
            DateTime dateTime = (DateTime.Parse(dateTimeString)).ToLocalTime();

            return dateTime;
        }
    }
    public class ServiceCommonContainer
    {
        private static Dictionary<string, Vehicle> vehicleList = new Dictionary<string, Vehicle>();

        private static Dictionary<string, TrackData> previousDeviceLogList = new Dictionary<string, TrackData>();

        private static Dictionary<string, DateTime> latestMessageRecievedDateList = new Dictionary<string, DateTime>();

        public static Vehicle GetVehicleDetails(string imei, ILogger<ServiceCommonContainer> logger)
        {
            return new Vehicle { RegistrationNumber = imei , DeviceNo=imei};
            //Vehicle veh = null;
            //string vehicleId = imei;
            //veh.VehicleId= vehicleId;
            //veh.RegistrationNumber = imei;
            ////// Get vehicle from list.
            ////if (vehicleList.ContainsKey(vehicleId))
            ////{
            ////    veh = vehicleList[vehicleId];
            ////}

            ////if (veh == null)
            ////{
            ////    if (logger != null)
            ////        logger.Debug("Getting previous log from DB for IMEI: " + vehicleId);

            //veh = db.GetVehicle(vehicleId);
            ////    vehicleList.Add(vehicleId, veh);
            ////}

            //return veh;
        }

        public static TrackData GetPreviousLog(TrackData currentLog, ILogger<ServiceCommonContainer> logger)
        {
            TrackData previousLog = null;
            string vehicleId = currentLog.DeviceNo;

            // Get previous log from list.
            if (previousDeviceLogList.ContainsKey(vehicleId))
            {
                previousLog = previousDeviceLogList[vehicleId];
            }

            // If previous log not found or previous log date is greater than current log then get previous log from DB.
            if (previousLog == null || previousLog.TrackDateTime > currentLog.TrackDateTime)
            {
                if (logger != null)
                {
                    logger.LogDebug("Getting previous log from DB for IMEI: " + currentLog.DeviceNo);
                }
                previousLog = db.GetPreviousTrackData(currentLog.DeviceNo, currentLog.TrackDateTime);
            }
            else
            {
                if (logger != null)
                {
                    logger.Debug("Getting previous log from Memory-Cache for IMEI: " + currentLog.VehicleId);
                }
            }

            return previousLog;
        }

        public static TrackData GetPreviousLog(Vehicle vehicle, DBManager db)
        {
            TrackData previousLog = null;
            string vehicleId = vehicle.VehicleId;

            // Get previous log from list.
            if (previousDeviceLogList.ContainsKey(vehicleId))
            {
                previousLog = previousDeviceLogList[vehicleId];
            }

            // If previous log not found then get previous log from DB.
            if (previousLog == null)
            {
                previousLog = db.GetPreviousTrackData(vehicleId, DateTime.Now);
            }

            return previousLog;
        }

        public static TrackData GetPreviousLog(string vehicleId, DateTime currDate, DBManager db, ITrackLog logger)
        {
            TrackData previousLog = null;

            previousLog = db.GetPreviousTrackData(vehicleId, currDate);

            return previousLog;
        }

        public static void SetPreviousLog(TrackData currentLog)
        {
            string vehicleId = currentLog.DeviceNo;

            // Sets current log as previous log
            if (previousDeviceLogList.ContainsKey(vehicleId))
            {
                if (previousDeviceLogList[vehicleId].TrackDateTime < currentLog.TrackDateTime)
                    previousDeviceLogList[vehicleId] = currentLog;
            }
            else
                previousDeviceLogList.Add(vehicleId, currentLog);
        }

    }
}
