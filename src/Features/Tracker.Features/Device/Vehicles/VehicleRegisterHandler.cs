

using Tracker.Features.Account.Identity;

namespace Tracker.Features.Device.Vehicles
{

    public class VehicleRegister : IRequest<OperationResult<Unit>>
    {
        public string IMEI { get; set; }
        public string Name { get; set; }
        public string RegistrationNumber { get; set; }
        public string SimNo { get; set; }
        public string VehicleType { get; set; } // Referencing to the VehicleTypes

        public string Manufacturer { get; set; }
        public string VehicleModel { get; set; }
        public int Year { get; set; }
        public string ServiceProvider { get; set; }
        public DateTime LastServicedOn { get; set; }
        public int NextServiceAt { get; set; }
        public DateTime PUCExpiryDate { get; set; }
        public int TargetUtilizationPerDay { get; set; }
        public int SpeedLimit { get; set; }
        public DateTime InsuranceExpiryDate { get; set; }
        public DateTime PermitExpiryDate { get; set; }
        public DateTime NextServiceDate { get; set; }
        public DateTime InstallationDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public double TemperatureHigh { get; set; }

        public double TemperatureLow { get; set; }

        public double CurrentAmount { get; set; }
        public int GrasePeriod { get; set; }


        public int DataLimit { get; set; }
        public ObjectId DeviceTypeId { get; set; }// Referencing to the DeivceTypes


        public bool IsRelayEnabled { get; set; }
        public bool IsACConnected { get; set; }
        public bool IsFuelConnected { get; set; }
        public bool IsMagnetConnected { get; set; }
        public bool IsRentEnabled { get; set; }
        public string AmountStatus { get; set; }
        public string PamentType { get; set; }
        public double RenewalAmount { get; set; }
        public int RenewalDays { get; set; }
        public bool IsDeleted { get; set; }
        public Guid UserId { get; set; }

        public Fuelinfo? fuelinfo { get; set; }
        public float Mileage { get; set; }


        public DateTime CreatedDateTime { get; set; }
        public DateTime UpdatedDate { get; set; }

        public static VehiclesDto TodeviceVehiclesDto(VehicleRegister request)
        {
            var result = new VehiclesDto();
            if (request.fuelinfo != null)
            {

                request.fuelinfo.VMax = Convert.ToDouble(request.fuelinfo.VMax);
                request.fuelinfo.VMin = Convert.ToDouble(request.fuelinfo.VMin);
                request.fuelinfo.FuelTankCapacityLitres = Convert.ToDouble(request.fuelinfo.FuelTankCapacityLitres);
                result.fuelinfo = request.fuelinfo;
            }
            result.IMEI = request.IMEI;
            result.RegistrationNumber  = request.RegistrationNumber;
            result.SimNo = request.SimNo;
            result.VehicleType = request.VehicleType;
            result.ServiceProvider = request.ServiceProvider;
            result.Manufacturer = request.Manufacturer;
            result.VehicleModel = request.VehicleModel;
            //result.TimeZone = request.TimeZone;
            result.SpeedLimit = request.SpeedLimit;
            result.InstallationDate = request.InstallationDate;
            result.ExpiryDate = request.ExpiryDate;
            result.CurrentAmount = Convert.ToDouble(request.CurrentAmount);
            result.GrasePeriod = request.GrasePeriod;
            result.DataLimit = request.DataLimit;
            //result.DeviceTypeId = ObjectId.Parse(request.DeviceTypeId);
            //result.IsRelayEnabled = TrackerUtility.GetStatus(request.IsRelayEnabled);
            //result.IsACConnected = TrackerUtility.GetStatus(request.IsACConnected);
            //result.IsFuelConnected = TrackerUtility.GetStatus(request.IsFuelConnected);
            //result.IsMagnetConnected = TrackerUtility.GetStatus(request.IsMagnetConnected);
            //result.IsRentEnabled = TrackerUtility.GetStatus(request.IsRentEnabled);
            //result.AmountStatus = TrackerUtility.GetAmountStatus(request.AmountStatus);
            result.RenewalAmount = Convert.ToDouble(request.RenewalAmount);
            result.RenewalDays = request.RenewalDays;
            
            result.Mileage = request.Mileage;
            return result;
        }
        public static VehicleRegister ToAddOrUpdateDevice(VehiclesDto request)
        {
            return new VehicleRegister
            {
                IMEI = request.IMEI,
                RegistrationNumber = request.RegistrationNumber,
                SimNo = request.SimNo,
                VehicleType =request.VehicleType.ToString(),
                ServiceProvider = request.ServiceProvider,
                Manufacturer = request.Manufacturer,
                VehicleModel = request.VehicleModel,
                //TimeZone = request.TimeZone,
                SpeedLimit = request.SpeedLimit,
                InstallationDate = request.InstallationDate,
                ExpiryDate = request.ExpiryDate,
                CurrentAmount = request.CurrentAmount,
                GrasePeriod = request.GrasePeriod,
                DataLimit = request.DataLimit,
                //DeviceTypeId =request.DeviceTypeId.ToString(),
                //IsRelayEnabled = TrackerUtility.GetStatus(request.IsRelayEnabled),
                //IsACConnected = TrackerUtility.GetStatus(request.IsACConnected),
                //IsFuelConnected = TrackerUtility.GetStatus(request.IsFuelConnected),
                //IsMagnetConnected = TrackerUtility.GetStatus(request.IsMagnetConnected),
                //IsRentEnabled = TrackerUtility.GetStatus(request.IsRentEnabled),
                //AmountStatus = TrackerUtility.GetAmountStatus(request.AmountStatus),
                RenewalAmount = request.RenewalAmount,
                RenewalDays = request.RenewalDays,
                fuelinfo = request.fuelinfo,
                Mileage = request.Mileage,
            };
        }
        public static List<VehicleRegister> ToDeviceList(List<VehiclesDto> list)
        {
            var DeviceList = new List<VehicleRegister>();
            list.ForEach(tt
                => DeviceList.Add(ToAddOrUpdateDevice(tt)));
            return DeviceList;
        }
    }

    public class VehicleRegisterHandler : IRequestHandler<VehicleRegister, OperationResult<Unit>>
    {

        private readonly ICollectionProvider _prov;
        private readonly AlertsService _alertsService;
        private readonly AssignVehicleService _assignVehicleService;
        private readonly OperationResult<Unit> _result = new();
        private readonly IdentityService _identityService;
        public HttpContext _httpContext => new HttpContextAccessor().HttpContext;
        public VehicleRegisterHandler(ICollectionProvider provider, AlertsService alertsService
            , AssignVehicleService assignVehicleService,IdentityService identityService)
        {
            _prov = provider ?? throw new ArgumentNullException(nameof(_prov));
            _alertsService = alertsService ?? throw new ArgumentNullException(nameof(@alertsService));
            _assignVehicleService = assignVehicleService ?? throw new ArgumentNullException(nameof(@assignVehicleService));
            _identityService = identityService;

        }
        public async Task<OperationResult<Unit>> Handle(VehicleRegister request,
                CancellationToken cancellationToken)
        {
            var result = new OperationResult<IEnumerable<VehicleRegister>>();
            var CollectionName = _prov.GetCollection<VehiclesDto>(CollectionNames.Vehicles);
            await ValidateDeviceAsync(request, CollectionName);
            if (_result.IsError) return _result;
            // Create a session object that is used when leveraging transactions
            using (var session = await _prov.GetClient().StartSessionAsync())
            {
                // Begin transaction
                session.StartTransaction();
                try
                {
                    var Deviceerequest = VehicleRegister.TodeviceVehiclesDto(request);
                    Deviceerequest.CreatedDateTime = DateTime.Now;
                    await CollectionName.InsertOneAsync(Deviceerequest).ConfigureAwait(false);
                    //var assignid = await _assignVehicleService.AssignVehile(Deviceerequest.Id);
                    //await _alertsService.AddAlert(assignid, AlertNameType.WhatsApp.ToString(), AlertType.PowerVoid.ToString(), Deviceerequest.Id, 5, 5);
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
        private async Task ValidateDeviceAsync(VehicleRegister request, IMongoCollection<VehiclesDto> collection)
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

