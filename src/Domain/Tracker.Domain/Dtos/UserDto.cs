namespace Tracker.Domain.Dtos
{
    [CollectionName("Users")]
    public class UsersDto: MongoIdentityUser<Guid>
    {

       
        public int UserType { get; set; }
        public string ClientID { get; set; }
        public string ManagerId { get; set; }
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
        public bool UserStatus { get; set; }
        public List<string> RoleIds { get; set; } = new List<string>();

        //public List<IdentityUserClaim<string>> Claims { get; set; } = new List<IdentityUserClaim<string>>();

        //public List<IdentityUserLogin<string>> Logins { get; set; } = new List<IdentityUserLogin<string>>();

        //public List<IdentityUserToken<string>> Tokens { get; set; } = new List<IdentityUserToken<string>>();
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        
    }
}
