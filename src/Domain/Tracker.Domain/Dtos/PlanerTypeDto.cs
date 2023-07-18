namespace Tracker.Domain.Dtos
{
    public class PlanerTypesDto
    {
        public int Id { get; set; }
        public int Name { get; set; }
        public int DeviceCost { get; set; }
        public int RenewalCost { get; set; }
        public int RenewalDays { get; set; }
        public int IsActive { get; set; }
        public int IsDeleted { get; set; }
        public int CreatedDate { get; set; }
        public int UpdatedDate { get; set; }
    }
}
