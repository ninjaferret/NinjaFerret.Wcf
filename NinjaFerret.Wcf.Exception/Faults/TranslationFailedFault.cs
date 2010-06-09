namespace NinjaFerret.Wcf.Exception.Faults
{
    public class TranslationFailedFault : Fault
    {
        public string Message { get; set; }
        public string TranslationFailureReason { get; set; }
    }
}