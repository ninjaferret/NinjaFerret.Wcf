using System.ServiceModel;
using NinjaFerret.Wcf.Sample.BankManager.Interface.Faults;
using NinjaFerret.Wcf.Sample.BankManager.Interface.Model;

namespace NinjaFerret.Wcf.Sample.BankManager.Interface
{
    [ServiceContract]
    public interface ITransactionService
    {
        [OperationContract]
        void Deposit(BankAccount account, decimal amount, string description);

        [OperationContract]
        [FaultContract(typeof(InsufficientFundsFault))]
        void Withdraw(BankAccount account, decimal amount, string description);

        [OperationContract]
        [FaultContract(typeof(InsufficientFundsFault))]
        void Transfer(BankAccount creditorAccount, BankAccount debtorAccount, decimal amount, string description);
    }
}