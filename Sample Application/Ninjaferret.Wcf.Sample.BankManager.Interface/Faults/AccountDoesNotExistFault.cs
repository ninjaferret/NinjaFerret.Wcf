using System;
using NinjaFerret.Wcf.Exception;
using NinjaFerret.Wcf.Sample.BankManager.Interface.Exception;

namespace NinjaFerret.Wcf.Sample.BankManager.Interface.Faults
{
    [Serializable]
    public class AccountDoesNotExistFault : Fault
    {
        public string AccountNumber { get; set; }

        public override System.Exception ToException()
        {
            return new AccountDoesNotExistException(AccountNumber);
        }
    }
}