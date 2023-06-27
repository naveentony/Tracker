
using Microsoft.AspNetCore.Identity;
using Tracker.Features.Shared;

namespace Tracker.Features.Account.Identity
{
    public class RegisterUser : IRequest<OperationResult<IdentityResult>>
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public static UsersDto ToUsersDto(RegisterUser user)
        {
            return new UsersDto
            {
                UserName = user.Username,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
            };
        }

    }
    public class RegisterHandler : IRequestHandler<RegisterUser, OperationResult<IdentityResult>>
    {

        private readonly OperationResult<IdentityResult> _result = new();
        private readonly UserManager<UsersDto> _userManager;
        private readonly RoleManager<MongoRoleDto> _roleManager;
        private readonly IdentityService _identityService;
        private readonly ICollectionProvider _prov;
        public RegisterHandler(UserManager<UsersDto> userManager, RoleManager<MongoRoleDto> roleManager, IdentityService identityService, ICollectionProvider prov)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(_userManager));
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(_roleManager));
            _identityService = identityService ?? throw new ArgumentNullException(nameof(_identityService));
            _prov = prov;
        }

        public async Task<OperationResult<IdentityResult>> Handle(RegisterUser request,
                CancellationToken cancellationToken)
        {
            try
            {
                var result = new OperationResult<IEnumerable<RegisterUser>>();
                var collection = _prov.GetCollection<UsersDto>(CollectionNames.USERS);
                await ValidateUserAsync(request, collection);
                if (_result.IsError) return _result;
                var userdto = RegisterUser.ToUsersDto(request);
                userdto.CreatedDate = DateTime.Now;
                var role = await _roleManager.FindByNameAsync("User");
                if (role != null)
                    userdto.Roles.Add(role.Id);
                await _userManager.CreateAsync(userdto).ConfigureAwait(false);
                var identityUser = _userManager.FindByEmailAsync(request.Email);
                _result.Payload = LoginUser.Tologin(identityUser.Result);
                _result.Payload.Token = _identityService.GetJwtString(identityUser.Result);
                var list = new List<Token>();
                list.Add(_result.Payload.Token);
                await _identityService.SaveToken(identityUser.Result, collection, list);
                return _result;

            }
            catch (Exception e)
            {
                _result.AddError(ErrorCode.DatabaseOperationException, e.Message);
            }
            return _result;
        }
        private async Task ValidateUserAsync(RegisterUser request, IMongoCollection<UsersDto> collection)
        {
            var user = await _userManager.FindByNameAsync(request.Username);
            if (user is not null)
                _result.AddError(ErrorCode.ValidationError, IdentityErrorMessages.IdentityUserAlreadyExists);
            var email = await _userManager.FindByEmailAsync(request.Email);
            if (email is not null) 
                    _result.AddError(ErrorCode.ValidationError, IdentityErrorMessages.IdentityEmailAlreadyExists);
            var phoneNumber= await _prov.GetCollectionFristOrDefautFilter<UsersDto>(CollectionNames.USERS, "PhoneNumber", request.PhoneNumber);
            if(phoneNumber is not null)
                _result.AddError(ErrorCode.ValidationError, IdentityErrorMessages.PhoneNumber);
        }


    }
}
