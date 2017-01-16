using Microsoft.AspNetCore.Http;

namespace WireUp
{
    public class RouteDefinition
    {
        public string Template { get; set; }
        public string Verb { get; set; }
        public RequestDelegate RequestDelegate { get; set; }
    }
}