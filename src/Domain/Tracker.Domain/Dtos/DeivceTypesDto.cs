using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tracker.Domain.Dtos
{
    [CollectionName("DeivceTypes")]
    public class DeivceTypesDto
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Command1 { get; set; } = string.Empty;
        public string Command2 { get; set; } = string.Empty;
        public string Command3 { get; set; } = string.Empty;
        public string Command4 { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedDate { get; set; }= DateTime.UtcNow;
    }
}
