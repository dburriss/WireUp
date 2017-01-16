namespace WireUp
{
    public interface IRouteDefinitionFactory
    {
        RouteDefinition Create(object obj, IMap mapper = null);
    }
}