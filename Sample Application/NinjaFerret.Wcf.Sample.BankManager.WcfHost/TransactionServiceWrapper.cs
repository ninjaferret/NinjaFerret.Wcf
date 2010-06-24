using System;
using System.ServiceModel;
using NinjaFerret.Wcf.Sample.BankManager.Interface;
using NinjaFerret.Wcf.Sample.BankManager.Interface.Exception;
using NinjaFerret.Wcf.Sample.BankManager.Interface.Faults;
using NinjaFerret.Wcf.Sample.BankManager.Interface.Model;
using NinjaFerret.Wcf.Sample.BankManager.Service;

namespace NinjaFerret.Wcf.Sample.BankManager.WcfHost
{
    public class TransactionServiceWrapper : ITransactionService
    {
        private readonly TransactionService _transactionService;

        public TransactionServiceWrapper()
        {
            _transactionService = new TransactionService();
        }

        public void Deposit(BankAccount account, decimal amount, string description)
        {
            Console.WriteLine("Deposit of {0} to account {1}", amount, account.AccountNumber);
            try
            {
                _transactionService.Deposit(account, amount, description);
            }
            catch (System.Exception e)
            {
                Console.WriteLine("Exception occurred: {0}", e.Message);
                throw new FaultException("An error occurred despositing money to an account");
            }
            Console.WriteLine("Successfully deposited");
        }

        public void Withdraw(BankAccount account, decimal amount, string description)
        {
            Console.WriteLine("Withdrawal of {0} from account {1}", amount, account.AccountNumber);
            try
            {
                _transactionService.Withdraw(account, amount, description);
            }
            catch(InsufficientFundsException e)
            {
                Console.WriteLine("Exception occurred: {0}", e.Message);
                var fault = new InsufficientFundsFault {AccountNumber = e.AccountNumber, Amount = e.Amount};
                throw new FaultException<InsufficientFundsFault>(fault, "There were insufficient funds to withdraw the amount");
            }
            catch (System.Exception e)
            {
                Console.WriteLine("Exception occurred: {0}", e.Message);
                throw new FaultException("An error occurred withdrawing from an account");
            }
            Console.WriteLine("Successfully withdrawn");
        }

        public void Transfer(BankAccount creditorAccount, BankAccount debtorAccount, decimal amount, string description)
        {
            Console.WriteLine("Traansfer of {0} from account {1} to account {2}", amount, debtorAccount.AccountNumber, creditorAccount);
            try
            {
                _transactionService.Transfer(creditorAccount, debtorAccount, amount, description);
            }
            catch (InsufficientFundsException e)
            {
                Console.WriteLine("Exception occurred: {0}", e.Message);
                var fault = new InsufficientFundsFault { AccountNumber = e.AccountNumber, Amount = e.Amount };
                throw new FaultException<InsufficientFundsFault>(fault, "There were insufficient funds to withdraw the amount");
            }
            catch (System.Exception e)
            {
                Console.WriteLine("Exception occurred: {0}", e.Message);
                throw new FaultException("An error occurred transferring money between accounts");
            }
            Console.WriteLine("Successfully withdrawn");
        }
    }
}