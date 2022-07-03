using System.Runtime.Serialization;

namespace BankApi.Controllers;

[DataContract]
public sealed class BankAccountRequest
{
    [DataMember(Name = "name")]
    public string? Name { get; set; }
    [DataMember(Name = "ownerName")]
    public string? OwnerName { get; set; }
    [DataMember(Name = "canGoNegative")]
    public bool? CanGoNegative { get; set; }
}