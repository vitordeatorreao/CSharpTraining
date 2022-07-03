using System.Runtime.Serialization;

namespace BankApi.Controllers;

[DataContract]
public sealed class TransferRequest
{
    [DataMember(Name = "sourceAccountId")]
    public Guid SourceAccountId { get; set; }
    [DataMember(Name = "targetAccountId")]
    public Guid TargetAccountId { get; set; }
    [DataMember(Name = "amount")]
    public decimal Amount { get; set; }
}