
using System.Data;
using Tracker.Domain.Dtos;

namespace Tracker.ImportData.Managers
{
    public  class DBManager
    {
       
        //public HttpContext _httpContext => new HttpContextAccessor().HttpContext;
        //public DBManager(CollectionProvider provider)
        //{
        //    _provider = provider;
        //    _deviceTypesService = deviceTypesService;
        //}
        public  List<DeivceTypesDto> LoadDeivceTypesData()
        {
            var responce = new List<DeivceTypesDto>();
            DataSet result =  DBHelper.Executetext("select * from TrackerTb");
            foreach (DataRow row in result.Tables[0].Rows)
            {

                var obj = new DeivceTypesDto();
                obj.Name = row["TrackerName"].ToString();
                obj.Description = row["TrackerDescription"].ToString();
                obj.Command1 = row["Command1"].ToString();
                obj.Command2 = row["Command2"].ToString();
                obj.Command3 = row["Command3"].ToString();
                obj.Command4 = row["Command4"].ToString();
                obj.CreatedDate = DateTime.UtcNow;
                responce.Add(obj);
            }
            return responce;
        }
        public List<VehicleTypeDto> LoadVehicleTypesData()
        {
            var tlist = new List<VehicleTypeDto>();
            DataSet result = DBHelper.Executetext("select * from VehicleTypes");
            foreach (DataRow row in result.Tables[0].Rows)
            {
                var obj = new VehicleTypeDto();
                    obj.Name = row["Vehicle"].ToString();
                obj.Amount = Convert.ToDouble( row["Amount"].ToString());
                obj.Status = row["status"].ToString() == "True" ? "Enable" : "Disable";
                obj.CreatedDate = DateTime.UtcNow;
                tlist.Add(obj);
            }
            return tlist;
        }
    }

}
