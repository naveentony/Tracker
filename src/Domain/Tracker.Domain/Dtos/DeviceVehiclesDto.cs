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
        public string DeviceTypeId { get; set; }// Referencing to the DeivceTypes
        [BsonRequired]
        public string SimNo { get; set; }
        public string SalesPerson { get; set; } 
        public string VehicleModel { get; set; } 
        public string TimeZone { get; set; }
        public int SpeedLimit { get; set; }
        [BsonRequired]
        public DateTime StartDate { get; set; }
        [BsonRequired]
        public DateTime ExpiryDate { get; set; }
        public double CurrentAmount { get; set; }
        public int GrasePeriod { get; set; }

        [BsonRequired]
        public int DataLimit { get; set; }
        public string AmountStatus { get; set; }
        public Fuelinfo fuelinfo { get; set; } = new Fuelinfo();
        public float Mileage { get; set; }
        public double RenewalAmount { get; set; }
        public int RenewalDays { get; set; }
        [BsonRequired]
        public string IsACConnected { get; set; }
        [BsonRequired]
        public string IsFuelConnected { get; set; }
        [BsonRequired]
        public string IsMagnetConnected { get; set; }
        [BsonRequired]
        public string IsRelayEnabled { get; set; }
        [BsonRequired]
        public string IsRentEnabled { get; set; }
        [BsonRequired]
        public string VehicleTypeId { get; set; } // Referencing to the VehicleTypes
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
    /*
    public class DeviceVehiclesAggregateValidator
    {
        /// <summary>
        /// Validates a friend request instance
        /// </summary>
        /// <param name="friendRequest">The friend request instance to be validated</param>
        /// <exception cref="FriendRequestValidationException">Thrown when the instance is not valid</exception>
        public static void ValidateFriendRequest(DeviceVehiclesDto request)
        {
            var validator = new DeviceVehiclesValidator();
            var validationResult = validator.Validate(request);

            if (!validationResult.IsValid)
                ThrowNotValidException<DeviceVehiclesValidationException>(validationResult.Errors);
        }

        private static void ThrowNotValidException<T>(List<ValidationFailure> errors)
            where T : DomainModelInvalidException
        {
            var exception = new DeviceVehiclesValidationException("New Device request is not  valid");
            errors
                .ForEach(e => exception.ValidationErrors.Add(e.ErrorMessage));
            throw exception;
        }
    }
    public class DeviceVehiclesValidator : AbstractValidator<DeviceVehiclesDto>
    {

        public DeviceVehiclesValidator()
        {
            RuleFor(x => x.DeviceNo)
                .Custom((DeviceNo, context) =>
                {
                    if (string.IsNullOrEmpty(DeviceNo))
                    {
                        
                    }
                        context.AddFailure(new ValidationFailure("DeviceNo",
                            "Friend request id is not a valid GUID format"));
                });
            RuleFor(x => x.DateSent).LessThanOrEqualTo(DateTime.Now);
        }
    }
    public class DeviceVehiclesValidationException : DomainModelInvalidException
    {
        internal DeviceVehiclesValidationException() { }
        internal DeviceVehiclesValidationException(string message) : base(message) { }
        internal DeviceVehiclesValidationException(string message, Exception inner) : base(message, inner) { }
    }

    */
}

