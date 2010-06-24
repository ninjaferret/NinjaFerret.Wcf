using System.Collections.Generic;
using System.Linq;
using NinjaFerret.Wcf.Sample.BankManager.Interface;
using NinjaFerret.Wcf.Sample.BankManager.Interface.Exception;
using NinjaFerret.Wcf.Sample.BankManager.Interface.Model;

namespace NinjaFerret.Wcf.Sample.BankManager.Service
{
    public class TransactionService : ITransactionService
    {
        private static readonly Dictionary<string , IList<Transaction>> _transactions = new Dictionary<string, IList<Transaction>>(); 

        public void Deposit(BankAccount account, decimal amount, string description)
        {
            if (!_transactions.ContainsKey(account.AccountNumber))
            {
                _transactions.Add(account.AccountNumber, new List<Transaction>());
            }
            var transaction = new Transaction { AccountNumber = account.AccountNumber, Amount = amount, Description = description };
            _transactions[account.AccountNumber].Add(transaction);
        }

        public void Withdraw(BankAccount account, decimal amount, string description)
        {
            if (!_transactions.ContainsKey(account.AccountNumber))
            {
                _transactions.Add(account.AccountNumber, new List<Transaction>());
            }
            if (Balance(account) < amount)
            {
                throw new InsufficientFundsException(account.AccountNumber, amount);
            }
            var transaction = new Transaction { AccountNumber = account.AccountNumber, Amount = -amount, Description = description };
            _transactions[account.AccountNumber].Add(transaction);
        }

        public void Transfer(BankAccount creditorAccount, BankAccount debtorAccount, decimal amount, string description)
        {
            Withdraw(debtorAccount, amount, description);
            Deposit(creditorAccount, amount, description);
        }

        private decimal Balance(BankAccount bankAccount)
        {
            return _transactions[bankAccount.AccountNumber].Sum(account => account.Amount);
        }
    }
}