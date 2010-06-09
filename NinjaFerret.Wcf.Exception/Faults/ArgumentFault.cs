namespace NinjaFerret.Wcf.Exception.Faults
{
    public class ArgumentFault : Fault
    {
        public string Message { get; set; }
        public string Parameter { get; set; }
    }
}