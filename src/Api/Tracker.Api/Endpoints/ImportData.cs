using MediatR;
using Tracker.Application.Abstractions;
using Tracker.Features.Manage.ImportData;

namespace Tracker.Api.Endpoints
{
    public class ImportData : EndpointDefinition
    {
        public override void RegisterEndpoints(WebApplication app)
        {
            var accountGroup = app.MapGroup(ApiRoutes.BaseRoute);
            accountGroup.MapGet(ApiRoutes.Import.ImportData, ImportDataFromSql);
        }
        private async Task<IResult> ImportDataFromSql(IMediator mediator, CancellationToken token)
        {
            var query = new ImportDataFromSql();
            var response = await mediator.Send(query, token);
            return TypedResults.Ok(response);
        }
    }
}
