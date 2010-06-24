using System.Runtime.Serialization;

namespace NinjaFerret.Wcf.Sample.BankManager.Interface.Model
{
    [DataContract]
    public class Transaction
    {
        [DataMember]
        public string AccountNumber { get; set; }

        [DataMember]
        public decimal Amount { get; set; }

        [DataMember]
        public string Description { get; set; }
    }
}