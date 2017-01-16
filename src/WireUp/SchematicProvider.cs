using System;
using System.Collections.Generic;
using System.Reflection;
using PhilosophicalMonkey;

namespace WireUp
{
    public class SchematicProvider
    {
        private readonly IDictionary<string, List<Type>> _schematics;

        public IEnumerable<string> Keys => _schematics.Keys;

        public static SchematicProvider Create(Assembly[] assemblies)
        {
            var types = Reflect.OnTypes.GetAllTypes(assemblies);
            var schematics = GetWireUpSchematics(types);
            var provider = new SchematicProvider();
            foreach (var schematic in schematics)
            {
                var wireUpAttr = schematic.GetTypeInfo().GetCustomAttribute<WireUpAttribute>(true);
                var keyAttr = wireUpAttr.GetType().GetTypeInfo().GetCustomAttribute<KeyAttribute>(true);
                var key = keyAttr.Name;
                provider.Add(key, schematic);
            }
            return provider;
        }
        private static IEnumerable<Type> GetWireUpSchematics(IEnumerable<Type> types)
        {
            foreach (var t in types)
            {
                if (t.GetTypeInfo().IsDefined(typeof(WireUpAttribute)))
                {
                    yield return t;
                }
            }
        }

        private SchematicProvider()
        {
            this._schematics = new Dictionary<string, List<Type>>();
        }

        public IEnumerable<Type> Get(string key)
        {
            if (_schematics.ContainsKey(key))
            {
                return _schematics[key];
            }

            return new List<Type>();
        }

        public void Add(string key, Type type)
        {
            var attr = type.GetTypeInfo().GetCustomAttribute<WireUpAttribute>(true);
            if (attr == null)
            {
                throw new InvalidOperationException($"Schematic types must be decorated with an attribute that inherits from `{typeof(WireUpAttribute).Name}`");
            }
            if (!_schematics.ContainsKey(key))
            {
                _schematics[key] = new List<Type>();
            }
            if (!_schematics[key].Contains(type))
            {
                _schematics[key].Add(type);
            }
        }
    }
}
