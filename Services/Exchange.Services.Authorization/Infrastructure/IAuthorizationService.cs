using Exchange.Services.Authorization.Data.DTO;

namespace Exchange.Services.Authorization.Infrastructure;

/// <summary>
/// Сервис бизнес-логики для авторизации и регистрации
/// </summary>
public interface IAuthorizationService
{
    /// <summary>
    /// Аутентификация в существующий аккаунт
    /// </summary>
    /// <param name="signIn"></param>
    /// <returns></returns>
    public Task<AuthResponse<AuthDto>> SignInAsync(SignInDto model);
    /// <summary>
    /// Регистрация нового пользователя
    /// </summary>
    /// <param name="signUp"></param>
    /// <returns></returns>
    public Task<AuthResponse<AuthDto>> SignUpAsync(SignUpDto model);
    /// <summary>
    /// Проверка на наличие пользователей в БД 
    /// </summary>
    /// <returns></returns>
    Task<bool> IsEmptyAsync();
    /// <summary>
    /// Выдача нового access-токена
    /// </summary>
    /// <param name="refreshToken"></param>
    /// <returns></returns>
    public Task<AuthResponse<AuthDto>> GetAccessTokenAsync(string refreshToken);
    /// <summary>
    /// Удаление refresh-токена у пользователя
    /// </summary>
    /// <param name="refreshToken"></param>
    /// <returns></returns>
    public Task<AuthResponse<object>> Logout(string refreshToken);
}
