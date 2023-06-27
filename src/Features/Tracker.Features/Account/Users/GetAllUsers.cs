using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tracker.Domain.Settings;

namespace Tracker.Features.Account.Users
{
    [BsonIgnoreExtraElements]
    public class UsersResult : UsersDto
    {

        public static UsersResult FromTrackerTypeDto(VehicleTypeDto trackerTypedto)
        {
            return new UsersResult
            {
                //Id = trackerTypedto.Id,
                //Vehicle = trackerTypedto.Vehicle,
                //Amount = trackerTypedto.Amount,
                //Status = trackerTypedto.Status

            };
        }
        public static List<UsersResult> FromUserDtoToList(List<UsersDto> tlist)
        {
            var list = new List<UsersResult>();
            tlist.ForEach(tt
                => list.Add(ToUserResult(tt)));
            return list; ;
        }
        public static UsersResult ToUserResult(UsersDto user)
        {
            return new UsersResult
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Tokens = user.Tokens
            };
        }
    }
    public class GetAllUsersQuery : IRequest<OperationResult<IEnumerable<UsersResult>>>
    {
    }
    public class GetAllVehicleTypeQueryHandler
        : IRequestHandler<GetAllUsersQuery, OperationResult<IEnumerable<UsersResult>>>
    {

        private readonly ICollectionProvider _prov;
        public GetAllVehicleTypeQueryHandler(ICollectionProvider provider)
        {
            _prov = provider ?? throw new ArgumentNullException(nameof(_prov));

        }
        public async Task<OperationResult<IEnumerable<UsersResult>>> Handle(GetAllUsersQuery request,
                CancellationToken cancellationToken)
        {
            var result = new OperationResult<IEnumerable<UsersResult>>();
            var CollectionName = _prov.GetCollection<UsersDto>(CollectionNames.USERS);
            var filter = DataFilter.Filters();
            var data = await _prov.QueryByPage(CollectionName, filter);
            result.Payload = UsersResult.FromUserDtoToList(data.readOnlyList);
            result.TotalPages = data.totalPages;
            result.TotalCount = data.count;
            return result;
        }


    }
}
