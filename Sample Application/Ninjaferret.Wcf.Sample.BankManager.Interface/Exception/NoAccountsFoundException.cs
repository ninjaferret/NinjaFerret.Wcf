using System;
using NinjaFerret.Wcf.Exception;
using NinjaFerret.Wcf.Sample.BankManager.Interface.Faults;
using NinjaFerret.Wcf.Sample.BankManager.Interface.Model;

namespace NinjaFerret.Wcf.Sample.BankManager.Interface.Exception
{
    public class NoAccountsFoundException : ApplicationException, IException
    {
        public AccountType AccountType { get; private set;}
        public string CustomerName { get; private set; }

        public NoAccountsFoundException(string customerName, AccountType accountType)
        {
            AccountType = accountType;
            CustomerName = customerName;
        }

        public Fault ToFault()
        {
            return new NoAccountsFoundFault {AccountType = AccountType, CustomerName = CustomerName};
        }
    }
}