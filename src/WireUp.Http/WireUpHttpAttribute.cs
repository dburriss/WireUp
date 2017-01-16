using System;

namespace WireUp
{
    [Key("HTTP")]
    public class WireUpHttpAttribute : WireUpAttribute
    {
        public string In { get; protected set; }
        public string Out { get; protected set; }
        public Type InType { get; protected set; }
        public Type OutType { get; protected set; }
        public string OutTransport { get; protected set; }
        public string Verb { get; protected set; }

        public WireUpHttpAttribute(string @in, string @out, Type inType, Type outType, string outTransport = "HTTP", string verb = "GET")
        {
            In = @in;
            Out = @out;
            InType = inType;
            OutType = outType;
            OutTransport = outTransport;
            Verb = verb;
        }
    }
}