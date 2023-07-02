namespace Tracker.Domain.Dtos
{
    [CollectionName("AssignVehicles")]
    public class AssignVehiclesDto
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public Guid UserId { get; set; } // Referencing Users
        public string VehicleId { get; set; } // Referencing DeviceVehicles
        public string AdminId { get; set; } // Referencing users id
        public string ClientId { get; set; } // Referencing users id
        public DateTime CreateDate { get; set; }
    }
   
}

