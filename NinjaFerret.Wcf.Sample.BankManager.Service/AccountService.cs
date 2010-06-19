using System;
using System.Collections.Generic;
using System.Linq;
using Ninjaferret.Wcf.Sample.BankManager.Interface;
using Ninjaferret.Wcf.Sample.BankManager.Interface.Exception;
using Ninjaferret.Wcf.Sample.BankManager.Interface.Model;

namespace NinjaFerret.Wcf.Sample.BankManager.Service
{
    public class AccountService : IAccountService
    {
        private readonly List<BankAccount> _bankAccounts = new List<BankAccount>();

        public BankAccount Get(string accountNumber)
        {
            try
            {
                return _bankAccounts.Single(account => account.AccountNumber.Equals(accountNumber));
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
                return
                    _bankAccounts.Where(
                        account => account.CustomerName.Equals(customerName) && account.AccountType == accountType);
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