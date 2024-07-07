using Exchange.Services.Authorization.Data.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Exchange.ExchangeAccount.Controllers;
[ApiController]
[Route("/api/auth")]
public class AuthController : Controller
{
    private readonly Services.Authorization.Infrastructure.IAuthorizationService _authorizationService;
    private readonly ILogger<AuthController> _logger;
    public AuthController(Services.Authorization.Infrastructure.IAuthorizationService authorizationService, ILogger<AuthController> logger) =>
        (_authorizationService, _logger) = (authorizationService, logger);
    /// <summary>
    /// Регистрация нового пользователя с выдачей access и refresh токенов
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [Route("signUp")]
    [AllowAnonymous]
    [ProducesResponseType(type:typeof(SignUpDTO), statusCode: StatusCodes.Status200OK)]
    [ProducesResponseType(type: typeof(string), statusCode: StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> SignUp([FromBody] SignUpDTO signUpDTO)
    {
        try
        {
            return Ok(await _authorizationService.SignUpAsync(signUpDTO));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status403Forbidden, ex.Message);
        }
    }

    [HttpPost]
    [Route("signIn")]
    [AllowAnonymous]
    [ProducesResponseType(type: typeof(SignInDTO), statusCode: StatusCodes.Status200OK)]
    [ProducesResponseType(type: typeof(string), statusCode: StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> SignIn([FromBody] SignInDTO signInDTO)
    {
        try
        {
            return Ok(await _authorizationService.SignInAsync(signInDTO));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status403Forbidden, ex.Message);
        }
    }

    [HttpDelete]
    [Route("logout")]
    [Authorize]
    public async Task<IActionResult> Logout(string refreshToken)
    {
        try
        {
            return Ok(await _authorizationService.Logout(refreshToken));
        }
        catch(Exception ex) 
        {
            _logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status403Forbidden, ex.Message);
        }
    }

    [HttpPost]
    [Route("refresh")]
    [AllowAnonymous]
    public async Task<IActionResult> RefreshAccess(string refreshToken)
    {
        return Ok(await _authorizationService.GetAccessTokenAsync(refreshToken));
    }
}
