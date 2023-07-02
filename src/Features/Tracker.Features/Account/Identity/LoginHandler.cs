using Tracker.Features.Shared;
using static MongoDB.Driver.WriteConcern;

namespace Tracker.Features.Account.Identity
{
   
    public class IdentityResult
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public Token Token { get; set; }
    }
    public class LoginUser : IRequest<OperationResult<IdentityResult>>
    {
        //public string Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public static IdentityResult Tologin(UsersDto user)
        {
            return new IdentityResult
            {
                Email = user.Email,
                Id=user.Id.ToString()
            };
        }

    }
    public class LoginHandler : IRequestHandler<LoginUser, OperationResult<IdentityResult>>
    {
        private readonly UserManager<UsersDto> _userManager;

        private readonly IdentityService _identityService;
        private OperationResult<IdentityResult> _result = new();
        private readonly ICollectionProvider _prov;
        public LoginHandler(UserManager<UsersDto> userManager, IdentityService identityService, ICollectionProvider prov)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _identityService = identityService;
            _prov = prov;
        }
        public async Task<OperationResult<IdentityResult>> Handle(LoginUser request, CancellationToken cancellationToken)
        {
            try
            {
                var identityUser = await ValidateAndGetIdentityAsync(request);
                if (_result.IsError) return _result;

                var collection = _prov.GetCollection<UsersDto>(CollectionNames.USERS);
                var user = await _prov.GetCollectionFristOrDefautFilter<UsersDto>(CollectionNames.USERS, "Id", identityUser.Id.ToString());
                _result.Payload = LoginUser.Tologin(user);
                _result.Payload.Email = identityUser.UserName.ToString();
                _result.Payload.Token = _identityService.GetJwtString(user);
                var list = new List<Token>();
                list.Add(_result.Payload.Token);
                await _identityService.SaveToken(identityUser, collection, list);
                return _result;
            }
            catch (Exception e)
            {
                _result.AddUnknownError(e.Message);
            }
            return _result;   
        }
        private async Task<UsersDto> ValidateAndGetIdentityAsync(LoginUser request)
        {
            var identityUser = await _userManager.FindByEmailAsync(request.Email);

            if (identityUser is not null)
            {
                var validPassword = await _userManager.CheckPasswordAsync(identityUser, request.Password);
                if (!validPassword)
                    _result.AddError(ErrorCode.IncorrectPassword, IdentityMessages.IncorrectPassword);
                return identityUser;
            }
            else
                _result.AddError(ErrorCode.IdentityUserDoesNotExist, IdentityMessages.NonExistentIdentityUser);

            return identityUser;
        }
        

    }
}
