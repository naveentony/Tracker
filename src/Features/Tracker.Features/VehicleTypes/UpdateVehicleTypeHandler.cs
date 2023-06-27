namespace Tracker.Features.VehicleTypes
{

    public class UpdateVehicleTypeHandler : IRequestHandler<AddOrUpdateVehicleType, OperationResult<Unit>>
    {

        private readonly ICollectionProvider _prov;
        private readonly OperationResult<Unit> _result = new();
        public UpdateVehicleTypeHandler(ICollectionProvider provider)
        {
            _prov = provider ?? throw new ArgumentNullException(nameof(_prov));

        }
        public async Task<OperationResult<Unit>> Handle(AddOrUpdateVehicleType request,
                CancellationToken cancellationToken)
        {
            try
            {
                var result = new OperationResult<IEnumerable<AddOrUpdateVehicleType>>();
                var CollectionName = _prov.GetCollection<VehicleTypeDto>(CollectionNames.VEHICLETYPES);
                var VehicleTyperequest = AddOrUpdateVehicleType.FromAddOrUpdateVehicleTypeDto(request);
                VehicleTyperequest.UpdatedDate = DateTime.Now;
                await CollectionName.FindOneAndUpdateAsync(Builders<VehicleTypeDto>
                                    .Filter.Eq("Id", ObjectId.Parse(VehicleTyperequest.Id)),
                                     Builders<VehicleTypeDto>.Update.Set("Vehicle", VehicleTyperequest.Vehicle)
                                    .Set("Amount", VehicleTyperequest.Amount)
                                    .Set("Status", VehicleTyperequest.Status)
                                    .Set("UpdatedDate", VehicleTyperequest.UpdatedDate)).ConfigureAwait(false); ;
            }
            catch (Exception e)
            {
                _result.AddError(ErrorCode.DatabaseOperationException, e.Message);
            }
            return _result;

        }


    }

}
