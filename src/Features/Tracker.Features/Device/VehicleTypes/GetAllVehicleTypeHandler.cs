using Tracker.Domain.Settings;

namespace Tracker.Features.Device.VehicleTypes
{
    public class VehicleTypesResult
    {
        public string Id { get; set; } = string.Empty;
        public string Vehicle { get; set; } = string.Empty;
        public double Amount { get; set; }
        public string Status { get; set; } = StatusType.Enable.ToString();
        public static VehicleTypesResult FromTrackerTypeDto(VehicleTypeDto trackerTypedto)
        {
            return new VehicleTypesResult
            {
                Id = trackerTypedto.Id,
                Vehicle = trackerTypedto.Name,
                Amount = trackerTypedto.Amount,
                Status = trackerTypedto.Status

            };
        }
        public static List<VehicleTypesResult> FromTrackerTypeList(List<VehicleTypeDto> list)
        {
            var trackerTypes = new List<VehicleTypesResult>();
            list.ForEach(tt
                => trackerTypes.Add(FromTrackerTypeDto(tt)));
            return trackerTypes;
        }
    }
    public class GetAllVehicleTypeQuery : IRequest<OperationResult<IEnumerable<VehicleTypesResult>>>
    {
        public string Id { get; set; } = string.Empty;
    }
    public class GetAllVehicleTypeHandler
        : IRequestHandler<GetAllVehicleTypeQuery, OperationResult<IEnumerable<VehicleTypesResult>>>
    {

        private readonly ICollectionProvider _prov;
        public GetAllVehicleTypeHandler(ICollectionProvider provider)
        {
            _prov = provider ?? throw new ArgumentNullException(nameof(_prov));

        }
        public async Task<OperationResult<IEnumerable<VehicleTypesResult>>> Handle(GetAllVehicleTypeQuery request,
                CancellationToken cancellationToken)
        {

            var result = new OperationResult<IEnumerable<VehicleTypesResult>>();
            var CollectionName = _prov.GetCollection<VehicleTypeDto>(CollectionNames.VEHICLETYPES);
            var filter = DataFilter.Filters();
            var data = await _prov.QueryByPage(CollectionName, filter);
            result.Payload = VehicleTypesResult.FromTrackerTypeList(data.readOnlyList);
            result.TotalPages = data.totalPages;
            result.TotalCount = data.count;
            return result;
        }


    }
}

