namespace WireUp.Host
{
    [WireUpHttp(
        @in: "google", inType: typeof(string), 
        @out: "http://google.com/", outType:typeof(string), 
        outTransport: "HTTP", 
        verb: "GET")]
    public class TestHttpSchematic : IWireUpHttp
    {}
}