using System.Runtime.Serialization;

namespace BankApi.Controllers;

[DataContract]
public sealed class BankAccountResponse
{
    [DataMember(Name = "id")]
    public Guid Id { get; set; }
    [DataMember(Name = "name")]
    public string? Name { get; set ; }
    [DataMember(Name = "ownerName")]
    public string? OwnerName { get; set; }
    [DataMember(Name = "balance")]
    public decimal Balance { get; set; }
    [DataMember(Name = "openedAt")]
    public DateTime OpenedAt { get; set; }
    [DataMember(Name = "canGoNegative")]
    public bool CanGoNegative { get; set; }
}
