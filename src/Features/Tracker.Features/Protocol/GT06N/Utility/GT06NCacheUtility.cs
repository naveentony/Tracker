using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tracker.Features.Protocol.GT06N.Models;

namespace Tracker.Features.Protocol.GT06N.Utility
{
    public class GT06NCacheUtility
    {
        private static Dictionary<string, HeartbeatStatus> previousHeartbeatLogList = new Dictionary<string, HeartbeatStatus>();

        public static HeartbeatStatus GetPreviousHeartbeatStatus(string DeviceNo, DBManager db, ITrackLog logger, DateTime trackDataTime)
        {
            HeartbeatStatus previousLog = null;

            // Get previous log from list.
            if (previousHeartbeatLogList.ContainsKey(DeviceNo))
            {
                previousLog = previousHeartbeatLogList[DeviceNo];
            }

            // If previous log not found or previous log date is greater than current log then get previous log from DB.
            if (previousLog == null)
            {
                if (logger != null)
                    logger.Debug("Getting previous hearbeat status log from DB for Vehicle: " + DeviceNo);

                TrackDataDto previousTrackData = db.GetPreviousHeartbeatData(DeviceNo, trackDataTime);

                if (previousTrackData != null)
                {
                    previousLog = new HeartbeatStatus()
                    {
                        DeviceNo = previousTrackData.DeviceNumber,
                        IgnitionOn = previousTrackData.IgnitionOn,
                        BatteryStatus = previousTrackData.BatteryStatus
                    };
                }
            }
            else
            {
                if (logger != null)
                    logger.Debug("Getting previous hearbeat status log from Memory-Cache for Vechicle: " + DeviceNo);
            }

            return previousLog;
        }

        public static void SetPreviousHeartbeatStatus(HeartbeatStatus currentLog)
        {
            string vehicleId = currentLog.DeviceNo;

            // Sets current log as previous log
            if (previousHeartbeatLogList.ContainsKey(vehicleId))
                previousHeartbeatLogList[vehicleId] = currentLog;
            else
                previousHeartbeatLogList.Add(vehicleId, currentLog);
        }
    }
}
