using LTS.API.Common.Middleware;
using Scalar.AspNetCore;

namespace LTS.API.Infrastructure.Services.Extensions
{
    public static class MiddleWareExtension
    {
        public static WebApplication MyMiddleWare(this WebApplication app)
        {
            //if (app.Environment.IsDevelopment())
            //{
            //    app.UseSwagger();
            //    app.UseSwaggerUI(options =>
            //    {
            //        options.SwaggerEndpoint("/swagger/v1/swagger.json", "LTS API V1");
            //        options.RoutePrefix = string.Empty; // Swagger at root
            //    });
            //    app.MapOpenApi();
            //}

            app.MapOpenApi();
            app.MapScalarApiReference(options =>
            {
                options.Title = "LTS API";
                options.Theme = ScalarTheme.DeepSpace;

                // JWT Auth
                options.AddHttpAuthentication("Bearer", scheme =>
                {
                    scheme.Description = "Login karo, token lo, yahan paste karo";
                });
            });
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            return app;
        }
    }
}
