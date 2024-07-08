using Exchange.Services.EmailAction.Data;

namespace Exchange.Services.MessageSendler.Infrastructure;

public interface IMessageSendler
{
    public Task SendNotificationAsync(EmailModel accountVoluteDTO);
}
