using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using PhilosophicalMonkey;
using Xunit;

namespace WireUp.Test
{
    public class SchematicProviderTests
    {
        private static SchematicProvider Sut()
        {
            var type = typeof(TestHttpSchematic);
            var a = Reflect.OnTypes.GetAssembly(type);
            return SchematicProvider.Create(new Assembly[] { a });
        }

        private static SchematicProvider EmptySut()
        {
            return SchematicProvider.Create(new Assembly[] { });
        }

        [Fact]
        public void Add_TypeNotSchematic_ThrowsInvalidOperationException()
        {
            var sut = EmptySut();
            Assert.Throws<InvalidOperationException>(() => sut.Add("test", typeof(object)));
        }

        [Fact]
        public void Add_TypeToEmptyProvider_IsInKeys()
        {
            var sut = EmptySut();
            var t = typeof(TestHttpSchematic);
            sut.Add("test", t);
            Assert.Contains("test", sut.Keys);
        }

        [Fact]
        public void Get_WithEmptyProvider_ReturnsEmptyList()
        {
            var sut = EmptySut();
            Assert.Empty(sut.Get("Does_not_exist"));
        }

        [Fact]
        public void Get_WithNonEmptyProvider_ReturnsList()
        {
            var sut = Sut();
            Assert.NotEmpty(sut.Get("HTTP"));
        }

        [Fact]
        public void Add_TypeToEmptyProvider_ContainsType()
        {
            var sut = EmptySut();
            var t = typeof(TestHttpSchematic);
            sut.Add("test", t);
            Assert.Contains(t, sut.Get("test"));
        }

        [Fact]
        public void Create_AssemblyContainsOneSchematic_ProviderHasOneSchematic()
        {
            var sut = Sut();
            Assert.Equal(1, sut.Get("HTTP").Count());
        }

        [Fact]
        public void Add_TypeTwice_IsIgnored()
        {
            var sut = Sut();
            var t = typeof(TestHttpSchematic);
            sut.Add("HTTP", t);
            Assert.Equal(1, sut.Get("HTTP").Count());
        }



    }
}
