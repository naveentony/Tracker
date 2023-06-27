using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tracker.Domain.Settings;
//using Tracker.Features.VehicleTypes;

namespace Tracker.Features.Device
{
    public class AllDeviceeQuery : IRequest<OperationResult<IEnumerable<Device>>>
    {
        public string Id { get; set; } = string.Empty;
    }
    public class AllDeviceHandler
      : IRequestHandler<AllDeviceeQuery, OperationResult<IEnumerable<Device>>>
    {

        private readonly ICollectionProvider _prov;
        public AllDeviceHandler(ICollectionProvider provider)
        {
            _prov = provider ?? throw new ArgumentNullException(nameof(_prov));

        }
        public async Task<OperationResult<IEnumerable<Device>>> Handle(AllDeviceeQuery request,
                CancellationToken cancellationToken)
        {

            var result = new OperationResult<IEnumerable<Device>>();
            var CollectionName = _prov.GetCollection<DeviceVehiclesDto>(CollectionNames.VEHICLETYPES);
            var filter = DataFilter.Filters();
            var data = await _prov.QueryByPage(CollectionName, filter);
            result.Payload = Device.ToDeviceList(data.readOnlyList);
            result.TotalPages = data.totalPages;
            result.TotalCount = data.count;
            return result;
        }


    }
}
