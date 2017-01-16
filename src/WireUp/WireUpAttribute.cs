namespace WireUp
{
    [System.AttributeUsage(System.AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public abstract class WireUpAttribute : System.Attribute
    {
    }

    [System.AttributeUsage(System.AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class KeyAttribute : System.Attribute
    {
        public string Name { get; }

        public KeyAttribute(string name)
        {
            Name = name;
        }
    }
}