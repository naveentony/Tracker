
using MongoDB.Bson;
using MongoDB.Driver.GeoJsonObjectModel;
using System.Data;
using Tracker.Domain.Dtos;

namespace Tracker.ImportData.Managers
{
    public  class DBManager
    {
       
        //public HttpContext _httpContext => new HttpContextAccessor().HttpContext;
        //public DBManager(CollectionProvider provider)
        //{
        //    _provider = provider;
        //    _deviceTypesService = deviceTypesService;
        //}
        public  List<TrackerTypesDto> LoadTrackerTypesData()
        {
            //from weelTrack
            var responce = new List<TrackerTypesDto>();
            DataSet result =  DBHelper.Executetext("select * from Trackers");
            foreach (DataRow row in result.Tables[0].Rows)
            {

                var obj = new TrackerTypesDto();
                obj.Name = row["Name"].ToString();
                obj.CompanyName = row["CompanyName"].ToString();
                obj.UpdateRates = row["UpdateRates"].ToString();
                obj.VRef = (double)row["VRef"];
                obj.Offset = (double)row["Offset"];
                obj.DefaultUpdateRate = (int)row["DefaultUpdateRate"];
                obj.CreatedDate = DateTime.UtcNow;
                responce.Add(obj);
            }
            return responce;
        }
        public List<VehicleTypesDto> LoadVehicleTypesData()
        {
            var tlist = new List<VehicleTypesDto>();
            DataSet result = DBHelper.Executetext("select * from VehicleTypes");
            foreach (DataRow row in result.Tables[0].Rows)
            {
                var obj = new VehicleTypesDto();
                    obj.Name = row["Vehicle"].ToString();
                obj.Amount = Convert.ToDouble( row["Amount"].ToString());
                obj.Status = row["status"].ToString() == "True" ? "Enable" : "Disable";
                obj.CreatedDate = DateTime.UtcNow;
                tlist.Add(obj);
            }
            return tlist;
        }
        public List<VehiclesDto> LoadNewDeviceVehicleData()
        {
            var tlist = new List<VehiclesDto>();
            DataSet result = DBHelper.Executetext(@"select * from NewDeviceVehicle
                                                   inner join VehicleLicense
                                                   on NewDeviceVehicle.Vid=VehicleLicense.VehicleId
                                                   inner join AssignAssets 
                                                   on AssignAssets.Vid=NewDeviceVehicle.Vid
                                                   inner join users on users.Uid=AssignAssets.Uid
                                                   where users.Uid=4668");
            foreach (DataRow row in result.Tables[0].Rows)
            {
                //var obj = new VehiclesDto();
                //obj.IMEI = row["DeviceNo"].ToString();
                //obj.RegistrationNumber = row["VehicleNo"].ToString();
                //obj.SimNo = row["SimNo"].ToString();
                //obj.ve = ObjectId.Parse("64a11dc9b44c5eaf46548bc5");
                //obj.SalesPerson = row["SalesPerson"].ToString();
                //obj.Customer = row["Customer"].ToString();
                //obj.VehicleModel = row["VehicleModel"].ToString();
                //obj.TimeZone = row["TimeZone"].ToString();
                //obj.SpeedLimit = Convert.ToInt32( row["SpeedLimit"].ToString() == "" ? 0 : row["SpeedLimit"]);
                //obj.InstallationDate = Convert.ToDateTime(row["InstallationDate"].ToString() == "" ? 0 : row["InstallationDate"]);
                //obj.ExpiryDate = Convert.ToDateTime(row["ExpiryDate"].ToString() == "" ? 0 : row["ExpiryDate"]);
                //obj.CurrentAmount = Convert.ToDouble(row["CurrentAmount"].ToString() == "" ? 0 : row["CurrentAmount"]);
                //obj.GrasePeriod = Convert.ToInt32(row["GrasePeriod"].ToString() == "" ? 0 : row["GrasePeriod"]);
                //obj.DataLimit = Convert.ToInt32(row["DataLimit"].ToString() == "" ? 0 : row["DataLimit"]);
                //obj.DeviceTypeId = ObjectId.Parse("64a11797892512a580afca66");
                //obj.IsRelayEnabled = row["IsRelayEnabled"].ToString() == "1" ? "Enable" : "Disable";
                //obj.IsACConnected = row["IsACConnected"].ToString() == "1" ? "Enable" : "Disable";
                //obj.IsFuelConnected = row["IsFuelConnected"].ToString() == "1" ? "Enable" : "Disable";
                //obj.IsMagnetConnected = row["IsMagnetConnected"].ToString() == "1" ? "Enable" : "Disable";
                //obj.AmountStatus = "Paid";
                //obj.IsRentEnabled = "Enable";
                //obj.PamentType = "PostPaid";
                //obj.RenewalAmount = Convert.ToDouble(row["RenewalAmount"].ToString()==""?0: row["RenewalAmount"]);
                //obj.RenewalDays = Convert.ToInt32(row["RenewalDays"].ToString() == "" ? 0 : row["RenewalDays"]);
                //obj.Mileage = Convert.ToSingle(row["Mileage"].ToString() == "" ? 0 : row["Mileage"]);
                //obj.CreatedDateTime = DateTime.Now;
                //Fuelinfo fuelinfo = new Fuelinfo();
                //fuelinfo.IsFuelReadingInverse = row["IsFuelReadingInverse"].ToString() == "1" ? "Enable" : "Disable"; 
                //fuelinfo.FuelTankCapacityLitres = Convert.ToDouble(row["FuelTankCapacityLitres"].ToString() == "" ? 0 : row["FuelTankCapacityLitres"]); ;
                //fuelinfo.VMax = Convert.ToDouble(row["VMax"].ToString() == "" ? 0 : row["VMax"]); ;
                //fuelinfo.VMin = Convert.ToDouble(row["VMin"].ToString() == "" ? 0 : row["VMin"]); ;
                //obj.fuelinfo = fuelinfo;
                //tlist.Add(obj);
            }
            return tlist;
        }
        public List<TrackerDataDto> TrackerData()
        {
            var tlist = new List<TrackerDataDto>();
            DataSet result = DBHelper.Executetext(@"select  * from TrackData");
            foreach (DataRow row in result.Tables[0].Rows)
            {
                var obj = new TrackerDataDto();
                obj.IMEI = row["DeviceNo"].ToString();
                obj.SoftwareVersion = row["SoftwareVersion"].ToString();
                obj.ProfileName = "";
                obj.GPSStatus = true;
                obj.SignalStrength = 0;
                obj.location = GeoJson.Point(new GeoJson2DGeographicCoordinates(Convert.ToDouble(row["Longitude"]), Convert.ToDouble(row["Latitude"])));
                obj.LocationName = "Need tofill";
                //var cordinates = new double[] { ,) };
                //obj.location = new Location
                //{
                //    type = "Point",
                //    coordinates = cordinates
                //    LocationName = "Need tofill"
                //};
                obj.Altitude = (int)row["Altitude"];
                obj.Speed = (int)row["Speed"];
                obj.Direction = (int)row["Direction"];
                obj.Satellite = Convert.ToInt32(row["Satellite"]);
                obj.GPSPositionAccuracyIndication = Convert.ToDouble(0);
                obj.MilageReading = Convert.ToDouble(0);
                obj.Cell = "";
                obj.Analog1 = Convert.ToDouble(row["Analog1"]);
                obj.Analog2 = Convert.ToDouble(row["Analog2"]);
                obj.Analog3 = Convert.ToDouble(0);
                obj.Analog4 = Convert.ToDouble(0);
                obj.DigitalInputLevel1 = false;
                obj.DigitalInputLevel2 = false;
                obj.DigitalInputLevel3 = false;
                obj.DigitalInputLevel4 = false;
                obj.DigitalOutputLevel1 = false;
                obj.DigitalOutputLevel2 = false;
                obj.DigitalOutputLevel3 = false; 
                obj.DigitalOutputLevel4 = false;
                //obj.Vehicles_VehicleId = null;
                obj.InfoNumber = 0;

                obj.HarshDetecation = false;
                obj.RFID = "";
                obj.IsIgnitionOn = Convert.ToInt32(row["Ignition"]) == 0 ? false : true;
                //obj.fuelData = null;
                obj.CreatedDateTime = DateTime.Now;

                tlist.Add(obj);
            }
            return tlist;
        }
        public List<TrackerDataLiveDto> TrackerDataLive()
        {
            var tlist = new List<TrackerDataLiveDto>();
            DataSet result = DBHelper.Executetext(@"select  * from TrackDataLive");
            foreach (DataRow row in result.Tables[0].Rows)
            {
                var obj = new TrackerDataLiveDto();
                obj.IMEI = row["DeviceNo"].ToString();
                if (obj.IMEI == "358657100287718")
                {
                    obj.additionalParameters.ParameterValue = 12345;
                    obj.additionalParameters.ParameterName = "test";
                }
                obj.SoftwareVersion = row["SoftwareVersion"].ToString();
                obj.ProfileName = "";
                obj.GPSStatus = true;
                obj.SignalStrength = 0;
                obj.TrackDateTime = Convert.ToDateTime(row["TrackDateTime"].ToString());
                obj.location = GeoJson.Point(new GeoJson2DGeographicCoordinates(Convert.ToDouble(row["Longitude"]), Convert.ToDouble(row["Latitude"])));
                obj.LocationName = "Need tofill";
                obj.Altitude = (int)row["Altitude"];
                obj.Speed = (int)row["Speed"];
                obj.Direction = (int)row["Direction"];
                obj.Satellite = Convert.ToInt32(row["Satellite"]);
                obj.GPSPositionAccuracyIndication = Convert.ToDouble(0);
                obj.MilageReading = Convert.ToDouble(0);
                obj.Cell = "";
                obj.Analog1 = Convert.ToDouble(row["Analog1"]);
                obj.Analog2 = Convert.ToDouble(row["Analog2"]);
                obj.Analog3 = Convert.ToDouble(0);
                obj.Analog4 = Convert.ToDouble(0);
                obj.DigitalInputLevel1 = false;
                obj.DigitalInputLevel2 = false;
                obj.DigitalInputLevel3 = false;
                obj.DigitalInputLevel4 = false;
                obj.DigitalOutputLevel1 = false;
                obj.DigitalOutputLevel2 = false;
                obj.DigitalOutputLevel3 = false;
                obj.DigitalOutputLevel4 = false;
                //obj.Vehicles_VehicleId = null;
                obj.InfoNumber = 0;

                obj.HarshDetecation = false;
                obj.RFID = "";
                obj.IsIgnitionOn = Convert.ToInt32(row["Ignition"]) == 0 ? false : true;
                obj.BatteryVoltage = Convert.ToDouble(0);
                obj.BatteryStatus = false;
                obj.PortNumber = 5003;
                obj.IsRentEnable = false;
                //obj.fuelData = null;
                obj.CreatedDateTime = DateTime.Now;
                tlist.Add(obj);
            }
            return tlist;
        }

        public List<VehiclesDto> VehiclesDtoData()
        {
            var tlist = new List<VehiclesDto>();
            DataSet result = DBHelper.Executetext(@"select  * from NewDeviceVehicle");
            foreach (DataRow row in result.Tables[0].Rows)
            {
                var obj = new VehiclesDto();
                
                obj.IMEI = row["DeviceNo"].ToString();
                var deviceList=new List<string>();
                deviceList.Add("358657100287718");
                deviceList.Add("358657100325997");
                deviceList.Add("355172101858189");
                deviceList.Add("355172102018601");
                deviceList.Add("862111128003717");
                deviceList.Add("862201128002153");
                deviceList.Add("358657101645674");
                deviceList.Add("862205128002688");
                deviceList.Add("358657102589731");
                deviceList.Add("869689043032937");
                deviceList.Add("358657102578668");
                if (deviceList.Contains(obj.IMEI))
                {
                    obj.Users_Id = Guid.Parse("73c5c313-ede4-42e3-ac20-8b81738a949b");
                }
                obj.Name = row["VehicleNo"].ToString();
                obj.RegistrationNumber = row["VehicleNo"].ToString();
                obj.SimNo = row["SimNo"].ToString();
                obj.VehicleType = "Truck";
                obj.Manufacturer = row["VehicleModel"].ToString();
                obj.VehicleModel = row["VehicleModel"].ToString();
                obj.Year = 2020;
                obj.ServiceProvider = row["Customer"].ToString();
                //obj.LastServicedOn = (int)row["Direction"];
               // obj.NextServiceAt = Convert.ToInt32(row["Satellite"]);
                //obj.PUCExpiryDate = Convert.ToDouble(0);
                //obj.TargetUtilizationPerDay = Convert.ToDouble(0);
                obj.SpeedLimit = Convert.ToInt32( row["SpeedLimit"]);
                //obj.InsuranceExpiryDate = 
                //obj.PermitExpiryDate = Convert.ToDouble(row["Analog2"]);
                //obj.NextServiceDate = Convert.ToDouble(0);
                obj.InstallationDate = Convert.ToDateTime(row["InstallationDate"]);
                obj.ExpiryDate = Convert.ToDateTime(row["ExpiryDate"]);
                //obj.TemperatureHigh = false;
                //obj.TemperatureLow = false;
                obj.CurrentAmount = Convert.ToDouble(row["RenewalAmount"]);
                obj.GrasePeriod = 0;// Convert.ToInt32(row["GrasePeriod"]);
                obj.DataLimit = Convert.ToInt32(row["DataLimit"]);
                obj.TrackerTypes_Id = ObjectId.Parse("64b5664e901eceb7749c145f");
                obj.IsRelayEnabled = false;
                obj.IsACConnected = false;

                obj.IsFuelConnected = false;
                obj.IsMagnetConnected = false;
                obj.IsRentEnabled = false;
                obj.AmountStatus = "Paid";
                obj.PamentType = "Credit";
                obj.RenewalAmount = Convert.ToDouble(row["RenewalAmount"]);
                obj.RenewalDays = Convert.ToInt32(row["RenewalDays"]);

                obj.IsDeleted = false;
                // obj.fuelinfo = "";
                //obj.Mileage = Convert.ToInt32(row["Ignition"]) == 0 ? false : true;
               // obj.UpdatedDate = false;
                //obj.fuelData = null;
                obj.CreatedDateTime = DateTime.Now;
                tlist.Add(obj);
            }
            return tlist;
        }
    }
}
