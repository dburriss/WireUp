namespace WireUp.Test
{
    [WireUpHttp(
        @in: "google/?q={q}", inType: typeof(string), 
        @out: "http://google.com/", outType:typeof(string), 
        outTransport: "HTTP", 
        verb: "GET")]
    public class TestHttpSchematic
    {}
}