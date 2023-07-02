using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Tracker.Features.Shared
{

    public static class IdentityMessages
    {
        public const string NonExistentIdentityUser = "Unable to find a user with the specified username";
        public const string IncorrectPassword = "The provided password is incorrect";
        public const string IdentityUserAlreadyExists = "Provided user Name already exists.";
        public const string IdentityEmailAlreadyExists = "Provided email address already exists.";
        public const string PhoneNumber = "Provided Phone Number already exists. ";
        public const string UnauthorizedAccountRemoval = "Cannot remove account as you are not its owner";
        public const string NonExistentIdentityRole = "Unable to find a user with the specified rolename";
        public const string InvalidRole = "Unable to find the specified rolename";
        public const string RoleAlreadyExists = "Provided Role Name already exists.";


    }
    public static class ClinetMessages
    {
        public const string ClientNameAlreadyExists = "Provided Clinet Name already exists.";
    }
    public static class DeviceMessages
    {
        public const string DeviceNumberAlreadyExists = "Provided Device Number  already exists.";
        public const string SimNumberAlreadyExists = "Provided Sim Number  already exists.";


        public const string DeviceTypeAlreadyExists = "Provided Device Type  already exists.";
        public const string VehicleTypeAlreadyExists = "Provided Vehicle Type  already exists.";

    }
}