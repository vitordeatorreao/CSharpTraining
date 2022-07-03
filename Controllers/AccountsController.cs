using Microsoft.AspNetCore.Mvc;
using BankApi.Models;

namespace BankApi.Controllers;

[ApiController]
[Route("api/accounts")]
public class AccountsController : ControllerBase
{
    private readonly ILogger<AccountsController> _logger;
    private readonly IAccountDALAsync _accountDAO;

    public AccountsController(ILogger<AccountsController> logger, IAccountDALAsync accountDAO)
    {
        _logger = logger;
        _accountDAO = accountDAO;
    }

    [HttpPost]
    public ActionResult<BankAccountResponse> Create(BankAccountRequest request)
    {
        throw new NotImplementedException();
    }

    [HttpGet("{id}")]
    public ActionResult<BankAccountResponse> Read(Guid id)
    {
        throw new NotImplementedException();
    }

    [HttpPut]
    public ActionResult<BankAccountResponse> Update(BankAccountUpdateRequest request)
    {
        throw new NotImplementedException();
    }

    [HttpDelete("{id}")]
    public ActionResult<BankAccountResponse> Delete(Guid id)
    {
        throw new NotImplementedException();
    }

    [HttpGet]
    public ActionResult<IEnumerable<BankAccountResponse>> List()
    {
        throw new NotImplementedException();
    }
}
