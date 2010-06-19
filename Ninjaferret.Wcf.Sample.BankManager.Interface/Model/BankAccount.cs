namespace Ninjaferret.Wcf.Sample.BankManager.Interface.Model
{
    public class BankAccount
    {
        public string AccountNumber { get; set; }
        public string CustomerName { get; set; }
        public AccountType AccountType { get; set; }
        public decimal OverdraftLimit { get; set; }
    }
}