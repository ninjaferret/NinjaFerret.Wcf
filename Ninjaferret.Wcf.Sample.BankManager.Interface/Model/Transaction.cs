namespace Ninjaferret.Wcf.Sample.BankManager.Interface.Model
{
    public class Transaction
    {
        public string AccountNumber { get; set; }
        public decimal Amount { get; set;}
        public string Description { get; set; }
    }
}