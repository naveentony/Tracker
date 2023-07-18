using Microsoft.AspNetCore.Identity;

namespace Tracker.Domain.Dtos
{
    [CollectionName("Users")]
    public class UsersDto: MongoIdentityUser<Guid>
    {
        public UsersDto()
        {
            AssigedUsers = new List<Guid>();
        }
        public int UserType { get; set; }
        public Client? Client { get; set; }
        public string PlanId { get; set; }
        public string address_id { get; set; }
        public string InvoicingName { get; set; }
        public object SortExpression { get; set; }
        public DateTime SiteStoppedDate { get; set; }
        public double DevicePrice { get; set; }
        public string SalesPerson { get; set; }
        public string SalesPerson_MobileNo { get; set; }
        public string FireBaseKey { get; set; }
        public DateTime Activitydate { get; set; }
        public string IsActive { get; set; } 
        public Guid ParentId { get; set; }
        public List<Guid> AssigedUsers { get; set; } 
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
    public class Client
    {
        public string Name { get; set; }
        public Logo Logo { get; set; }
        public string GSTNumber { get; set; }
        public string InvoiceName { get; set; }
        public string DisplayTitle { get; set; } 
    }
    public class Logo
    {
        public string FileName { get; set; } 
        public byte[] File { get; set; } 
    }
}
