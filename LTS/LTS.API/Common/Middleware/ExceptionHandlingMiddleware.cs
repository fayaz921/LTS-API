using LTS.API.Common.Exceptions;
using LTS.API.Common.Response;
using LTS.API.Domain.Enums;
using System.Net;
using ValidationException = LTS.API.Common.Exceptions.ValidationException;

namespace LTS.API.Common.Middleware;

public class ExceptionHandlingMiddleware(
    RequestDelegate next,
    ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (NotFoundException ex)
        {
            logger.LogWarning(ex, "Not found: {Message}", ex.Message);
            await WriteResponse(context, HttpStatusCode.NotFound,
                ApiResponse<string>.Fail(ex.Message, ResponseType.NotFound));
        }
        catch (ValidationException ex)
        {
            logger.LogWarning("Validation failed: {Errors}", ex.Errors);
            await WriteResponse(context, HttpStatusCode.BadRequest,
                ApiResponse<string>.Fail(string.Join(", ", ex.Errors), ResponseType.BadRequest));
        }
        catch (UnauthorizedException ex)
        {
            logger.LogWarning(ex, "Unauthorized: {Message}", ex.Message);
            await WriteResponse(context, HttpStatusCode.Unauthorized,
                ApiResponse<string>.Fail(ex.Message, ResponseType.Unauthorized));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception: {Message}", ex.Message);
            await WriteResponse(context, HttpStatusCode.InternalServerError,
                ApiResponse<string>.Fail("Something went wrong. Please try again.", ResponseType.ServerError));
        }
    }

    private static async Task WriteResponse<T>(
        HttpContext context,
        HttpStatusCode statusCode,
        ApiResponse<T> response)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        await context.Response.WriteAsJsonAsync(response);
    }
}