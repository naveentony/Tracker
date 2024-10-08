﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tracker.Features.Shared;

namespace Tracker.Features.Account.Roles
{
    public class Role : IRequest<OperationResult<Unit>>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public StatusType Status { get; set; }
        public static MongoRoleDto ToAddOrUpdateRole(Role role)
        {
            return new MongoRoleDto
            {
                Name = role.Name,
                Description = role.Description,
                Status = role.Status.ToString()
            };
        }
    }

    public class AddRoleHandler : IRequestHandler<Role, OperationResult<Unit>>
    {

        private readonly OperationResult<Unit> _result = new();
        private readonly RoleManager<MongoRoleDto> _roleManager;
        public AddRoleHandler(RoleManager<MongoRoleDto> roleManager)
        {
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
                
                await _roleManager.CreateAsync(role).ConfigureAwait(false);
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

            if (identityRole is not null)
                _result.AddError(ErrorCode.IdentityUserAlreadyExists, IdentityMessages.RoleAlreadyExists);

            return identityRole;
        }


    }
}
