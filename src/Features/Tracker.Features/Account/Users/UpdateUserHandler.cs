using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tracker.Features.Shared;

namespace Tracker.Features.Account.Users
{
    public class UpdateUser : IRequest<OperationResult<Unit>>
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string Token { get; set; }
        public static UsersDto ToUsersDto(UpdateUser user)
        {
            return new UsersDto
            {
                UserName = user.Username,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
            };
        }

    }
    public class UpdateUserHandler : IRequestHandler<UpdateUser, OperationResult<Unit>>
    {

        private readonly OperationResult<Unit> _result = new();
        private readonly UserManager<UsersDto> _userManager;
        private readonly RoleManager<MongoRoleDto> _roleManager;
        public UpdateUserHandler(UserManager<UsersDto> userManager, RoleManager<MongoRoleDto> roleManager)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(_userManager));
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(_roleManager));
        }

        public async Task<OperationResult<Unit>> Handle(UpdateUser request,
                CancellationToken cancellationToken)
        {
            try
            {
                var result = new OperationResult<IEnumerable<UpdateUser>>();
                await ValidateUserAsync(request);
                if (_result.IsError) return _result;
                var user = UpdateUser.ToUsersDto(request);
                user.UpdatedDate = DateTime.Now;
                var role = await _roleManager.FindByNameAsync("User");
                if (role != null)
                    user.Roles.Add(role.Id);
                await _userManager.UpdateAsync(user).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                _result.AddError(ErrorCode.DatabaseOperationException, e.Message);
            }
            return _result;
        }
        private async Task ValidateUserAsync(UpdateUser request)
        {
            var user = await _userManager.FindByNameAsync(request.Username);
            if (user is not null)
            {
                if (!(user.PhoneNumber.Equals(request.PhoneNumber)))
                {
                 var phoneNumberToken =  await _userManager.GenerateChangePhoneNumberTokenAsync(user,user.PhoneNumber);
                    var changePhoneResult = await _userManager.ChangePhoneNumberAsync(user, request.PhoneNumber, phoneNumberToken);
                    if (!changePhoneResult.Succeeded)
                        _result.AddError(ErrorCode.ValidationError, IdentityMessages.PhoneNumber);
                }
            }
            else
                _result.AddError(ErrorCode.IdentityUserDoesNotExist, IdentityMessages.NonExistentIdentityRole);
        }
        


    }
}
