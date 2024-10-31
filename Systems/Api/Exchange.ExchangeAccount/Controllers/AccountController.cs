using AccountServiceProto;
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
    private readonly IAccountService _accountService;
    private readonly ILogger<AccountController> _logger;
    private readonly ApiKeySettings _settings;
    public AccountController(IAccountService accountService, ILogger<AccountController> logger, ApiKeySettings settings) 
        => (_accountService,_logger,_settings) = (accountService,logger,settings);

    [HttpPost]
    [Route("/api/new")]
    public async Task<IActionResult> CreateAccount(AccountDto request)
    {
        if (!Request.Headers.TryGetValue("secret", out var key)) return Unauthorized("Header not found.");
        
        var result = await _accountService.AddAccountAsync(request);
        _logger.LogInformation($"Add new account: {result.Data}");
        return Ok(new AccountResponse()
        {
            Success = result.Data,
            ErrorMessage = result.ErrorMessage,
        });
    }
    
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
    [Route("/api/account/upload-image")]
    public async Task<IActionResult> UploadImage([FromQuery]string id,[FromForm] IFormFile file)
    {
        if (file.Length == 0)
        {
            _logger.LogError("File is null or empty");
            return BadRequest("File is missing or empty");
        }
        
        _logger.LogInformation($"Upload image: {file.FileName}");
        _logger.LogInformation($"Upload type: {file.ContentType}");
        _logger.LogInformation($"Id: {id}");
        return Ok(await _accountService.UploadImageAsync(id, file));
    }
    
    [HttpPost]
    [Authorize]
    [Route("add-volute")]
    public async Task<IActionResult> AddFavoriteVolute(string id, [FromBody] FavoriteDto favoriteDto)
    {
        try
        {
            return Ok(await _accountService.AddFavoriteVoluteAsync(Guid.Parse(id),favoriteDto));
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
    public async Task<IActionResult> DeleteFavoriteVolute(string id, [FromBody] FavoriteDto favoriteDto)
    {
        try
        {
            return Ok(await _accountService.DeleteFavoriteVoluteAsync(Guid.Parse(id), favoriteDto));
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
        if (!Request.Headers.TryGetValue("secret", out var key)) return Unauthorized("Header not found.");
        
        if (key.Equals(_settings.XAPIKEY))
        {
            return Ok(await _accountService.GetAccountsAcceptedAsync());
        }
        
        return Unauthorized("Header not found.");
    }
}
