// using Exchange.Services.Authorization.Data.DTO;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;
//
// namespace Exchange.ExchangeAccount.Controllers;
// [ApiController]
// [Route("/api/auth")]
// public class AuthController : Controller
// {
//     private readonly Exchange.Services.Authorization.Infrastructure.IAuthorizationService _authorizationService;
//     private readonly ILogger<AuthController> _logger;
//     // ReSharper disable once ConvertToPrimaryConstructor
//     public AuthController(Exchange.Services.Authorization.Infrastructure.IAuthorizationService authorizationService, ILogger<AuthController> logger) =>
//         (_authorizationService, _logger) = (authorizationService, logger);
//     /// <summary>
//     /// Регистрация нового пользователя с выдачей access и refresh токенов
//     /// </summary>
//     /// <returns></returns>
//     [HttpPost]
//     [Route("signUp")]
//     [AllowAnonymous]
//     [ProducesResponseType(type:typeof(SignUpDto), statusCode: StatusCodes.Status200OK)]
//     [ProducesResponseType(type: typeof(string), statusCode: StatusCodes.Status403Forbidden)]
//     public async Task<IActionResult> SignUp([FromBody] SignUpDto signUpDto)
//     {
//         try
//         {
//             return Ok(await _authorizationService.SignUpAsync(signUpDto));
//         }
//         catch (Exception ex)
//         {
//             _logger.LogError(ex.Message);
//             return StatusCode(StatusCodes.Status403Forbidden, ex.Message);
//         }
//     }
//     /// <summary>
//     /// Аутентификация и идентификация пользователя
//     /// </summary>
//     /// <param name="signInDto"></param>
//     /// <returns></returns>
//     [HttpPost]
//     [Route("signIn")]
//     [AllowAnonymous]
//     [ProducesResponseType(type: typeof(SignInDto), statusCode: StatusCodes.Status200OK)]
//     [ProducesResponseType(type: typeof(string), statusCode: StatusCodes.Status403Forbidden)]
//     public async Task<IActionResult> SignIn([FromBody] SignInDto signInDto)
//     {
//         try
//         {
//             return Ok(await _authorizationService.SignInAsync(signInDto));
//         }
//         catch (Exception ex)
//         {
//             _logger.LogError(ex.Message);
//             return StatusCode(StatusCodes.Status403Forbidden, ex.Message);
//         }
//     }
//     /// <summary>
//     /// Удаление refreshToken'а у пользователя, выход из аккаунта на устройстве
//     /// </summary>
//     /// <param name="refreshToken"></param>
//     /// <returns></returns>
//     [HttpDelete]
//     [Route("logout")]
//     [Authorize]
//     public async Task<IActionResult> Logout(string refreshToken)
//     {
//         try
//         {
//             return Ok(await _authorizationService.Logout(refreshToken));
//         }
//         catch(Exception ex) 
//         {
//             _logger.LogError(ex.Message);
//             return StatusCode(StatusCodes.Status403Forbidden, ex.Message);
//         }
//     }
//     /// <summary>
//     /// Выдача нового accessTokne'а на основе refreshToken'а 
//     /// </summary>
//     /// <param name="refreshToken"></param>
//     /// <returns></returns>
//     [HttpPost]
//     [Route("refresh")]
//     [AllowAnonymous]
//     public async Task<IActionResult> RefreshAccess(string refreshToken)
//     {
//         return Ok(await _authorizationService.GetAccessTokenAsync(refreshToken));
//     }
// }
