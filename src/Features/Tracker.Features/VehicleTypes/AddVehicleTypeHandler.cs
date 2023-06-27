﻿namespace Tracker.Features.VehicleTypes
{
    public class AddOrUpdateVehicleType : IRequest<OperationResult<Unit>>
    {
        public string Vehicle { get; set; } = string.Empty;
        public double Amount { get; set; }
        public string Status { get; set; } = StatusType.Enable.ToString();
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? UpdatedDate { get; set; }
        public static VehicleTypeDto FromTrackerType(VehicleTypesResult trackerType)
        {
            return new VehicleTypeDto
            {
                Id = trackerType.Id,
                Vehicle = trackerType.Vehicle,
                Amount = trackerType.Amount,
                Status = trackerType.Status,
            };
        }

        public static VehicleTypeDto FromAddOrUpdateVehicleTypeDto(AddOrUpdateVehicleType vehicleTypeDto)
        {
            return new VehicleTypeDto
            {
                Vehicle = vehicleTypeDto.Vehicle,
                Amount = vehicleTypeDto.Amount,
                Status = vehicleTypeDto.Status
            };
        }
    }

    public class AddVehicleTypeHandler : IRequestHandler<AddOrUpdateVehicleType, OperationResult<Unit>>
    {

        private readonly ICollectionProvider _prov;
        private readonly OperationResult<Unit> _result = new();
        public AddVehicleTypeHandler(ICollectionProvider provider)
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
                VehicleTyperequest.CreatedDate = DateTime.Now;
                await CollectionName.InsertOneAsync(VehicleTyperequest).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                _result.AddError(ErrorCode.DatabaseOperationException, e.Message);
            }
            return _result;
        }


    }
}
