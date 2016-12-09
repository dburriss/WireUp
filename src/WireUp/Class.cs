using System;
using System.Net.Http;
using System.Reflection;
using Microsoft.AspNetCore.Http;

namespace WireUp
{

    public class DefaultMapper : IMap
    {
        public void Map<TFrom, TTo>(TFrom fromObj, TTo toObj)
        {
            throw new NotImplementedException();
        }
    }
    public class HttpWireUpFactory : IRouteDefinitionFactory
    {
        public RouteDefinition Create(object obj, IMap mapper)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            if (mapper == null)
                throw new ArgumentNullException(nameof(mapper));

            WireUpHttpAttribute attribute = null;

            attribute = obj.GetType().GetTypeInfo().GetCustomAttribute<WireUpHttpAttribute>();
            if (attribute != null)
            {
                return new RouteDefinition()
                {
                    Template = attribute.In,
                    Verb = attribute.Verb,
                    RequestDelegate = CreateDelegate(attribute.Out, attribute.OutTransport, mapper, attribute.InType, attribute.OutType)
                };
            }
            throw new InvalidOperationException($"Class needs `{typeof(WireUpHttpAttribute).Name}` or {typeof(IWireUpHttp).Name}");
        }

        private RequestDelegate CreateDelegate(string @out, string outTransport, IMap mapper, Type inType, Type outType)
        {
            return ctx =>
            {
                //model bind to inValue
                //map to outValue
                using (var client = new HttpClient())
                {
                    var result = client.GetAsync(@out).Result;
                    return ctx.Response.WriteAsync(result.Content.ReadAsStringAsync().Result);
                }
            };
        }
    }

    public interface IMap
    {
        void Map<TFrom, TTo>(TFrom fromObj, TTo toObj);
    }

    public interface IWireUpHttp
    {
    }

    public class RouteDefinition
    {
        public string Template { get; set; }
        public string Verb { get; set; }
        public RequestDelegate RequestDelegate { get; set; }
    }

    public interface IRouteDefinitionFactory
    {
        RouteDefinition Create(object obj, IMap mapper);
    }
}