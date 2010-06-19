using System.Collections.Generic;
using Ninjaferret.Wcf.Sample.BankManager.Interface.Model;

namespace NinjaFerret.Wcf.Sample.BankManager.Client
{
    public interface IBankAdministrator
    {
        void CreateAccount(string customerName, AccountType accountType);
        void Deposit(string accountNumber, decimal amount, string description);
        void Withdraw(string accountNumber, decimal amount, string description);
        void Transfer(string creditorAccountNumber, string debtorAccountNumber, decimal amount, string description);
        IEnumerable<BankAccount> FindAccount(string customerName, AccountType accountType);
        BankAccount Get(string accountNumber);
    }
}