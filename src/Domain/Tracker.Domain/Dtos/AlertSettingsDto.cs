namespace Tracker.Domain.Dtos
{
    [CollectionName("AlertSettings")]
    public class AlertSettingsDto
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string AssignId { get; set; }  // Referencing to the DeviceVehicles
        public string VehicleId { get; set; } // Referencing to the AssignVehicles
        public List<string> AlertName { get; set; }
        public List<string> AlertType { get; set; }
        public int SMSLimit { get; set; }
        public int EmailLimit { get; set; }
        public DateTime LastUpdatedDate { get; set; }
    }
   
}

