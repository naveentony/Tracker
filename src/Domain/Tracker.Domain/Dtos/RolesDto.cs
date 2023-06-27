using Tracker.Domain.Enums;

namespace Tracker.Domain.Dtos
{
    [CollectionName("Roles")]
    public class MongoRoleDto : MongoIdentityRole<Guid>
    {
        public string Description { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdateDate { get; set; }
       
       


    }
  
}
