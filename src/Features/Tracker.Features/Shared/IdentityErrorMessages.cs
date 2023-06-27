namespace Tracker.Features.Shared
{
    public class IdentityErrorMessages
    {
        public const string NonExistentIdentityUser = "Unable to find a user with the specified username";
        public const string IncorrectPassword = "The provided password is incorrect";
        public const string IdentityUserAlreadyExists = "Provided email address already exists.";
        public const string IdentityEmailAlreadyExists = "Provided email address already exists.";
        public const string PhoneNumber = "Provided Phone Number already exists. ";
        public const string UnauthorizedAccountRemoval = "Cannot remove account as you are not its owner";

        public const string NonExistentIdentityRole = "Unable to find a user with the specified rolename";

        

        public const string RoleAlreadyExists = "Provided Role Name already exists.";
    }
}