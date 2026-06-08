using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using RentalManager.Middlewares;

namespace RentalManager.DI.Core
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseAppMiddlewares(this IApplicationBuilder app)
        {
            app.UseRouting();

            app.UseCors("AllowReactApp");

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseMiddleware<ExceptionMiddleware>();
            app.UseMiddleware<JwtAccountResolutionMiddleware>();
            app.UseMiddleware<WhatsAppAccountResolutionMiddleware>();
            app.UseMiddleware<AccountAccessMiddleware>();

            app.UseAuthorization();

            // Swagger ONLY in development
            var env = app.ApplicationServices.GetRequiredService<IHostEnvironment>();

            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }


            return app;
        }
    }
}