namespace WireUp
{
    public interface IMap
    {
        void Map<TFrom, TTo>(TFrom fromObj, TTo toObj);
    }
}