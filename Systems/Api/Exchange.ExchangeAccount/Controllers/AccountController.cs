using Exchange.Services.Account.Data.DTO;
using Exchange.Services.Account.Infrastructure;
using Exchange.Services.Settings.SettingsConfigure;
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
    private readonly ApiKeySettings _settings;
    public AccountController(IAccountService accountService, ILogger<AuthController> logger, ApiKeySettings settings) 
        => (_accountService,_logger,_settings) = (accountService,logger,settings);

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
        if (Request.Headers.TryGetValue("secret", out var key))
        {
            _logger.LogInformation($"{_settings.XAPIKEY}");
            _logger.LogInformation($"{key}");
            if (key.Equals(_settings.XAPIKEY))
            {
                return Ok(await _accountService.GetAccountsAcceptedAsync());
            }
        }
        return Unauthorized("Header not found.");
    }
}
