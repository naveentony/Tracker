using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tracker.Domain.Enums;
using Tracker.Domain.Exceptions;

namespace Tracker.Domain.Dtos
{
    /*
    [CollectionName("DeviceVehicles")]
    public class DeviceVehiclesDto
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonRequired]
        public string DeviceNo { get; set; }
        [BsonRequired]
        public string VehicleNo { get; set; }

        [BsonRequired]
        public string SimNo { get; set; }
        [BsonRequired]
        public ObjectId VehicleTypeId { get; set; } // Referencing to the VehicleTypes

        public string SalesPerson { get; set; }
        public string Customer { get; set; }
        public string VehicleModel { get; set; }
        public string TimeZone { get; set; }
        public int SpeedLimit { get; set; }
        [BsonRequired]
        public DateTime InstallationDate { get; set; }
        [BsonRequired]
        public DateTime ExpiryDate { get; set; }
        public double CurrentAmount { get; set; }
        public int GrasePeriod { get; set; }


        [BsonRequired]
        public int DataLimit { get; set; }
        [BsonRequired]
        public ObjectId DeviceTypeId { get; set; }// Referencing to the DeivceTypes


        [BsonRequired]
        public string IsRelayEnabled { get; set; }
        public string IsACConnected { get; set; }
        public string IsFuelConnected { get; set; }
        public string IsMagnetConnected { get; set; }
        public string IsRentEnabled { get; set; }
        public string AmountStatus { get; set; }
        public string PamentType { get; set; }
        public double RenewalAmount { get; set; }
        public int RenewalDays { get; set; }

        public Fuelinfo? fuelinfo { get; set; } = new Fuelinfo();
        public float Mileage { get; set; }


        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

    }
    */
    public class Fuelinfo
    {
        public string IsFuelReadingInverse { get; set; }
        public double FuelTankCapacityLitres { get; set; }
        public double VMax { get; set; }
        public double VMin { get; set; }
    }
    [CollectionName("Vehicles")]
    public class VehiclesDto
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
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
        public Guid Users_Id { get; set; }
        public ObjectId TrackerTypes_Id { get; set; }// Referencing to the DeivceTypes
        public Fuelinfo? fuelinfo { get; set; }
        public float Mileage { get; set; }


        public DateTime CreatedDateTime { get; set; }
        public DateTime UpdatedDate { get; set; }

    }

}

