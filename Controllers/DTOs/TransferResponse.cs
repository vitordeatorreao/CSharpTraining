using System.Runtime.Serialization;

namespace BankApi.Controllers;

[DataContract]
public sealed class TransferResponse
{
    [DataMember(Name = "sourceAccount")]
    public BankAccountResponse? SourceAccount { get; set; }
    [DataMember(Name = "targetAccount")]
    public BankAccountResponse? TargetAccount { get; set; }
}
