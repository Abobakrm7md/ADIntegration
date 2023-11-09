using System.Text.RegularExpressions;
using Microsoft.Extensions.DependencyInjection;

namespace WFCustomization.Application
{
    public static class Extensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            return services;
        }

    }
}
