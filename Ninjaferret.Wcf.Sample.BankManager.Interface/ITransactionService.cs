using Ninjaferret.Wcf.Sample.BankManager.Interface.Model;

namespace Ninjaferret.Wcf.Sample.BankManager.Interface
{
    public interface ITransactionService
    {
        void Deposit(BankAccount account, decimal amount, string description);
        void Withdraw(BankAccount account, decimal amount, string description);
        void Transfer(BankAccount creditorAccount, BankAccount debtorAccount, decimal amount, string description);
    }
}