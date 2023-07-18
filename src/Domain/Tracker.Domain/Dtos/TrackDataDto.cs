using MongoDB.Bson;
using MongoDB.Driver.GeoJsonObjectModel;

namespace Tracker.Domain.Dtos
{
    [CollectionName("TrackerData")]
    public class TrackerDataDto
    {
        public TrackerDataDto() { }
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string IMEI { get; set; }
        public string SoftwareVersion { get; set; }
        public string ProfileName { get; set; }
        public bool GPSStatus { get; set; }
        public int SignalStrength { get; set; }
        public DateTime TrackDateTime { get; set; }
        public GeoJsonPoint<GeoJson2DGeographicCoordinates> location { get; set; }
        public string LocationName { get; set; }
        public int Altitude { get; set; }
        public int Speed { get; set; }
        public int Direction { get; set; }
        public int Satellite { get; set; }
        public double GPSPositionAccuracyIndication { get; set; }
        public double MilageReading { get; set; }
        public string Cell { get; set; }
        public double Analog1 { get; set; }
        public double Analog2 { get; set; }
        public double Analog3 { get; set; }
        public double Analog4 { get; set; }
        public bool DigitalInputLevel1 { get; set; }
        public bool DigitalInputLevel2 { get; set; } //AC
        public bool DigitalInputLevel3 { get; set; }
        public bool DigitalInputLevel4 { get; set; }
        public bool DigitalOutputLevel1 { get; set; }
        public bool DigitalOutputLevel2 { get; set; }
        public bool DigitalOutputLevel3 { get; set; }
        public bool DigitalOutputLevel4 { get; set; }
        public ObjectId? Vehicles_VehicleId { get; set; } = new ObjectId();
        public int InfoNumber { get; set; }
        public bool HarshDetecation { get; set; }
        public string RFID { get; set; }
        public bool IsIgnitionOn { get; set; }
        public FuelData fuelData { get; set; }
        public DateTime CreatedDateTime { get; set; }
    }
    public class FuelData
    {
        public double RawVoltage { get; set; }
        public double Voltage { get; set; }
        public double FuelReading { get; set; }
    }
   
    public class VehicleData
    {
        public int VehicleStatus { get; set; }
        public DateTime VehicleStatusSince { get; set; }
        public DateTime VehicleRunningSince { get; set; }
        public bool IsTripActive { get; set; }
        public int ActiveTripId { get; set; }
        public int ParkingMode { get; set; }
    }
    public class AdditionalParameters
    {
        public string ParameterId { get; set; }
        public string ParameterName { get; set; }
        public int ParameterValue { get; set; }
    }
    //public string[] pranm =["001",

}
