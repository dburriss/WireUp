using System;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace WireUp
{
    [Key("HTTP")]
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
                var data = ctx;
                string json = string.Empty;
                var stream = data.Request.Body;
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    json =  reader.ReadToEnd();
                }

                //map to outValue
                var inObj = JsonConvert.DeserializeObject(json, inType);
                //var outObj = Activator.CreateInstance(outType);
                //mapper.Map(inObj, outObj);

                //TODO: to out transport
                using (var client = new HttpClient())
                {
                    var result = client.GetAsync(@out).Result;
                    return ctx.Response.WriteAsync(result.Content.ReadAsStringAsync().Result);
                }
            };
        }
    }
}