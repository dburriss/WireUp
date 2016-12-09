namespace WireUp
{
    [System.AttributeUsage(System.AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public abstract class WireUpAttribute : System.Attribute
    {
        public WireUpAttribute ()
        {
          
        }
    }
}