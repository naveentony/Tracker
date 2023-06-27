using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tracker.Domain.Enums;

namespace Tracker.Domain.Dtos
{
    [CollectionName("DeviceVehicles")]
    public class DeviceVehiclesDto
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } 
        public string DeviceNo { get; set; } 
        public string VehicleNo { get; set; } 
        public string DeviceType { get; set; }
        public string SimNo { get; set; }
        public string SalesPerson { get; set; } 
        public string VehicleModel { get; set; } 
        public string TimeZone { get; set; }
        public int SpeedLimit { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public double CurrentAmount { get; set; }
        public int GrasePeriod { get; set; }
        

        public int DataLimit { get; set; }
        public string AmountStatus { get; set; }
        public Fuelinfo fuelinfo { get; set; } = new Fuelinfo();
        public float Mileage { get; set; }
        public double RenewalAmount { get; set; }
        public int RenewalDays { get; set; }
        public StatusType IsACConnected { get; set; }
        public StatusType IsFuelConnected { get; set; }
        public StatusType IsMagnetConnected { get; set; }
        public StatusType IsRelayEnabled { get; set; }
        public Guid VehicleTypeId { get; set; } // Referencing to the VehicleTypes
        public Guid DeviceTypeId { get; set; } // Referencing to the DeivceTypes
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

    }
    public class Fuelinfo
    {
        public int? IsFuelReadingInverse { get; set; }
        public decimal FuelTankCapacityLitres { get; set; }
        public decimal VMax { get; set; }
        public decimal VMin { get; set; }
    }
    public class VehicleDeviceTransation
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string VehicleId { get; set; }  // Referencing to the DeviceVehicles
        public DateTime StartDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public double AmountPaid { get; set; }
        public long PaymentId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
    public class AlertSettings
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string AssignId { get; set; }  // Referencing to the DeviceVehicles
        public string VehicleId { get; set; } // Referencing to the AssignVehicles
        public List<string> AlertName { get; set; }
        public List<string> AlertType { get; set; }
        public int SMSLimit { get; set; }
        public int EmailLimit { get; set; }
        public DateTime CreatedDate { get; set; }
    }
    public enum AlertNameType
    {
        SMS,Email,WhatsApp
    }
    public enum AlertType
    {
        PowerVoid, SpeedVoid, Ignition, SOS, Ignitionoff, StoppedBy30Min
    }
}

