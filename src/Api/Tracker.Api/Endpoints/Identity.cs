using MediatR;
using Tracker.Application.Abstractions;
using Tracker.Application.Models;
using Tracker.Features.Account.Identity;
using Tracker.Features.Account.Roles;
using Tracker.Features.Account.Users;
using Tracker.ImportData.Managers;

namespace Tracker.Api.Endpoints
{
    public class Identity : EndpointDefinition
    {
        public override void RegisterEndpoints(WebApplication app)
        {
            var accountGroup = app.MapGroup(ApiRoutes.BaseRoute);
            accountGroup.MapPost(ApiRoutes.Account.Login, UserLogin);
            accountGroup.MapPost(ApiRoutes.Account.Register, UserRegister);
            accountGroup.MapPost(ApiRoutes.Account.CreateRole, CreateRole);
        }
        private async Task<IResult> UserLogin(IMediator mediator, LoginUser login, CancellationToken token)
        {
            var result = new OperationResult<IEnumerable<IdentityResult>>();
            var response = await mediator.Send(login, token);
            return TypedResults.Ok(response);
        }
        private async Task<IResult> UserRegister(IMediator mediator, RegisterUser register, CancellationToken token)
        {
            var result = new OperationResult<IEnumerable<IdentityResult>>();
            var response = await mediator.Send(register, token);
            return TypedResults.Ok(response);
        }

        private async Task<IResult> CreateRole(IMediator mediator, Role role, CancellationToken token)
        {
            var result = new OperationResult<IEnumerable<GetAllRoles>>();
            var response = await mediator.Send(role, token);
            return TypedResults.Ok(response);
        }
        

    }
}
