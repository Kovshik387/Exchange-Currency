using Exchange.Services.Account.Data.DTO;
using Exchange.Services.Account.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Exchange.ExchangeAccount.Controllers;
[ApiController]
[Route("/api/account")]
public class AccountController : Controller
{
    //test
    private readonly IAccountService _accountService;
    private readonly ILogger<AuthController> _logger;
    public AccountController(IAccountService accountService, ILogger<AuthController> logger) => (_accountService,_logger) = (accountService,logger);

    [HttpGet]
    [Route("/api/account/{id}")]
    public async Task<IActionResult> GetAccountDetailsById(string id)
    {
        try
        {
            return Ok(await _accountService.GetAccountByIdAsync(Guid.Parse(id)));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    [Authorize]
    [Route("add-volute")]
    public async Task<IActionResult> AddFavoriteVolute(string id, [FromBody] FavoriteDTO favoriteDTO)
    {
        try
        {
            return Ok(await _accountService.AddFavoriteVoluteAsync(Guid.Parse(id),favoriteDTO));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete]
    [Authorize]
    [Route("delete-volute")]
    public async Task<IActionResult> DeleteFavoriteVolute(string id, [FromBody] FavoriteDTO favoriteDTO)
    {
        try
        {
            return Ok(await _accountService.DeleteFavoriteVoluteAsync(Guid.Parse(id), favoriteDTO));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return BadRequest(ex.Message);
        }
    }

    [HttpPatch]
    [Authorize]
    [Route("change-forward")]
    public async Task<IActionResult> ChangeForwardingAsync(string id)
    {
        try
        {
            return Ok(await _accountService.ChangeForwardingAsync(Guid.Parse(id)));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    [Route("/api/accounts")]
    public async Task<IActionResult> GetAccounts()
    {
        return Ok(await _accountService.GetAccountsAcceptedAsync());
    }
}
