namespace WireUp.Http.Test
{
    [WireUpHttp(
        @in: "google/?q={q}", inType: typeof(string), 
        @out: "http://google.com/", outType:typeof(string), 
        outTransport: "HTTP", 
        verb: "POST")]
    public class TestHttpSchematic
    {}
}