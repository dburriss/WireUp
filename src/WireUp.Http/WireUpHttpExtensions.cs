using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using PhilosophicalMonkey;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace WireUp
{
    public static class WireUpHttpExtensions
    {
        public static void AddWireUpHttp(this IServiceCollection services, params Assembly[] assemblies)
        {
            services.AddWireUpHttp(null, assemblies);
        }

        public static void AddWireUpHttp(this IServiceCollection services, WireUpHttpOptions httpOptions = null, params Assembly[] assemblies)
        {
            //wire up providers that can return definitions from types in the assemblies
            if (httpOptions == null)
            {
                httpOptions = new WireUpHttpOptions();
            }
            if (assemblies.Length == 0)
            {
                assemblies = new Assembly[] { Assembly.GetEntryAssembly() };
            }

            services.TryAddSingleton<SchematicProvider>(SchematicProvider.Create(assemblies));

            services.AddSingleton<IRouteDefinitionFactory, HttpWireUpFactory>();

        }

        public static void UseWireUpHttp(this IApplicationBuilder app)
        {
            var routeBuilder = new RouteBuilder(app);
            var schematicProvider = app.ApplicationServices.GetService<SchematicProvider>();
            var keys = schematicProvider.Keys;
            var factories = app.ApplicationServices.GetServices<IRouteDefinitionFactory>().ToList();
            var mapper = app.ApplicationServices.GetService<IMap>() ?? new DefaultMapper();

            foreach (var key in keys)
            {
                var schematics = schematicProvider.Get(key).ToList();
                foreach (var factory in factories)
                {
                    var factoryKeyAttr = factory.GetType().GetTypeInfo().GetAttribute<KeyAttribute>();
                    if (factoryKeyAttr.Name == key)
                    {
                        foreach (var schematicType in schematics)
                        {
                            var s = Activator.CreateInstance(schematicType);
                            var def = factory.Create(s, mapper);
                            routeBuilder.MapVerb(def.Verb, @def.Template, def.RequestDelegate);
                        }
                    }
                }
            }

            var routes = routeBuilder.Build();
            app.UseRouter(routes);
        }
    }
}