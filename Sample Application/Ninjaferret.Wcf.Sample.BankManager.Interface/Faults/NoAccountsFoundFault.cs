using System;
using NinjaFerret.Wcf.Exception;
using NinjaFerret.Wcf.Sample.BankManager.Interface.Exception;
using NinjaFerret.Wcf.Sample.BankManager.Interface.Model;

namespace NinjaFerret.Wcf.Sample.BankManager.Interface.Faults
{
    [Serializable]
    public class NoAccountsFoundFault : Fault
    {
        public AccountType AccountType { get; set; }
        public string CustomerName { get; set; }

        public override System.Exception ToException()
        {
            return new NoAccountsFoundException(CustomerName, AccountType);
        }
    }
}