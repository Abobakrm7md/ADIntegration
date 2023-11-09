using System;
using WFCustomization.Core.Repositories;
using WFCustomization.Shared.Exceptions;
using WFCustomization.Shared;
using WFCustomization.Shared.Contexts;
using WFCustomization.Shared.Swagger;
using WFCustomization.Shared.Logging;
using WFCustomization.Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using WFCustomization.Application;
using MediatR;

namespace WFCustomization.Infrastructure
{
    public static class Extensions
    {
        private const string CorrelationIdKey = "correlation-id";

        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddErrorHandling();
            services.AddShared();
            services.AddContext();
            services.AddSwaggerDocs();
            services.AddMediatR(typeof(Extensions).Assembly, typeof(Application.Extensions).Assembly);
            var x = services.GetOptions<AppSettings>("app");
            services.AddSingleton(services.GetOptions<AppSettings>("app"));

            services.AddTransient<IOrdersRepository, OrdersRepository>();

            return services;
        }


        public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app)
        {
            app.UseCorrelationId();

            app.UseErrorHandling();

            app.UseSwaggerDocs();

            app.UseHttpsRedirection();

            app.UseContext();

            app.UseLogging();

            app.UseRouting();

            app.UseAuthorization();

            return app;
        }

    }
}
