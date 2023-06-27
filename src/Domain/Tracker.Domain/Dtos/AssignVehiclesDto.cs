namespace Tracker.Domain.Dtos
{
    [CollectionName("AssignVehicles")]
    public class AssignVehiclesDto
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public Guid Id { get; set; }
        public Guid UserId { get; set; } // Referencing Users
        public Guid VehicleId { get; set; } // Referencing DeviceVehicles
        public DateTime CreateDate { get; set; }
    }
}

