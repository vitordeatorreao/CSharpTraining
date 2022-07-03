using Microsoft.AspNetCore.Mvc;
using BankApi.Models;

namespace BankApi.Controllers;

[ApiController]
[Route("api/accounts")]
public class AccountsController : ControllerBase
{
    private readonly ILogger<AccountsController> _logger;
    private readonly IAccountDALAsync _accountDAL;

    public AccountsController(ILogger<AccountsController> logger, IAccountDALAsync accountDAL)
    {
        _logger = logger;
        _accountDAL = accountDAL;
    }

    [HttpPost]
    public async Task<ActionResult<BankAccountResponse>> Create(BankAccountRequest request)
    {
        var validationError = ValidateRequest(request);
        if (validationError != null) return validationError;
        var account = new Account
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            OwnerName = request.OwnerName,
            OpenedAt = DateTime.UtcNow,
            CanGoNegative = request.CanGoNegative.HasValue ? request.CanGoNegative.Value : false,
            // No need to set Balance, as its defaul value is what we want.
        };
        var createdAccount = await _accountDAL.Create(account);
        return CreatedAtAction(
            // nameOf returns the name of a class, method, field or property as a string.
            // Using your IDE's rename feature will affect this string as well, so this can be
            // very useful. More in:
            // https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/nameof
            nameof(Read),
            new { id = account.Id },
            ToResponse(account));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BankAccountResponse>> Read(Guid id)
    {
        var account = await _accountDAL.ReadById(id);
        if (account == null) return NotFound();
        return ToResponse(account);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<BankAccountResponse>> Update(Guid id, BankAccountRequest request)
    {
        var account = await _accountDAL.ReadById(id);
        if (account == null) return NotFound();
        if (!string.IsNullOrWhiteSpace(request.Name)) account.Name = request.Name;
        if (!string.IsNullOrWhiteSpace(request.OwnerName)) account.OwnerName = request.OwnerName;
        var updatedAccount = await _accountDAL.Update(account);
        return ToResponse(updatedAccount);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<BankAccountResponse>> Delete(Guid id)
    {
        try
        {
            var deletedAccount = await _accountDAL.DeleteById(id);
            return ToResponse(deletedAccount);
        }
        catch (AccountNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpGet]
    public async IAsyncEnumerable<BankAccountResponse> List()
    {
        foreach (var account in await _accountDAL.List())
        {
            yield return ToResponse(account);
        }
    }

    private BankAccountResponse ToResponse(Account account)
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

    private ActionResult<BankAccountResponse>? ValidateRequest(BankAccountRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            // Ideally, include a body explaining what is invalid in the request
            return BadRequest();
        }
        if (string.IsNullOrWhiteSpace(request.OwnerName))
        {
            return BadRequest();
        }
        return default;
    }
}
