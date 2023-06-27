using MongoDB.Driver;

namespace Tracker.Domain.Settings
{
    public class DataFilter
    {
        //public DataFilter(int pageNumber,int pageSize,string filterId,string orderBy,string columnName,string columnValue) { 
        // PageNumber = pageNumber;
        //    PageSize = pageSize;
        //    FilterID = filterId;
        //    Orderby = orderBy;
        //    ColumnName = columnName;
        //    ColumnValue = columnValue;
        //}
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string FilterID { get; set; }
        public string Orderby { get; set; }
        public string ColumnName { get; set; }
        public string ColumnValue { get; set; }

        public static DataFilter Filters() {
            return new DataFilter { PageNumber = 1, PageSize = 10, Orderby="Name" };
        }
    }
}
