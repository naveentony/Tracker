using MediatR;
using Tracker.Application.Abstractions;
using Tracker.Application.Models;
using Tracker.Features.LiveTracking;

namespace Tracker.Api.Endpoints
{
    public class LiveTrack : EndpointDefinition
    {
        public override void RegisterEndpoints(WebApplication app)
        {
            var accountGroup = app.MapGroup(ApiRoutes.BaseRoute);
            accountGroup.MapGet(ApiRoutes.Live.LiveTracking, LiveTracking);
            accountGroup.MapGet(ApiRoutes.Device.DeviceDetailsByUserId, HistoryTracking);
            //accountGroup.MapGet(ApiRoutes.Device.DeleteDeviceById, DeleteDevice);
            //accountGroup.MapPost(ApiRoutes.Device.RegisterDevice, RegisterDevice);
            //accountGroup.MapPost(ApiRoutes.Device.UpdateDevice, UpdateDevice);

        }
        private async Task<IResult> LiveTracking(IMediator mediator, string? VehicleId, CancellationToken token)
        {
            var PayLoad = new LiveData();// { VehicleId = VehicleId };
            var result = new OperationResult<LiveDataResult>();
            var response = await mediator.Send(PayLoad, token);
            return result.IsError ? HandleErrorResponse(result.Errors) : TypedResults.Ok(response);
        }
        private async Task<IResult> HistoryTracking(IMediator mediator, string VehicleId,DateTime FromDate, DateTime Todate, CancellationToken token)
        {
            var PayLoad = new LiveData();// { VehicleId = VehicleId };
            var result = new OperationResult<LiveDataResult>();
            var response = await mediator.Send(PayLoad, token);
            return result.IsError ? HandleErrorResponse(result.Errors) : TypedResults.Ok(response);
        }
    }
}
