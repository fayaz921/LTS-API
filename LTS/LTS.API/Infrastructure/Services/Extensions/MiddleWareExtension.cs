using LTS.API.Common.Middleware;

namespace LTS.API.Infrastructure.Services.Extensions
{
    public static class MiddleWareExtension
    {
        public static WebApplication MyMiddleWare(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "LTS API V1");
                    options.RoutePrefix = string.Empty; // Swagger at root
                });
                app.MapOpenApi();
            }
                app.UseMiddleware<ExceptionHandlingMiddleware>();
                app.UseHttpsRedirection();
                app.UseAuthentication();
                app.UseAuthorization();
                app.MapControllers();
                return app;
        }
    }
}
