using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using PhilosophicalMonkey;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Net.Http;

namespace WireUp
{
    public static class WireUpExtensions
    {
        public static void AddWireUpHttp(this IServiceCollection services, params Assembly[] assemblies)
        {
            //wire up providers that can return definitions from types in the assemblies
            var types = Reflect.OnTypes.GetAllTypes(assemblies);
            var schematics = GetWireUpSchematics(types);

        }

        private static IEnumerable<Type> GetWireUpSchematics(IEnumerable<Type> types)
        {            
            foreach (var t in types)
            {
                if(t.GetTypeInfo().IsDefined(typeof(WireUpAttribute)))
                {
                    yield return t;
                }
            }
        }

        public static void UseWireUp(this IApplicationBuilder app)
        {
            var defaultHandler =  new RouteHandler(new RequestDelegate(ctx => {       
                return Task.FromResult(0);
            }));
            var routeBuilder = new RouteBuilder(app, defaultHandler);
            //routeBuilder.MapVerb()
            routeBuilder.MapVerb("GET", "google", ctx => {
                using(var client = new HttpClient())
                {
                    var result = client.GetAsync("http://google.com/?q=bob").Result;
                    return ctx.Response.WriteAsync(result.Content.ReadAsStringAsync().Result);
                }
            });

            var routes = routeBuilder.Build();
            app.UseRouter(routes);
        }
    }
}