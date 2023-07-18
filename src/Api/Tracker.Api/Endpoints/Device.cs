using MediatR;
using Microsoft.Win32;
using Tracker.Application.Abstractions;
using Tracker.Application.Models;
using Tracker.Features.Device.Vehicles;

namespace Tracker.Api.Endpoints
{
    public class Device : EndpointDefinition
    {
        public override void RegisterEndpoints(WebApplication app)
        {
            var accountGroup = app.MapGroup(ApiRoutes.BaseRoute);
            accountGroup.MapGet(ApiRoutes.Device.DeviceDetailsByDeviceNo, GetDeviceDetailsByDeviceNo);
            accountGroup.MapGet(ApiRoutes.Device.DeviceDetailsByUserId, GetDeviceDetailsByUserId);
            accountGroup.MapGet(ApiRoutes.Device.DeleteDeviceById, DeleteDevice);
            accountGroup.MapPost(ApiRoutes.Device.RegisterDevice, RegisterDevice);
            accountGroup.MapPost(ApiRoutes.Device.UpdateDevice, UpdateDevice);
            
        }
        private async Task<IResult> RegisterDevice(IMediator mediator, VehicleRegister register, CancellationToken token)
        {
            var result = new OperationResult<Unit>();
            result = await mediator.Send(register, token);
            return result.IsError ? HandleErrorResponse(result.Errors): TypedResults.CreatedAtRoute(result, nameof(GetDeviceDetailsByDeviceNo), new { id = register.IMEI});
        }
        private async Task<IResult> GetDeviceDetailsByDeviceNo(IMediator mediator,  CancellationToken token, string DeviceNo)
        {
            var PayLoad = new AllVehiclesQuery { DeviceNo = DeviceNo };
            var result = new OperationResult<VehicleRegister>();
            var response = await mediator.Send(PayLoad, token);
            return result.IsError ? HandleErrorResponse(result.Errors) : TypedResults.Ok(response);
        }
        private async Task<IResult> GetDeviceDetailsByUserId(IMediator mediator, CancellationToken token, string UserId)
        {
            var PayLoad = new AllVehiclesQuery {  UserId = UserId };
            var result = new OperationResult<AllVehiclesQuery>();
            var response = await mediator.Send(PayLoad, token);
            return result.IsError ? HandleErrorResponse(result.Errors) : TypedResults.Ok(response);
        }
        private async Task<IResult> UpdateDevice(IMediator mediator, CancellationToken token, VehicleRegister request)
        {
            var result = new OperationResult<IEnumerable<Unit>>();
            var response = await mediator.Send(request, token);
            return result.IsError ? HandleErrorResponse(result.Errors) : TypedResults.CreatedAtRoute(result, nameof(GetDeviceDetailsByDeviceNo), new { id = request.IMEI });
        }
        private async Task<IResult> DeleteDevice(IMediator mediator, CancellationToken token, string IMEI)
        {
            var result = new OperationResult<VehicleRegister>();
            result.Payload.IMEI = IMEI;
            var response = await mediator.Send(result, token);
            return result.IsError ? HandleErrorResponse(result.Errors) : TypedResults.Ok(response);
        }
    }
}
