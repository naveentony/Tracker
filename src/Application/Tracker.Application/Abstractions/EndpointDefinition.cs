﻿

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Tracker.Application.Abstractions
{
    public interface IEndpointDefinition
    {
        void RegisterEndpoints(WebApplication app);
    }
    public abstract class EndpointDefinition : IEndpointDefinition
    {
        public abstract void RegisterEndpoints(WebApplication app);

        protected IResult HandleErrorResponse(List<Error> errors)
        {
            var apiError = new ErrorResponse();

            if (errors.Any(e => e.Code == ErrorCode.NotFound))
            {
                var error = errors.FirstOrDefault(e => e.Code == ErrorCode.NotFound);

                apiError.StatusCode = 404;
                apiError.StatusPhrase = "Not Found";
                apiError.Timestamp = DateTime.Now;
                apiError.Errors.Add(error.Message);

                return Results.BadRequest(apiError);
            }
            apiError.StatusCode = 400;
            apiError.StatusPhrase = "Bad request";
            apiError.Timestamp = DateTime.Now;
            errors.ForEach(e => apiError.Errors.Add(e.Message));
            return Results.BadRequest(apiError);
        }

    }
}
