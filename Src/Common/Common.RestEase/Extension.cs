using Microsoft.Extensions.DependencyInjection;
using RestEase;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Linq;
using System.Reflection;
namespace Common.RestEase
{
    public static class Extension
    {

        public static IServiceCollection AddRestEase(this IServiceCollection services)
        {
            var apiTypes = AppDomain.CurrentDomain.
                GetAssemblies().
                SelectMany(s => s.GetTypes()).
                Where(IsRestApi).
                ToArray();
            foreach (var apiType in apiTypes)
            {
                services.AddScoped(apiType, serviceProvider =>
                {
                    var attribute = apiType.GetCustomAttributes<RestApiAttribute>(false).Single();
                    var httpClientHandler = serviceProvider.GetRequiredService<CustomHttpClientHandler>();
                    var httpClient = new HttpClient(httpClientHandler)
                    {
                        BaseAddress = new Uri(attribute.BaseUrl),
                    };
                    var restClient = typeof(RestClient)
                        .GetMethod("For", genericParameterCount: 1, new[] { typeof(HttpClient) })
                        .MakeGenericMethod(apiType)
                        .Invoke(null, new object[] { httpClient });
                    return restClient;

                });
            }
            return services;
        }

        private static bool IsRestApi(Type arg)
        {
            return arg.GetCustomAttributes(typeof(RestApiAttribute), false).Any();
        }
    }
}
