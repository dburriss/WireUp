using System;
using System.Net.Http;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace WireUp.Test
{
    public class HttpWireUpFactoryTests
    {
        private static RouteDefinition CreateRouteDefinition(HttpWireUpFactory sut)
        {
            var mapper = new DefaultMapper();
            var def = sut.Create(new TestHttpSchematic(), mapper);
            return def;
        }

        [Fact]
        public void Create_ArgumentIsNull_ThrowsArgumentNullException()
        {
            var sut = new HttpWireUpFactory();
            Assert.Throws<ArgumentNullException>(() => sut.Create(null, null));
        }

        [Fact]
        public void Create_MapperIsNull_ThrowsArgumentNullException()
        {
            var sut = new HttpWireUpFactory();
            Assert.Throws<ArgumentNullException>(() => sut.Create(new object(), null));
        }

        [Fact]
        public void Create_WithNoAttribute_ThrowsInvalidOperationException()
        {
            var sut = new HttpWireUpFactory();
            Assert.Throws<InvalidOperationException>(() => sut.Create(new object(), new DefaultMapper()));
        }

        [Fact]
        public void Create_WithAttributeInGoogle_RouteDefinationTemplateIsGoogle()
        {
            var sut = new HttpWireUpFactory();
            var def = CreateRouteDefinition(sut);
            Assert.Equal("google/?q={q}", def.Template);
        }

        [Fact]
        public void Create_WithAttributeVerbGet_RouteDefinationVerbIsGet()
        {
            var sut = new HttpWireUpFactory();
            var def = CreateRouteDefinition(sut);
            Assert.Equal("GET", def.Verb);
        }

        [Fact]
        public void Create_FromAttribute_CreatesARequestDelegate()
        {
            var sut = new HttpWireUpFactory();
            var def = CreateRouteDefinition(sut);
            Assert.NotNull(def.RequestDelegate);
        }
    }

    public class DefaultMapper : IMap
    {
        public void Map<TFrom, TTo>(TFrom fromObj, TTo toObj)
        {
            throw new NotImplementedException();
        }
    }
}

namespace WireUp
{

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
