using System.Text.Json.Serialization;

namespace Tracker.Domain.Dtos
{
    [CollectionName("Clients")]
    public class ClientsDto
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; }
        public Logo logo { get; set; }=new Logo();
        public string GSTNumber { get; set; }=string.Empty;
        public string InvoiceName { get; set; } = string.Empty;
        public string DisplayTitle { get; set; } = string.Empty;
        public string IsActive { get; set; } = string.Empty;
        public DateTime CreateDate { get; set; } = DateTime.UtcNow;
    }
    public class Logo
    {
        public string fileName { get; set; } = string.Empty;
        public byte[] file { get; set; }=new byte[0];
    }
}

