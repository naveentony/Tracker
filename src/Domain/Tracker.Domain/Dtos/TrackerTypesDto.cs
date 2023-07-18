using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tracker.Domain.Dtos
{
    [CollectionName("TrackerTypes")]
    public class TrackerTypesDto
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        public string Name { get; set; } 
        public string CompanyName { get; set; } 
        public string UpdateRates { get; set; } 
        public double VRef { get; set; } 
        public double Offset { get; set; }
        public int DefaultUpdateRate { get; set; }
        public double ZeroSpeed { get; set; } 
        public DateTime CreatedDate { get; set; } 
        public DateTime UpdatedDate { get; set; }
    }
}
