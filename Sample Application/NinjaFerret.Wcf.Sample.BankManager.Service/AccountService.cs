using System;
using System.Collections.Generic;
using System.Linq;
using NinjaFerret.Wcf.Sample.BankManager.Interface;
using NinjaFerret.Wcf.Sample.BankManager.Interface.Exception;
using NinjaFerret.Wcf.Sample.BankManager.Interface.Model;

namespace NinjaFerret.Wcf.Sample.BankManager.Service
{
    public class AccountService : IAccountService
    {
        private static readonly List<BankAccount> _bankAccounts = new List<BankAccount>();

        public BankAccount Get(string accountNumber)
        {
            try
            {
                var bankAccount = _bankAccounts.Single(account => account.AccountNumber.Equals(accountNumber));
                return bankAccount;
            }
            catch(InvalidOperationException)
            {
                throw new AccountDoesNotExistException(accountNumber);
            }
        }

        public IEnumerable<BankAccount> Find(string customerName, AccountType accountType)
        {
            try
            {
                var bankAccounts = _bankAccounts.Where(
                    account => account.CustomerName.Equals(customerName) && account.AccountType == accountType);
                return bankAccounts;
            }
            catch(InvalidOperationException)
            {
                throw new NoAccountsFoundException(customerName, accountType);
            }
        }

        public void Create(string customerName, AccountType accountType)
        {
            var account = new BankAccount
                              {
                                  AccountNumber = _bankAccounts.Count.ToString("00000000"),
                                  AccountType = accountType,
                                  CustomerName = customerName,
                                  OverdraftLimit = 0
                              };
            _bankAccounts.Add(account);
        }
    }
}