using System.Runtime.Serialization;

namespace NinjaFerret.Wcf.Sample.BankManager.Interface.Model
{
    [DataContract]
    public enum AccountType
    {
        [EnumMember]
        Current, 
        [EnumMember]
        Savings
    }
}