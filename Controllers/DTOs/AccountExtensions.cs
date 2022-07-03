using BankApi.Models;

namespace BankApi.Controllers;

public static class AccountExtensions
{
    public static BankAccountResponse ToResponse(this Account account)
    {
        return new BankAccountResponse
        {
            Id = account.Id,
            Name = account.Name,
            OwnerName = account.OwnerName,
            OpenedAt = account.OpenedAt,
            CanGoNegative = account.CanGoNegative,
            Balance = account.Balance,
        };        
    }
}