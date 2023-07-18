namespace Tracker.Features.Device.Vehicles
{
    public class VehicleDelete : IRequest<OperationResult<Unit>>
    {
        public string Id { get; set; }
    }


    public class VehicleDeleteHandler : IRequestHandler<VehicleDelete, OperationResult<Unit>>
    {

        private readonly ICollectionProvider _prov;
        private readonly OperationResult<Unit> _result = new();
        public VehicleDeleteHandler(ICollectionProvider provider)
        {
            _prov = provider ?? throw new ArgumentNullException(nameof(_prov));

        }
        public async Task<OperationResult<Unit>> Handle(VehicleDelete request,
                CancellationToken cancellationToken)
        {
            try
            {
                var result = new OperationResult<IEnumerable<VehicleRegister>>();
                var CollectionName = _prov.GetCollection<VehiclesDto>(CollectionNames.Vehicles);
                await CollectionName.DeleteOneAsync(Builders<VehiclesDto>.Filter.Eq("Id", ObjectId.Parse(request.Id)));//.ConfigureAwait(false);
            }
            catch (Exception e)
            {
                _result.AddError(ErrorCode.DatabaseOperationException, e.Message);
            }
            return _result;
        }

    }
}
