using System;

namespace Ninjaferret.Wcf.Sample.BankManager.Interface.Exception
{
    public class AccountDoesNotExistException : ApplicationException
    {
        public AccountDoesNotExistException(string accountNumber) :
            base(string.Format("Could not find account {0}", accountNumber))
        {
            AccountNumber = accountNumber;
        }

        protected string AccountNumber { get; private set; }
    }
}