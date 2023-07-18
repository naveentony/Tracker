
using System.Diagnostics.Eventing.Reader;
using Tracker.Domain.Dtos;

namespace Tracker.Features.Account.Identity
{
    public enum UserType
    {
        ClientManager,
        Client,
        User,
        Manager

    }
    public class RegisterUser : IRequest<OperationResult<IdentityResult>>
    {
        public string RoleName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public static UsersDto ToUsersDto(RegisterUser user)
        {
            var usersDto = new UsersDto
            {
                UserName = user.Username,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
            };
            if (user.RoleName.ToUpper() == "SUPERMANAGER")
                usersDto.UserType = 0;
            else if (user.RoleName.ToUpper() == "CLIENT")
                usersDto.UserType = 1;
            else if (user.RoleName.ToUpper() == "MANAGER")
                usersDto.UserType = 2;
            else if (user.RoleName.ToUpper() == "USER")
                usersDto.UserType = 3;
            return usersDto;


        }

    }
    public class RegisterHandler : IRequestHandler<RegisterUser, OperationResult<IdentityResult>>
    {

        private readonly OperationResult<IdentityResult> _result = new();
        private readonly UserManager<UsersDto> _userManager;
        private readonly RoleManager<MongoRoleDto> _roleManager;
        private readonly IdentityService _identityService;
        private readonly ClientService _clientService;
        private readonly ICollectionProvider _prov;
        public HttpContext _httpContext => new HttpContextAccessor().HttpContext;
        public RegisterHandler(UserManager<UsersDto> userManager, RoleManager<MongoRoleDto> roleManager,
            IdentityService identityService, ICollectionProvider prov, ClientService clientService)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(_userManager));
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(_roleManager));
            _identityService = identityService ?? throw new ArgumentNullException(nameof(_identityService));
            _prov = prov;
            _clientService = clientService ?? throw new ArgumentNullException(nameof(clientService));
        }

        public async Task<OperationResult<IdentityResult>> Handle(RegisterUser request,
                CancellationToken cancellationToken)
        {
            try
            {
                var result = new OperationResult<IEnumerable<RegisterUser>>();
                var collection = _prov.GetCollection<UsersDto>(CollectionNames.USERS);
               // var clientDto = new ClientsDto();
                await ValidateUserAsync(request, collection);
                var role = await FindRolebyNameAsync(request);
                if (_result.IsError) return _result;
                var userdto = RegisterUser.ToUsersDto(request);
                userdto.CreatedDate = DateTime.Now;
                if (userdto.UserType==0 || userdto.UserType == 1)
                {
                    AddClientInfo(userdto);
                }
               userdto.ParentId = _httpContext.GetIdentityId();
                if (role != null)
                    userdto.Roles.Add(role.Id);
                await _userManager.CreateAsync(userdto,request.Password).ConfigureAwait(false);
                var identityUser = (await _userManager.FindByEmailAsync(request.Email));
                _result.Payload = LoginUser.Tologin(identityUser);
                _result.Payload.Token = _identityService.GetJwtString(identityUser);
                var list = new List<Token>();
                list.Add(_result.Payload.Token);
                await _identityService.SaveToken(identityUser, collection, list);
                await _identityService.AssigneUserToParent(userdto.Id, collection, userdto.ParentId);
                return _result;

            }
            catch (Exception e)
            {
                _result.AddError(ErrorCode.DatabaseOperationException, e.Message);
            }
            return _result;
        }
        private  UsersDto AddClientInfo(UsersDto userdto)
        {
            userdto.Client = new Client
            {
                Name = userdto.UserName
            };
            return userdto;
        }
        private async Task<MongoRoleDto> FindRolebyNameAsync(RegisterUser request)
        {
            var role = await _roleManager.FindByNameAsync(request.RoleName);
            if (role is null)
                _result.AddError(ErrorCode.ValidationError, IdentityMessages.InvalidRole);
            return role;
        }

        private async Task ValidateUserAsync(RegisterUser request, IMongoCollection<UsersDto> collection)
        {
            var user = await _userManager.FindByNameAsync(request.Username);
            if (user is not null)
                _result.AddError(ErrorCode.ValidationError, IdentityMessages.IdentityUserAlreadyExists);
            var email = await _userManager.FindByEmailAsync(request.Email);
            if (email is not null)
                _result.AddError(ErrorCode.ValidationError, IdentityMessages.IdentityEmailAlreadyExists);
            var phoneNumber = await _prov.GetCollectionFristOrDefautFilter<UsersDto>(CollectionNames.USERS, "PhoneNumber", request.PhoneNumber);
            if (phoneNumber is not null)
                _result.AddError(ErrorCode.ValidationError, IdentityMessages.PhoneNumber);

        }
        //private async Task ValidateClientAsync(RegisterUser request, IMongoCollection<ClientsDto> collection)
        //{
        //    var client = (await collection.FindAsync(x=>x.Name == request.Username)).FirstOrDefault();
        //    if (client is not null)
        //        _result.AddError(ErrorCode.ValidationError, ClinetMessages.ClientNameAlreadyExists);
        //}


    }
}
