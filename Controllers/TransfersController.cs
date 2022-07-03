using Microsoft.AspNetCore.Mvc;
using BankApi.Models;

namespace BankApi.Controllers;

[ApiController]
[Route("api/transfers")]
public class TransfersController : ControllerBase
{
    private readonly ILogger<AccountsController> _logger;
    private readonly IAccountDALAsync _accountDAL;

    public TransfersController(ILogger<AccountsController> logger, IAccountDALAsync accountDAL)
    {
        _logger = logger;
        _accountDAL = accountDAL;
    }

    [HttpPost]
    public async Task<ActionResult<TransferResponse>> TransferFunds(TransferRequest request)
    {
        if (request.Amount <= 0)
        {
            return BadRequest();
        }
        var sourceAccount = await _accountDAL.ReadById(request.SourceAccountId);
        if (sourceAccount == null)
        {
            return NotFound();
        }
        var targetAccount = await _accountDAL.ReadById(request.TargetAccountId);
        if (targetAccount == null)
        {
            return NotFound();
        }
        if (sourceAccount.Balance < request.Amount && !sourceAccount.CanGoNegative) {
            return BadRequest();
        }
        sourceAccount.Balance -= request.Amount;
        targetAccount.Balance += request.Amount;
        var newSourceAcc = await _accountDAL.Update(sourceAccount);
        var newTargetAcc = await _accountDAL.Update(targetAccount);
        return new TransferResponse
        {
            SourceAccount = newSourceAcc.ToResponse(),
            TargetAccount = newTargetAcc.ToResponse(),
        };
    }
}