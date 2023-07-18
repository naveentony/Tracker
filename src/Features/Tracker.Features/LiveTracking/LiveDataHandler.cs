using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.GeoJsonObjectModel;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Tracker.Features.Device.DeviceTypes;
using Tracker.ImportData.Managers;

namespace Tracker.Features.LiveTracking
{
    public class LiveDataResult
    {

        public string IMEI { get; set; }
        public string RegistraionNumber { get; set; }
        public string TrackerId { get; set; }
        public string TrackerName { get; set; }
        public string VehicleType { get; set; }

        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string DeviceName { get; set; }
        public int Speed { get; set; }
        public DateTime TrackDateTime { get; set; }
        public bool Ignition { get; set; }
        public GeoJsonPoint<GeoJson2DGeographicCoordinates> Location { get; set; }
        public bool ACStatus { get; set; }
        public bool BatteryStatus { get; set; }
        public string VehicleStatusSince { get; set; }
        public int PortNumber { get; set; }
        public double FuelReading { get; set; }
        public int Altitude { get; set; }
        public double AnalogInputVoltage3 { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string InfoNumber { get; set; }
        public AdditionalParameters AdditionalParameters { get; set; }
        public bool IsTripActive { get; set; }
        public int ActiveTripId { get; set; }
        public bool IsExpired { get; set; }
        public VehicleStatus Status { get; set; }

    }
    public class LiveData : IRequest<OperationResult<IEnumerable<LiveDataResult>>>
    {
        public string VehicleId { get; set; }
    }
    public class LiveDataHandler
        : IRequestHandler<LiveData, OperationResult<IEnumerable<LiveDataResult>>>
    {

        private readonly ICollectionProvider _prov;
        private readonly UserService _userService;
        public LiveDataHandler(ICollectionProvider provider, UserService userService)
        {
            _prov = provider ?? throw new ArgumentNullException(nameof(_prov));
            _userService = userService;
        }
        public async Task<OperationResult<IEnumerable<LiveDataResult>>> Handle(LiveData request,
                CancellationToken cancellationToken)
        {

            var result = new OperationResult<IEnumerable<LiveDataResult>>();
            var TrackerDataLive = _prov.GetCollection<TrackerDataLiveDto>(CollectionNames.TrackerDataLive);
            var Vehicles = _prov.GetCollection<VehiclesDto>(CollectionNames.Vehicles);
            var TrackerTypes = _prov.GetCollection<TrackerTypesDto>(CollectionNames.TrackerTypes);
            var Users = _prov.GetCollection<UsersDto>(CollectionNames.USERS);
            var CurrentUser = Guid.Parse("73c5c313-ede4-42e3-ac20-8b81738a949b");
            var userlist = Users.AsQueryable().ToList().Where(x => x.Id == CurrentUser).First().AssigedUsers;
            userlist.Add(CurrentUser);
            var res = (from t in  TrackerDataLive.AsQueryable()
                       join v in Vehicles.AsQueryable()
                        on t.IMEI equals v.IMEI
                       join d in TrackerTypes.AsQueryable()
                       on v.TrackerTypes_Id equals d.Id
                       join u in Users.AsQueryable()
                       on v.Users_Id equals u.Id
                       select new LiveDataResult
                       {
                           IMEI = t.IMEI,
                           RegistraionNumber = v.RegistrationNumber,
                           TrackerId = v.TrackerTypes_Id.ToString(),
                           TrackerName = d.Name,
                           VehicleType = v.VehicleType,
                           Location = t.location,
                           UserId = v.Users_Id,
                           UserName = u.UserName,
                           Speed = t.Speed,
                           Ignition = t.IsIgnitionOn,
                           ACStatus = t.DigitalInputLevel2,
                           BatteryStatus = t.BatteryStatus,
                           // Status = LiveService.GetCurrentTrackerStatus(d, t),
                           DeviceName = d.Name,
                           TrackDateTime = t.TrackDateTime,
                           PortNumber = t.PortNumber,
                           FuelReading = t.fuelData.FuelReading,
                           Altitude = t.Altitude,
                           AnalogInputVoltage3 = t.Analog3,
                           IsExpired = v.ExpiryDate > DateTime.Now,
                           AdditionalParameters = t.additionalParameters,
                           IsTripActive = t.vehicleData.IsTripActive,
                           ActiveTripId = t.vehicleData.ActiveTripId

                       }).ToList().Where(x => userlist.Contains(x.UserId));
            result.TotalCount = res.Count();
            result.CurrentPage = 1;
            result.PageSize = 100;
            result.TotalPages = ((int)(result.TotalCount - 1) / result.PageSize) + 1;
            result.Payload = res.Skip((result.CurrentPage - 1) * result.PageSize).Take(result.PageSize);
            return result;
        }


    }



}
