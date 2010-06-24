using System.Collections.Generic;
using System.ServiceModel;
using NinjaFerret.Wcf.Sample.BankManager.Interface.Faults;
using NinjaFerret.Wcf.Sample.BankManager.Interface.Model;

namespace NinjaFerret.Wcf.Sample.BankManager.Interface
{
    [ServiceContract]
    public interface IAccountService
    {
        [OperationContract]
        [FaultContract(typeof(AccountDoesNotExistFault))]
        BankAccount Get(string accountNumber);

        [OperationContract]
        [FaultContract(typeof(NoAccountsFoundFault))]
        IEnumerable<BankAccount> Find(string customerName, AccountType accountType);

        [OperationContract]
        void Create(string customerName, AccountType accountType);
    }
}