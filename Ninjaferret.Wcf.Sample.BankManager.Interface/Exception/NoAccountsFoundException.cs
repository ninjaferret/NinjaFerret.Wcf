using System;
using Ninjaferret.Wcf.Sample.BankManager.Interface.Model;

namespace Ninjaferret.Wcf.Sample.BankManager.Interface.Exception
{
    public class NoAccountsFoundException : ApplicationException
    {
        public AccountType AccountType { get; private set;}
        public string CustomerName { get; private set; }

        public NoAccountsFoundException(string customerName, AccountType accountType)
        {
            AccountType = accountType;
            CustomerName = customerName;
        }
    }
}