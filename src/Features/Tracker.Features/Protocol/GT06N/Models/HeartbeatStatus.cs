using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tracker.Features.Protocol.GT06N.Models
{
    public class HeartbeatStatus
    {
        private string deviceNo;

        public string DeviceNo
        {
            get { return deviceNo; }
            set { DeviceNo = value; }
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

    }
}
