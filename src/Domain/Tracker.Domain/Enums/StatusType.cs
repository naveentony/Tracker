//using static Tracker.Api.Features.VehicleTypes.GetAllVehicleTypes;

namespace Tracker.Domain.Enums
{
    public enum StatusType
    {
        Disable,
        Enable
    }
    public enum YesNo
    {
        Yes,
        No
    }
    /*
    public class VehicleTypeAggregateValidator
    {
        /// <summary>
        /// Validates a friend request instance
        /// </summary>
        /// <param name="friendRequest">The friend request instance to be validated</param>
        /// <exception cref="FriendRequestValidationException">Thrown when the instance is not valid</exception>
        /// 
        ///

        /// <summary>
        /// Validates vehicleType instance
        /// </summary>
        /// <param name="vehicleTypeDto"></param>
        public static void ValidateFriendRequest(VehicleTypeDto vehicleTypeDto)
        {
            var validator = new VehicleTypeValidator();
            var validationResult = validator.Validate(vehicleTypeDto);

            if (!validationResult.IsValid)
                ThrowNotValidException<VehicleTypeValidationException>(validationResult.Errors);
        }

        private static void ThrowNotValidException<T>(List<ValidationFailure> errors)
            where T : DomainModelInvalidException
        {
            var exception = new VehicleTypeValidationException("Vehiche Type is not  valid");
            errors
                .ForEach(e => exception.ValidationErrors.Add(e.ErrorMessage));
            throw exception;
        }
    }
    public class VehicleTypeValidator : AbstractValidator<VehicleTypeDto>
    {
        public VehicleTypeValidator()
        {
            RuleFor(x => x.Vehicle)
                .Custom((vehicle, context) =>
                {
                    if (vehicle == string.Empty)
                        context.AddFailure(new ValidationFailure("vehicle",
                            "Friend request id is not a valid GUID format"));
                });
        }
    }
    public class VehicleTypeValidationException : DomainModelInvalidException
    {
        internal VehicleTypeValidationException() { }
        internal VehicleTypeValidationException(string message) : base(message) { }
        internal VehicleTypeValidationException(string message, Exception inner) : base(message, inner) { }
    }
    */
}
