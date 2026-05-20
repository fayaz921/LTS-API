using LTS.API.Common.Middleware;
using Scalar.AspNetCore;

namespace LTS.API.Infrastructure.Services.Extensions
{
    public static class MiddleWareExtension
    {
        public static WebApplication MyMiddleWare(this WebApplication app)
        {
                app.MapOpenApi();
                app.MapScalarApiReference(options =>
                {
                    options.Title = "LTS API";
                    options.Theme = ScalarTheme.DeepSpace;

                    options.AddHttpAuthentication("Bearer", scheme =>
                    {
                        scheme.Description = "Login and Paste the Jwt token";
                    });
                });
                app.UseMiddleware<ExceptionHandlingMiddleware>();
                app.UseHttpsRedirection();
                app.UseCors("AllowFrontend");
                app.UseAuthentication();
                app.UseAuthorization();
                app.MapControllers();
            return app;
        }
    }
}
