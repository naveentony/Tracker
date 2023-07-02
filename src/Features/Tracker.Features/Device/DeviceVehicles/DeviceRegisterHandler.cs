

namespace Tracker.Features.Device.DeviceVehicles
{

    public class DeviceRegister : IRequest<OperationResult<Unit>>
    {
        public string DeviceNo { get; set; }
        public string VehicleNo { get; set; }
        public string DeviceTypeId { get; set; }
        public string SimNo { get; set; }
        public string VehicleTypeId { get; set; }
        public string SalesPerson { get; set; }
        public string VehicleModel { get; set; }
        public string TimeZone { get; set; }
        public int SpeedLimit { get; set; }
        public DateTime InstallationDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int DataLimit { get; set; }
        public string AmountStatus { get; set; }
        public Fuelinfo fuelinfo { get; set; } = new Fuelinfo();
        public float Mileage { get; set; }
        public double RenewalAmount { get; set; }
        public int RenewalDays { get; set; }
        public string IsACConnected { get; set; }
        public string IsFuelConnected { get; set; }
        public string IsMagnetConnected { get; set; }
        public string IsRelayEnabled { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public static DeviceVehiclesDto TodeviceVehiclesDto(DeviceRegister request)
        {
            return new DeviceVehiclesDto
            {
                // Id = request.Id,
                DeviceNo = request.DeviceNo,
                VehicleNo = request.VehicleNo,
                DeviceTypeId = request.DeviceTypeId,
                SimNo = request.SimNo,
                //ve = request.VehicleId,
                SalesPerson = request.SalesPerson,
                VehicleModel = request.VehicleModel,
                TimeZone = request.TimeZone,
                SpeedLimit = request.SpeedLimit,
                CreatedDate = request.InstallationDate,
                ExpiryDate = request.ExpiryDate,
                DataLimit = request.DataLimit,
                AmountStatus = request.AmountStatus,
                Mileage = request.Mileage,
                RenewalAmount = request.RenewalAmount,
                RenewalDays = request.RenewalDays,
                IsACConnected = request.IsACConnected.ToString(),
                IsFuelConnected = request.IsFuelConnected.ToString(),
                IsMagnetConnected = request.IsMagnetConnected,
                IsRelayEnabled = request.IsRelayEnabled,
                fuelinfo = request.fuelinfo
            };
        }
        public static DeviceRegister ToAddOrUpdateDevice(DeviceVehiclesDto request)
        {
            return new DeviceRegister
            {
                // Id = request.Id,
                DeviceNo = request.DeviceNo,
                VehicleNo = request.VehicleNo,
                DeviceTypeId = request.DeviceTypeId,
                SimNo = request.SimNo,
                SalesPerson = request.SalesPerson,
                VehicleModel = request.VehicleModel,
                TimeZone = request.TimeZone,
                SpeedLimit = request.SpeedLimit,
                InstallationDate = request.CreatedDate,
                ExpiryDate = request.ExpiryDate,
                DataLimit = request.DataLimit,
                AmountStatus = request.AmountStatus,
                Mileage = request.Mileage,
                RenewalAmount = request.RenewalAmount,
                RenewalDays = request.RenewalDays,
                IsACConnected = request.IsACConnected.ToString(),
                IsFuelConnected = request.IsFuelConnected.ToString(),
                IsMagnetConnected = request.IsMagnetConnected.ToString(),
                IsRelayEnabled = request.IsRelayEnabled.ToString(),
                fuelinfo = request.fuelinfo
            };
        }
        public static List<DeviceRegister> ToDeviceList(List<DeviceVehiclesDto> list)
        {
            var DeviceList = new List<DeviceRegister>();
            list.ForEach(tt
                => DeviceList.Add(ToAddOrUpdateDevice(tt)));
            return DeviceList;
        }
    }

    public class DeviceRegisterHandler : IRequestHandler<DeviceRegister, OperationResult<Unit>>
    {

        private readonly ICollectionProvider _prov;
        private readonly AlertsService _alertsService;
        private readonly AssignVehicleService _assignVehicleService;
        private readonly OperationResult<Unit> _result = new();
        public DeviceRegisterHandler(ICollectionProvider provider, AlertsService alertsService, AssignVehicleService assignVehicleService)
        {
            _prov = provider ?? throw new ArgumentNullException(nameof(_prov));
            _alertsService = alertsService ?? throw new ArgumentNullException(nameof(@alertsService));
            _assignVehicleService = assignVehicleService ?? throw new ArgumentNullException(nameof(@assignVehicleService));

        }
        public async Task<OperationResult<Unit>> Handle(DeviceRegister request,
                CancellationToken cancellationToken)
        {
            var result = new OperationResult<IEnumerable<DeviceRegister>>();
            var CollectionName = _prov.GetCollection<DeviceVehiclesDto>(CollectionNames.NewDeviceVehicles);
            await ValidateDeviceAsync(request, CollectionName);
            if (_result.IsError) return _result;
            // Create a session object that is used when leveraging transactions
            using (var session = await _prov.GetClient().StartSessionAsync())
            {
                // Begin transaction
                session.StartTransaction();
                try
                {
                    var Deviceerequest = DeviceRegister.TodeviceVehiclesDto(request);
                    Deviceerequest.CreatedDate = DateTime.Now;
                    await CollectionName.InsertOneAsync(Deviceerequest).ConfigureAwait(false);
                    var assignid = await _assignVehicleService.AssignVehile(Deviceerequest.Id);
                    await _alertsService.AddAlert(assignid, AlertNameType.WhatsApp.ToString(), AlertType.PowerVoid.ToString(), Deviceerequest.Id, 5, 5);
                    //Made it here without error? Let's commit the transaction
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
        private async Task ValidateDeviceAsync(DeviceRegister request, IMongoCollection<DeviceVehiclesDto> collection)
        {
            var deviceNo = (await collection.FindAsync(x => x.DeviceNo == request.DeviceNo)).FirstOrDefault();
            if (deviceNo is not null)
                _result.AddError(ErrorCode.ValidationError, DeviceMessages.DeviceNumberAlreadyExists);
            var SimNo = (await collection.FindAsync(x => x.SimNo == request.SimNo)).FirstOrDefault();
            if (SimNo is not null)
                _result.AddError(ErrorCode.ValidationError, DeviceMessages.SimNumberAlreadyExists);

        }



    }
}

