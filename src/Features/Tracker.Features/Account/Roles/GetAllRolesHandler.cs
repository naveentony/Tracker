using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tracker.Domain.Settings;

namespace Tracker.Features.Account.Roles
{
    public class GetAllRoles : MongoRoleDto
    {
      
        public static List<GetAllRoles> ToGetAllRoles(List<MongoRoleDto> roles)
        {
            var list = new List<GetAllRoles>();
            roles.ForEach(tt
                => list.Add(GetAllRoles.ToGetAllRoles(tt)));
            return list;
        }
        public static GetAllRoles ToGetAllRoles(MongoRoleDto role)
        {
            return new GetAllRoles
            {
                Name = role.Name,
                Description = role.Description,
                Status = role.Status
            };
        }
    }
    public class GetAllRolesQuery : IRequest<OperationResult<IEnumerable<GetAllRoles>>>
    {

    }
    public class GetAllRolesHandler : IRequestHandler<GetAllRolesQuery, OperationResult<IEnumerable<GetAllRoles>>>
    {

        private readonly ICollectionProvider _prov;
        public GetAllRolesHandler(ICollectionProvider provider)
        {
            _prov = provider ?? throw new ArgumentNullException(nameof(_prov));

        }
        public async Task<OperationResult<IEnumerable<GetAllRoles>>> Handle(GetAllRolesQuery request,
                CancellationToken cancellationToken)
        {
            var result = new OperationResult<IEnumerable<GetAllRoles>>();
            var CollectionName = _prov.GetCollection<MongoRoleDto>(CollectionNames.ROLES);
            var filter =DataFilter.Filters();
            var data = await _prov.QueryByPage(CollectionName, filter);
            result.Payload = GetAllRoles.ToGetAllRoles(data.readOnlyList);
            result.TotalPages = data.totalPages;
            result.TotalCount = data.count;
            return result;
        }


    }
}
