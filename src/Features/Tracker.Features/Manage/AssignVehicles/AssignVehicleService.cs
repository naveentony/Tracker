namespace Tracker.Features.Manage.AssignVehicles
{
    public class AssignVehicleService
    {
        private readonly CollectionProvider _provider;
        public HttpContext _httpContext => new HttpContextAccessor().HttpContext;
        public AssignVehicleService(CollectionProvider provider)
        {
            _provider = provider;
        }
        public async Task<string> AssignVehile(string VehicleId)
        {
            var AssignVehilce = _provider.GetCollection<AssignVehiclesDto>(CollectionNames.AssignVehicles);
            var request = new AssignVehiclesDto
            {
                UserId = _httpContext.GetIdentityIdClaimValue(),
                VehicleId = VehicleId,
                CreateDate = DateTime.UtcNow
            };
            await AssignVehilce.InsertOneAsync(request);
            return request.Id;
        }
    }
}
