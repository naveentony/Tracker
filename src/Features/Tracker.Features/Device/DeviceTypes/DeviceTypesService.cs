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
        public async Task<OperationResult<Unit>> AddDeviceType(DeivceTypesDto deivceTypesDto)
        {
            var deviceTypes = _provider.GetCollection<DeivceTypesDto>(CollectionNames.DeviceTypes);
            await ValidateDeviceTypeAsync(deivceTypesDto, deviceTypes);
            if (_result.IsError) return _result;
            await deviceTypes.InsertOneAsync(deivceTypesDto);
            return _result;
        }
        private async Task ValidateDeviceTypeAsync(DeivceTypesDto request, IMongoCollection<DeivceTypesDto> collection)
        {
            var name = (await collection.FindAsync(x => x.Name == request.Name)).FirstOrDefault();
            if (name is not null)
                _result.AddError(ErrorCode.ValidationError, DeviceMessages.DeviceTypeAlreadyExists);
        }
        public async Task<OperationResult<Unit>> AddVehicleType(VehicleTypeDto vehicleTypDto)
        {

            var CollectionName = _provider.GetCollection<VehicleTypeDto>(CollectionNames.VEHICLETYPES);
            await ValidateVehicleTypeAsync(vehicleTypDto, CollectionName);
            if (_result.IsError) return _result;
            await CollectionName.InsertOneAsync(vehicleTypDto);
            return _result;
        }
        private async Task ValidateVehicleTypeAsync(VehicleTypeDto request, IMongoCollection<VehicleTypeDto> collection)
        {
            var name = (await collection.FindAsync(x => x.Name == request.Name)).FirstOrDefault();
            if (name is not null)
                _result.AddError(ErrorCode.ValidationError, DeviceMessages.VehicleTypeAlreadyExists);
        }

    }
}
