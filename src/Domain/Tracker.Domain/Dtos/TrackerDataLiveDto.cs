namespace Tracker.Domain.Dtos
{
    [CollectionName("TrackDataLive")]
    public class TrackerDataLiveDto : TrackerDataDto
    {
        public double BatteryVoltage { get; set; }
        public bool BatteryStatus { get; set; }
        public int PortNumber { get; set; }
        public bool IsRentEnable { get; set; }
        public VehicleData? vehicleData { get; set; }
        public AdditionalParameters? additionalParameters { get; set; }=new AdditionalParameters();
    }

}
