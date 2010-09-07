using System;
using NinjaFerret.Wcf.Exception;
using NinjaFerret.Wcf.Sample.BankManager.Interface.Exception;

namespace NinjaFerret.Wcf.Sample.BankManager.Interface.Faults
{
    [Serializable]
    public class InsufficientFundsFault : TranslatableFault
    {
        public string AccountNumber { get; set; }
        public decimal Amount { get; set; }

        public override System.Exception ToException()
        {
            return new InsufficientFundsException(AccountNumber, Amount);
        }
    }
}