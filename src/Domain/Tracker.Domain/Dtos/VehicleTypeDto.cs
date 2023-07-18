namespace Tracker.Domain.Dtos
{

    public class VehicleTypesDto
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; }
        public double Amount { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

       
    }
}
