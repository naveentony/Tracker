using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tracker.Domain.Dtos
{
    public class DeivceTypesDto
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string TrackerName { get; set; } = string.Empty;
        public string TrackerDescription { get; set; } = string.Empty;
        public string Command1 { get; set; }
        public string Command2 { get; set; }
        public string Command3 { get; set; }
        public string Command4 { get; set; }
        public string Command5 { get; set; }
    }
}
