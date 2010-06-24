namespace NinjaFerret.Wcf.Exception
{
    public interface IException
    {
        Fault ToFault();
    }
}