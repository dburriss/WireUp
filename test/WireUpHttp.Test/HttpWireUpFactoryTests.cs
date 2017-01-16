using System;
using Xunit;

namespace WireUp.Http.Test
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
            Assert.Equal("POST", def.Verb);
        }

        [Fact]
        public void Create_FromAttribute_CreatesARequestDelegate()
        {
            var sut = new HttpWireUpFactory();
            var def = CreateRouteDefinition(sut);
            Assert.NotNull(def.RequestDelegate);
        }
    }

}

