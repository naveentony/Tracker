using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tracker.Domain.Settings;
//using Tracker.Features.VehicleTypes;

namespace Tracker.Features.Device.DeviceVehicles
{
    public class AllDeviceeQuery : IRequest<OperationResult<IEnumerable<DeviceRegister>>>
    {
        public string Id { get; set; } = string.Empty;
    }
    public class AllDeviceHandler
      : IRequestHandler<AllDeviceeQuery, OperationResult<IEnumerable<DeviceRegister>>>
    {

        private readonly ICollectionProvider _prov;
        public AllDeviceHandler(ICollectionProvider provider)
        {
            _prov = provider ?? throw new ArgumentNullException(nameof(_prov));

        }
        public async Task<OperationResult<IEnumerable<DeviceRegister>>> Handle(AllDeviceeQuery request,
                CancellationToken cancellationToken)
        {

            var result = new OperationResult<IEnumerable<DeviceRegister>>();
            var CollectionName = _prov.GetCollection<DeviceVehiclesDto>(CollectionNames.VEHICLETYPES);
            var filter = DataFilter.Filters();
            var data = await _prov.QueryByPage(CollectionName, filter);
            result.Payload = DeviceRegister.ToDeviceList(data.readOnlyList);
            result.TotalPages = data.totalPages;
            result.TotalCount = data.count;
            return result;
        }


    }
}
