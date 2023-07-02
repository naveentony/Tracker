using MediatR;
using Tracker.Application.Abstractions;
using Tracker.Application.Models;
using Tracker.Features;
using Tracker.Features.Account.Identity;
using Tracker.Features.Device.DeviceVehicles;

namespace Tracker.Api.Endpoints
{
    public class Device : EndpointDefinition
    {
        public override void RegisterEndpoints(WebApplication app)
        {
            var accountGroup = app.MapGroup(ApiRoutes.BaseRoute);
            accountGroup.MapPost(ApiRoutes.Device.DeviceRegister, DeviceRegister);
        }
        private async Task<IResult> DeviceRegister(IMediator mediator, DeviceRegister register, CancellationToken token)
        {
            var result = new OperationResult<Unit>();
            result = await mediator.Send(register, token);
            return result.IsError ? HandleErrorResponse(result.Errors): TypedResults.CreatedAtRoute(result, nameof(GetDeviceById), new { id = register.DeviceNo});
        }
        private async Task<IResult> GetDeviceById(IMediator mediator,  CancellationToken token, string DeviceNo)
        {
            var result = new OperationResult<IEnumerable<Unit>>();
            var response = await mediator.Send(DeviceNo, token);
            //return TypedResults.Ok(response);
            return result.IsError ? HandleErrorResponse(result.Errors) : TypedResults.Ok(response);
        }
        private async Task<IResult> GetAllDeviceByUserId(IMediator mediator, CancellationToken token, string DeviceNo)
        {
            var result = new OperationResult<IEnumerable<Unit>>();
            var response = await mediator.Send(DeviceNo, token);
            //return TypedResults.Ok(response);
            return result.IsError ? HandleErrorResponse(result.Errors) : TypedResults.Ok(response);
        }
        private async Task<IResult> UpdateDevice(IMediator mediator, CancellationToken token, string DeviceNo)
        {
            var result = new OperationResult<IEnumerable<Unit>>();
            var response = await mediator.Send(DeviceNo, token);
            return TypedResults.Ok(response);
            //return result.IsError ? HandleErrorResponse(result.Errors) : TypedResults.CreatedAtRoute(response, nameof(GetDeviceById), new { id = register.DeviceNo });
        }
        private async Task<IResult> DeleteDevice(IMediator mediator, CancellationToken token, string DeviceNo)
        {
            var result = new OperationResult<IEnumerable<Unit>>();
            var response = await mediator.Send(DeviceNo, token);
            //return TypedResults.Ok(response);
            return result.IsError ? HandleErrorResponse(result.Errors) : TypedResults.Ok(response);
        }
    }
}
