using Microsoft.AspNetCore.Identity;
using MongoDbGenericRepository.Utils;
using System.Text.Json.Serialization;

namespace Tracker.Domain.Dtos
{
    [CollectionName("Clients")]
    [BsonIgnoreExtraElements]
    public class ClientsDto: IdentityUser<Guid>
    {
        public string Name { get; set; }
        public Logo logo { get; set; }=new Logo();
        public string GSTNumber { get; set; }=string.Empty;
        public string InvoiceName { get; set; } = string.Empty;
        public string DisplayTitle { get; set; } = string.Empty;
        public string IsActive { get; set; } = string.Empty;
        public DateTime CreateDate { get; set; } = DateTime.UtcNow;
    }
   
}

