using Tracker.Domain.Dtos;

namespace Tracker.Features.Device.DeviceTypes
{
    public class DeviceTypesService
    {
        private readonly CollectionProvider _provider;
        //public HttpContext _httpContext => new HttpContextAccessor().HttpContext;
        private readonly OperationResult<Unit> _result = new();
        public DeviceTypesService(CollectionProvider provider)
        {
            _provider = provider;
        }
        public async Task<OperationResult<Unit>> AddDeviceType(TrackerTypesDto deivceTypesDto)
        {
            var deviceTypes = _provider.GetCollection<TrackerTypesDto>(CollectionNames.TrackerTypes);
            await ValidateDeviceTypeAsync(deivceTypesDto, deviceTypes);
            if (_result.IsError) return _result;
            await deviceTypes.InsertOneAsync(deivceTypesDto);
            return _result;
        }
        private async Task ValidateDeviceTypeAsync(TrackerTypesDto request, IMongoCollection<TrackerTypesDto> collection)
        {
            var name = (await collection.FindAsync(x => x.Name == request.Name)).FirstOrDefault();
            if (name is not null)
                _result.AddError(ErrorCode.ValidationError, DeviceMessages.DeviceTypeAlreadyExists);
        }
        public async Task<OperationResult<Unit>> AddVehicleType(VehicleTypesDto vehicleTypDto)
        {

            var CollectionName = _provider.GetCollection<VehicleTypesDto>(CollectionNames.VEHICLETYPES);
            await ValidateVehicleTypeAsync(vehicleTypDto, CollectionName);
            if (_result.IsError) return _result;
            await CollectionName.InsertOneAsync(vehicleTypDto);
            return _result;
        }
        private async Task ValidateVehicleTypeAsync(VehicleTypesDto request, IMongoCollection<VehicleTypesDto> collection)
        {
            var name = (await collection.FindAsync(x => x.Name == request.Name)).FirstOrDefault();
            if (name is not null)
                _result.AddError(ErrorCode.ValidationError, DeviceMessages.VehicleTypeAlreadyExists);
        }
        public async Task<OperationResult<Unit>> AddNewDevice(VehiclesDto request)
        {

            var CollectionName = _provider.GetCollection<VehiclesDto>(CollectionNames.Vehicles);
            await ValidateDeviceAsync(request, CollectionName);
            if (_result.IsError) return _result;
            //  var Deviceerequest = DeviceRegister.TodeviceVehiclesDto(request);
            request.CreatedDateTime = DateTime.Now;
            await CollectionName.InsertOneAsync(request).ConfigureAwait(false);
            return _result;
        }
        public async Task<OperationResult<Unit>> AddTrackerTypeData(List<TrackerTypesDto> request)
        {

            var CollectionName = _provider.GetCollection<TrackerTypesDto>("TrackerTypes");

            await CollectionName.InsertManyAsync(request).ConfigureAwait(false);
            return _result;
        }
        public async Task<OperationResult<Unit>> AddTrackerData(List<TrackerDataDto> request)
        {

            var CollectionName = _provider.GetCollection<TrackerDataDto>("TrackerData");
           
            await CollectionName.InsertManyAsync(request).ConfigureAwait(false);
            return _result;
        }
        public async Task<OperationResult<Unit>> AddDeviceTypesData(List<TrackerTypesDto> request)
        {

            var CollectionName = _provider.GetCollection<TrackerTypesDto>("DeviceTypes");

            await CollectionName.InsertManyAsync(request).ConfigureAwait(false);
            return _result;
        }
        public async Task<OperationResult<Unit>> AddTrackerDataLive(List<TrackerDataLiveDto> request)
        {

            var CollectionName = _provider.GetCollection<TrackerDataLiveDto>("TrackerDataLive");

            await CollectionName.InsertManyAsync(request).ConfigureAwait(false);
            return _result;
        }
        public async Task<OperationResult<Unit>> AddVehiclesData(List<VehiclesDto> request)
        {

            var CollectionName = _provider.GetCollection<VehiclesDto>("Vehicles");

            await CollectionName.InsertManyAsync(request).ConfigureAwait(false);
            return _result;
        }

        private async Task ValidateDeviceAsync(VehiclesDto request, IMongoCollection<VehiclesDto> collection)
        {
            var deviceNo = (await collection.FindAsync(x => x.IMEI == request.IMEI)).FirstOrDefault();
            if (deviceNo is not null)
                _result.AddError(ErrorCode.ValidationError, DeviceMessages.DeviceNumberAlreadyExists);
            var SimNo = (await collection.FindAsync(x => x.SimNo == request.SimNo)).FirstOrDefault();
            if (SimNo is not null)
                _result.AddError(ErrorCode.ValidationError, DeviceMessages.SimNumberAlreadyExists);

        }

    }
}
