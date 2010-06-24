using System;
using NinjaFerret.Wcf.Exception;
using NinjaFerret.Wcf.Sample.BankManager.Interface.Faults;

namespace NinjaFerret.Wcf.Sample.BankManager.Interface.Exception
{
    public class AccountDoesNotExistException : ApplicationException, IException
    {
        public AccountDoesNotExistException(string accountNumber) :
            base(string.Format("Could not find account {0}", accountNumber))
        {
            AccountNumber = accountNumber;
        }

        public string AccountNumber { get; private set; }

        public Fault ToFault()
        {
            return new AccountDoesNotExistFault {AccountNumber = AccountNumber};
        }
    }
}