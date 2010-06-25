using System;
using System.ServiceModel;
using NinjaFerret.Wcf.Exception;
using NinjaFerret.Wcf.Sample.BankManager.Interface.Faults;

namespace NinjaFerret.Wcf.Sample.BankManager.Interface.Exception
{
    public class AccountDoesNotExistException : ApplicationException, ITranslatableException
    {
        public AccountDoesNotExistException(string accountNumber) :
            base(string.Format("Could not find account {0}", accountNumber))
        {
            AccountNumber = accountNumber;
        }

        public string AccountNumber { get; private set; }

        public FaultException ToFaultException()
        {
            return
                new FaultException<AccountDoesNotExistFault>(
                    new AccountDoesNotExistFault {AccountNumber = AccountNumber},
                    "The account does not exist");
        }
    }
}