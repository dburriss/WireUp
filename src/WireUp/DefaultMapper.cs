namespace WireUp
{
    public class DefaultMapper : IMap
    {
        public void Map<TFrom, TTo>(TFrom fromObj, TTo toObj)
        {
            PhilosophicalMonkey.Reflect.OnMappings.Map(fromObj, toObj);
        }
    }
}