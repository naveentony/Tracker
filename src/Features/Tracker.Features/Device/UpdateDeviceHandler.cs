﻿namespace Tracker.Features.Device
{
    
    public class UpdateDeviceHandler : IRequestHandler<Device, OperationResult<Unit>>
    {

        private readonly ICollectionProvider _prov;
        private readonly OperationResult<Unit> _result = new();
        public UpdateDeviceHandler(ICollectionProvider provider)
        {
            _prov = provider ?? throw new ArgumentNullException(nameof(_prov));

        }
        public async Task<OperationResult<Unit>> Handle(Device request,
                CancellationToken cancellationToken)
        {
            try
            {
                var result = new OperationResult<IEnumerable<Device>>();
                var CollectionName = _prov.GetCollection<DeviceVehiclesDto>(CollectionNames.NewDeviceVehicle);
                var VehicleTyperequest = Device.TodeviceVehiclesDto(request);
                VehicleTyperequest.CreatedDate = DateTime.Now;
                //await CollectionName.UpdateOne(VehicleTyperequest).ConfigureAwait(false);
                await CollectionName.FindOneAndUpdateAsync(Builders<DeviceVehiclesDto>.Filter.Eq("Id", ObjectId.Parse(request.Id)),
                                    Builders<DeviceVehiclesDto>.Update.Set("DeviceNo", request.DeviceNo)
                                   .Set("DeviceNo", request.DeviceNo)
                                   .Set("DeviceType", request.DeviceType)
                                   .Set("SimNo", request.SimNo)
                                   .Set("VehicleId", request.VehicleId)
                                   .Set("SalesPerson", request.SalesPerson)
                                   .Set("VehicleModel", request.VehicleModel)
                                   .Set("TimeZone", request.TimeZone)
                                   .Set("SpeedLimit", request.SpeedLimit)
                                   .Set("InstallationDate", request.InstallationDate)
                                   .Set("ExpiryDate", request.ExpiryDate)
                                   .Set("DataLimit", request.DataLimit)
                                   .Set("DeviceTypeId", request.DeviceTypeId)
                                   .Set("AmountStatus", request.AmountStatus)
                                   .Set("fuelinfo", request.fuelinfo)
                                   .Set("Mileage", request.Mileage)
                                   .Set("RenewalAmount", request.RenewalAmount)
                                   .Set("RenewalDays", request.RenewalDays)
                                   .Set("IsACConnected", request.IsACConnected)
                                   .Set("IsFuelConnected", request.IsFuelConnected)
                                   .Set("IsMagnetConnected", request.IsMagnetConnected)
                                   .Set("IsRelayEnabled", request.IsRelayEnabled)).ConfigureAwait(false); ;
              
            }
            catch (Exception e)
            {
                _result.AddError(ErrorCode.DatabaseOperationException, e.Message);
            }
            return _result;
        }


    }
}