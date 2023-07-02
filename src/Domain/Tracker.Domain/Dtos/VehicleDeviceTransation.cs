namespace Tracker.Domain.Dtos
{
    public class VehicleDeviceTransation
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string VehicleId { get; set; }  // Referencing to the DeviceVehicles
        public DateTime StartDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public double AmountPaid { get; set; }
        public long PaymentId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
    
}

