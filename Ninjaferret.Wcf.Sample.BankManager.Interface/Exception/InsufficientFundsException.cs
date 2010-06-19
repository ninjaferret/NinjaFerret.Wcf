using System;

namespace Ninjaferret.Wcf.Sample.BankManager.Interface.Exception
{
    public class InsufficientFundsException : ApplicationException
    {
        public InsufficientFundsException(string accountNumber, decimal amount)
            : base(string.Format("There are insufficient funds to withdraw {0} from account {1}", amount, accountNumber))
        {
            AccountNumber = accountNumber;
            Amount = amount;
        }

        public string AccountNumber { get; private set; }
        public decimal Amount { get; private set; }
    }
}