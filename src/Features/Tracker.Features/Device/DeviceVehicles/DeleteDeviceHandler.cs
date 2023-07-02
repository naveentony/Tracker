namespace Tracker.Features.Device.DeviceVehicles
{
    public class DeleteDevice : IRequest<OperationResult<Unit>>
    {
        public string Id { get; set; }
    }


    public class DeleteDeviceHandler : IRequestHandler<DeleteDevice, OperationResult<Unit>>
    {

        private readonly ICollectionProvider _prov;
        private readonly OperationResult<Unit> _result = new();
        public DeleteDeviceHandler(ICollectionProvider provider)
        {
            _prov = provider ?? throw new ArgumentNullException(nameof(_prov));

        }
        public async Task<OperationResult<Unit>> Handle(DeleteDevice request,
                CancellationToken cancellationToken)
        {
            try
            {
                var result = new OperationResult<IEnumerable<DeviceRegister>>();
                var CollectionName = _prov.GetCollection<DeviceVehiclesDto>(CollectionNames.NewDeviceVehicles);
                await CollectionName.DeleteOneAsync(Builders<DeviceVehiclesDto>.Filter.Eq("Id", ObjectId.Parse(request.Id)));//.ConfigureAwait(false);
            }
            catch (Exception e)
            {
                _result.AddError(ErrorCode.DatabaseOperationException, e.Message);
            }
            return _result;
        }

    }
}
