namespace Tracker.Features.VehicleTypes
{

    public class RemoveVehicleType : IRequest<OperationResult<Unit>>
    {
        public string Id { get; set; } = string.Empty;
    }

    public class RemoveVehicleTypeHandler : IRequestHandler<RemoveVehicleType, OperationResult<Unit>>
    {

        private readonly ICollectionProvider _prov;
        private readonly OperationResult<Unit> _result = new();
        public RemoveVehicleTypeHandler(ICollectionProvider provider)
        {
            _prov = provider ?? throw new ArgumentNullException(nameof(_prov));

        }
        public async Task<OperationResult<Unit>> Handle(RemoveVehicleType request,
                CancellationToken cancellationToken)
        {
            try
            {
                var result = new OperationResult<IEnumerable<RemoveVehicleType>>();
                var CollectionName = _prov.GetCollection<VehicleTypeDto>(CollectionNames.VEHICLETYPES);
                //var VehicleTyperequest = VehicleTypeDto.FromAddOrUpdateVehicleTypeDto(request);
                //VehicleTyperequest.CreatedDate = DateTime.Now;
                await CollectionName.DeleteOneAsync(Builders<VehicleTypeDto>
                                    .Filter.Eq("Id", ObjectId.Parse(request.Id))).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                _result.AddError(ErrorCode.DatabaseOperationException, e.Message);
            }
            return _result;
        }


    }
}
