using Exchange.Services.EmailAction.Data;

namespace Exchange.Services.EmailAction.Infrastructure;

public interface IEmailAction
{
    public Task PublicateAccount(EmailModel model);
}
