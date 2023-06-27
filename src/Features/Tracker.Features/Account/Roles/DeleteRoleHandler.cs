using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tracker.Features.Shared;

namespace Tracker.Features.Account.Roles
{
    public class DeleteRoleHandler : IRequestHandler<Role, OperationResult<Unit>>
    {

        private readonly ICollectionProvider _prov;
        private readonly OperationResult<Unit> _result = new();
        private readonly RoleManager<MongoRoleDto> _roleManager;
        public DeleteRoleHandler(ICollectionProvider provider, RoleManager<MongoRoleDto> roleManager, UserManager<UsersDto> userManager)
        {
            _prov = provider ?? throw new ArgumentNullException(nameof(_prov));
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(_roleManager));
        }

        public async Task<OperationResult<Unit>> Handle(Role request,
                CancellationToken cancellationToken)
        {
            try
            {
                var result = new OperationResult<IEnumerable<Role>>();
                await ValidateAsync(request);
                if (_result.IsError) return _result;
                var role = Role.ToAddOrUpdateRole(request);
                role.CreatedDate = DateTime.Now;
                await _roleManager.DeleteAsync(role).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                _result.AddError(ErrorCode.DatabaseOperationException, e.Message);
            }
            return _result;
        }
        private async Task<MongoRoleDto> ValidateAsync(Role request)
        {
            var identityRole = await _roleManager.FindByNameAsync(request.Name);

            if (identityRole is null)
                _result.AddError(ErrorCode.IdentityUserDoesNotExist, IdentityErrorMessages.NonExistentIdentityRole);

            return identityRole;
        }


    }
}
