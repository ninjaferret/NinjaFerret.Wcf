using System;
using System.Collections.Generic;
using System.ServiceModel;
using NinjaFerret.Wcf.Sample.BankManager.Interface;
using NinjaFerret.Wcf.Sample.BankManager.Interface.Exception;
using NinjaFerret.Wcf.Sample.BankManager.Interface.Faults;
using NinjaFerret.Wcf.Sample.BankManager.Interface.Model;
using NinjaFerret.Wcf.Sample.BankManager.Service;

namespace NinjaFerret.Wcf.Sample.BankManager.WcfHost
{
    public class AccountServiceWrapper : IAccountService
    {
        private readonly AccountService _accountService;

        public AccountServiceWrapper()
        {
            _accountService = new AccountService();
        }

        public BankAccount Get(string accountNumber)
        {
            Console.WriteLine("Get account {0}", accountNumber);
            try
            {
                return _accountService.Get(accountNumber);
            }
            catch (AccountDoesNotExistException e)
            {
                Console.WriteLine("No accounts found exception thrown: {0}", e.Message);
                var fault = new AccountDoesNotExistFault { AccountNumber = e.AccountNumber};
                throw new FaultException<AccountDoesNotExistFault>(fault, "Could not find any accounts matching the criteria");
            }
            catch (System.Exception e)
            {
                Console.WriteLine("Exception occurred: {0}", e.Message);
                throw new FaultException("There was an error retrieving account");
            }
        }

        public IEnumerable<BankAccount> Find(string customerName, AccountType accountType)
        {
            Console.WriteLine("Find account {0}/{1}", customerName, accountType);
            try
            {
                return _accountService.Find(customerName, accountType);
            }
            catch(NoAccountsFoundException e)
            {
                Console.WriteLine("No accounts found exception thrown: {0}", e.Message);
                var fault = new NoAccountsFoundFault {AccountType = e.AccountType, CustomerName = e.CustomerName};
                throw new FaultException<NoAccountsFoundFault>(fault, "Could not find any accounts matching the criteria");
            }
            catch (System.Exception e)
            {
                Console.WriteLine("Exception occurred: {0}", e.Message);
                throw new FaultException("There was an error searching accounts");
            }
        }

        public void Create(string customerName, AccountType accountType)
        {
            Console.WriteLine("Create account {0}/{1}", customerName, accountType);
            try
            {
                _accountService.Create(customerName, accountType);
            }
            catch (System.Exception e)
            {
                Console.WriteLine("Exception occurred: {0}", e.Message);
                throw new FaultException("There was an error creating account");
            }
        }
    }
}