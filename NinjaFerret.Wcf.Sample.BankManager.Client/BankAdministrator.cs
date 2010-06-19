using System.Collections.Generic;
using Ninjaferret.Wcf.Sample.BankManager.Interface;
using Ninjaferret.Wcf.Sample.BankManager.Interface.Model;

namespace NinjaFerret.Wcf.Sample.BankManager.Client
{
    public class BankAdministrator : IBankAdministrator
    {
        private readonly IAccountService _accountService;
        private readonly ITransactionService _transactionService;

        public BankAdministrator(IAccountService accountService, ITransactionService transactionService)
        {
            _accountService = accountService;
            _transactionService = transactionService;
        }

        public BankAccount Get(string accountNumber)
        {
            return _accountService.Get(accountNumber);
        }

        public IEnumerable<BankAccount> FindAccount(string customerName, AccountType accountType)
        {
            return _accountService.Find(customerName, accountType);
        }

        public void CreateAccount(string customerName, AccountType accountType)
        {
            _accountService.Create(customerName, accountType);
        }

        public void Deposit(string accountNumber, decimal amount, string description)
        {
            _transactionService.Deposit(_accountService.Get(accountNumber), amount, description);
        }

        public void Withdraw(string accountNumber, decimal amount, string description)
        {
            _transactionService.Withdraw(_accountService.Get(accountNumber), amount, description);
        }

        public void Transfer(string creditorAccountNumber, string debtorAccountNumber, decimal amount, string description)
        {
            _transactionService.Transfer(_accountService.Get(creditorAccountNumber), _accountService.Get(debtorAccountNumber), amount, description);
        }
    }
}
