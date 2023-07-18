using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tracker.Domain.Settings;

namespace Tracker.Features.Device.Vehicles
{
    public class AllVehiclesQuery : IRequest<OperationResult<IEnumerable<VehicleRegister>>>
    {
        public string UserId { get; set; }
        public string DeviceNo { get; set; } 
    }
    public class AllVehiclesHandler
      : IRequestHandler<AllVehiclesQuery, OperationResult<IEnumerable<VehicleRegister>>>
    {
        public HttpContext _httpContext => new HttpContextAccessor().HttpContext;
        private readonly ICollectionProvider _prov;
        OperationResult<IEnumerable<VehicleRegister>> result = new ();
        public AllVehiclesHandler(ICollectionProvider provider)
        {
            _prov = provider ?? throw new ArgumentNullException(nameof(_prov));

        }
        public async Task<OperationResult<IEnumerable<VehicleRegister>>> Handle(AllVehiclesQuery request,
                CancellationToken cancellationToken)
        {

            //var result = GetData();
            var CollectionName = _prov.GetCollection<VehiclesDto>(CollectionNames.Vehicles);
            var UserType = _httpContext.GetUserType();
            if (request.DeviceNo is not null)
              await GetDeviceDetailsByDeviceNo(CollectionName, request);
            else
            {
                if (UserType == "0")
                  await  GetAllDeviceData(CollectionName, request);
            }
            return result;
        }
        private async Task GetDeviceDetailsByDeviceNo(IMongoCollection<VehiclesDto> collection, AllVehiclesQuery request)
        {
            var filter = DataFilter.Filters("DeviceNo", request.DeviceNo);
            var data = await _prov.QueryByPage(collection, filter);
            result.Payload = VehicleRegister.ToDeviceList(data.readOnlyList);
            result.TotalPages = data.totalPages;
            result.TotalCount = data.count;
        }

        //private async Task GetDeviceDetailsByDeviceNo1()
        //{

        //    var UserType = _httpContext.GetUserType();
        //    var CollectionName = _prov.GetCollection<DeviceVehiclesDto>(CollectionNames.DeviceVehicles);
        //    if (UserType == "0")
        //    {

        //    }
        //    else if (UserType == "1") { }
        //    else if (UserType == "2") { }
        //    else if (UserType == "3") { }
        //    else if (UserType == "4") { }
        //}
        private async Task GetAllDeviceData(IMongoCollection<VehiclesDto> collection, AllVehiclesQuery request)
        {
            var UserType = Convert.ToInt32(_httpContext.GetUserType());
            if (UserType == 0){
                var filter = DataFilter.Filters();
                var data = await _prov.QueryByPage(collection, filter);
                result.Payload = VehicleRegister.ToDeviceList(data.readOnlyList);
                result.TotalPages = data.totalPages;
                result.TotalCount = data.count;
            }
        }
        private OperationResult<IEnumerable<VehicleRegister>> GetDeviceDataByUserId(string UserID)
        {  
            var UserType= _httpContext.GetUserType();
            var CollectionName = _prov.GetCollection<VehiclesDto>(CollectionNames.Vehicles);
            return result;
        }



    }
}
