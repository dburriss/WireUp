//using System;
//using System.Collections.Generic;
//using Microsoft.AspNetCore.Builder;
//using Microsoft.Extensions.DependencyInjection;
//using System.Reflection;
//using PhilosophicalMonkey;
//using Microsoft.AspNetCore.Routing;
//using Microsoft.AspNetCore.Http;
//using System.Threading.Tasks;

//namespace WireUp
//{
//    public static class WireUpExtensions
//    {
//        private static IEnumerable<Type> GetWireUpSchematics(IEnumerable<Type> types)
//        {            
//            foreach (var t in types)
//            {
//                if(t.GetTypeInfo().IsDefined(typeof(WireUpAttribute)))
//                {
//                    yield return t;
//                }
//            }
//        }

//        public static void UseWireUp(this IApplicationBuilder app)
//        {
//            var defaultHandler =  new RouteHandler(new RequestDelegate(ctx => {       
//                return Task.FromResult(0);
//            }));
//            var routeBuilder = new RouteBuilder(app, defaultHandler);

//            var schematicProvider = (SchematicProvider)app.ApplicationServices.GetService(typeof(SchematicProvider));
//            var schematics = schematicProvider.Get("HTTP");

//            //this is all POC. Needs to come from schematics (provider)
//            //var types = Reflect.OnTypes.GetAllTypes(new Assembly[] {Assembly.GetEntryAssembly()});
//            //var schematics = GetWireUpSchematics(types);
//            //var factory = new HttpWireUpFactory();
//            //foreach (var schematicType in schematics)
//            //{
//            ////    var s = Activator.CreateInstance(schematicType);
//            //    var def = factory.Create(s, new DefaultMapper());
//            //    routeBuilder.MapVerb(def.Verb, @def.Template, def.RequestDelegate);
//            //}

//            //routeBuilder.MapVerb("GET", "google", ctx => {
//            //    using(var client = new HttpClient())
//            //    {
//            //        var result = client.GetAsync("http://google.com/?q=bob").Result;
//            //        return ctx.Response.WriteAsync(result.Content.ReadAsStringAsync().Result);
//            //    }
//            //});

//            var routes = routeBuilder.Build();
//            app.UseRouter(routes);
//        }
//    }
//}