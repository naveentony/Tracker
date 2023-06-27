using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tracker.Domain.Dtos;
//using Tracker.Features.VehicleTypes;

namespace Tracker.Features.Device
{

    public class Device : IRequest<OperationResult<Unit>>
    {
        public string Id { get; set; }
        public string DeviceNo { get; set; }
        public string VehicleNo { get; set; }
        public string DeviceType { get; set; }
        public string SimNo { get; set; }
        public int VehicleId { get; set; }
        public string SalesPerson { get; set; }
        public string VehicleModel { get; set; }
        public string TimeZone { get; set; }
        public int SpeedLimit { get; set; }
        public DateTime InstallationDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int DataLimit { get; set; }
        public int DeviceTypeId { get; set; }
        public string AmountStatus { get; set; }
        public Fuelinfo fuelinfo { get; set; } = new Fuelinfo();
        public float Mileage { get; set; }
        public double RenewalAmount { get; set; }
        public int RenewalDays { get; set; }
        public YesNo IsACConnected { get; set; }
        public YesNo IsFuelConnected { get; set; }
        public YesNo IsMagnetConnected { get; set; }
        public YesNo IsRelayEnabled { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public static DeviceVehiclesDto TodeviceVehiclesDto(Device request)
        {
            return new DeviceVehiclesDto
            {
                Id = request.Id,
                DeviceNo = request.DeviceNo,
                VehicleNo = request.VehicleNo,
                DeviceType = request.DeviceType,
                SimNo = request.SimNo,
                VehicleId = request.VehicleId,
                SalesPerson = request.SalesPerson,
                VehicleModel = request.VehicleModel,
                TimeZone = request.TimeZone,
                SpeedLimit = request.SpeedLimit,
                InstallationDate = request.InstallationDate,
                ExpiryDate = request.ExpiryDate,
                DataLimit = request.DataLimit,
                DeviceTypeId = request.DeviceTypeId,
                AmountStatus = request.AmountStatus,
                Mileage = request.Mileage,
                RenewalAmount = request.RenewalAmount,
                RenewalDays = request.RenewalDays,
                IsACConnected = request.IsACConnected,
                IsFuelConnected = request.IsFuelConnected,
                IsMagnetConnected = request.IsMagnetConnected,
                IsRelayEnabled = request.IsRelayEnabled,
                fuelinfo = request.fuelinfo
            };
        }
        public static Device ToAddOrUpdateDevice(DeviceVehiclesDto request)
        {
            return new Device
            {
                Id = request.Id,
                DeviceNo = request.DeviceNo,
                VehicleNo = request.VehicleNo,
                DeviceType = request.DeviceType,
                SimNo = request.SimNo,
                VehicleId = request.VehicleId,
                SalesPerson = request.SalesPerson,
                VehicleModel = request.VehicleModel,
                TimeZone = request.TimeZone,
                SpeedLimit = request.SpeedLimit,
                InstallationDate = request.InstallationDate,
                ExpiryDate = request.ExpiryDate,
                DataLimit = request.DataLimit,
                DeviceTypeId = request.DeviceTypeId,
                AmountStatus = request.AmountStatus,
                Mileage = request.Mileage,
                RenewalAmount = request.RenewalAmount,
                RenewalDays = request.RenewalDays,
                IsACConnected = request.IsACConnected,
                IsFuelConnected = request.IsFuelConnected,
                IsMagnetConnected = request.IsMagnetConnected,
                IsRelayEnabled = request.IsRelayEnabled,
                fuelinfo = request.fuelinfo
            };
        }
        public static List<Device> ToDeviceList(List<DeviceVehiclesDto> list)
        {
            var DeviceList = new List<Device>();
            list.ForEach(tt
                => DeviceList.Add(ToAddOrUpdateDevice(tt)));
            return DeviceList;
        }
    }

    public class AddDeviceHandler : IRequestHandler<Device, OperationResult<Unit>>
    {

        private readonly ICollectionProvider _prov;
        private readonly OperationResult<Unit> _result = new();
        public AddDeviceHandler(ICollectionProvider provider)
        {
            _prov = provider ?? throw new ArgumentNullException(nameof(_prov));

        }
        public async Task<OperationResult<Unit>> Handle(Device request,
                CancellationToken cancellationToken)
        {
            var result = new OperationResult<IEnumerable<Device>>();
            // Create a session object that is used when leveraging transactions
            using (var session = await _prov.GetClient().StartSessionAsync())
            {
                // Begin transaction
                session.StartTransaction();
                try
                {
                    var CollectionName = _prov.GetCollection<DeviceVehiclesDto>(CollectionNames.NewDeviceVehicle);
                    var Deviceerequest = Device.TodeviceVehiclesDto(request);
                    Deviceerequest.CreatedDate = DateTime.Now;
                    await CollectionName.InsertOneAsync(Deviceerequest).ConfigureAwait(false);

                    // Made it here without error? Let's commit the transaction
                    await session.CommitTransactionAsync();
                }
                catch (Exception ex)
                {
                    _result.AddError(ErrorCode.DatabaseOperationException, ex.Message);
                    await session.AbortTransactionAsync();
                }

            }
            return _result;
        }


    }
}

