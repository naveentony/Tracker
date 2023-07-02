using MediatR;
using Tracker.Application.Abstractions;
using Tracker.Features.Device.VehicleTypes;

namespace Tracker.Api.Endpoints
{
    public class VehicleType : EndpointDefinition
    {
        public override void RegisterEndpoints(WebApplication app)
        {
            var studentGroup = app.MapGroup(ApiRoutes.BaseRoute);
            studentGroup.MapGet(ApiRoutes.TrackerType.GetAll, GetAllVehicleType);//.RequireAuthorization();
        }
        private async Task<IResult> GetAllVehicleType(IMediator mediator, CancellationToken token)
        {

            var query = new GetAllVehicleTypeQuery();
            var response = await mediator.Send(query, token);
            return TypedResults.Ok(response);
        }
    }
}
