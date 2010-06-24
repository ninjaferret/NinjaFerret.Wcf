using System.Runtime.Serialization;

namespace NinjaFerret.Wcf.Sample.BankManager.Interface.Model
{
    [DataContract]
    public class BankAccount
    {
        [DataMember]
        public string AccountNumber { get; set; }

        [DataMember]
        public string CustomerName { get; set; }

        [DataMember]
        public AccountType AccountType { get; set; }

        [DataMember]
        public decimal OverdraftLimit { get; set; }
    }
}