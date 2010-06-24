using System;
using NinjaFerret.Wcf.Exception;
using NinjaFerret.Wcf.Sample.BankManager.Interface.Faults;

namespace NinjaFerret.Wcf.Sample.BankManager.Interface.Exception
{
    public class InsufficientFundsException : ApplicationException, IException
    {
        public InsufficientFundsException(string accountNumber, decimal amount)
            : base(string.Format("There are insufficient funds to withdraw {0} from account {1}", amount, accountNumber))
        {
            AccountNumber = accountNumber;
            Amount = amount;
        }

        public string AccountNumber { get; private set; }

        public decimal Amount { get; private set; }

        public Fault ToFault()
        {
            return new InsufficientFundsFault{ AccountNumber = AccountNumber, Amount = Amount};
        }
    }
}