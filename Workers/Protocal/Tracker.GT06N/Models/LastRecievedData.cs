using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tracker.GT06N.Models
{
    public class LastRecievedData
    {
        public Single Latitude { get; set; }
        public Single Longitude { get; set; }
        public bool GPSStatus { get; set; }
        public byte NoOfSatellite { get; set; }
    }
}
