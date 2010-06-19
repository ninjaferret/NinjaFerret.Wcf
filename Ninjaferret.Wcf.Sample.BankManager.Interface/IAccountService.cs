using System.Collections.Generic;
using Ninjaferret.Wcf.Sample.BankManager.Interface.Model;

namespace Ninjaferret.Wcf.Sample.BankManager.Interface
{
    public interface IAccountService
    {
        BankAccount Get(string accountNumber);
        IEnumerable<BankAccount> Find(string customerName, AccountType accountType);
        void Create(string customerName, AccountType accountType);
    }
}