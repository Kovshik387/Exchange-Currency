using Castle.Core.Logging;
using Exchange.Services.EmailAction.Data;
using Exchange.Services.MessageSendler.Data.DTO;
using Exchange.Services.MessageSendler.Infrastructure;
using Exchange.Services.Settings.SettingsConfigure;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Mail;

namespace Exchange.Services.MessageSendler.Services;

public class MessageSendler : IMessageSendler
{
    private readonly SmtpClient _smtpClient;
    private readonly EmailSettings _emailSettings;
    private readonly ILogger<MessageSendler> _logger;
    public MessageSendler(EmailSettings emailSettings, ILogger<MessageSendler> logger)
    {
        this._emailSettings = emailSettings;
        _smtpClient = new SmtpClient(_emailSettings.Provider,_emailSettings.Port);
        _smtpClient.Credentials = new NetworkCredential(_emailSettings.Email, _emailSettings.Password);
        _smtpClient.EnableSsl = true;
        _logger = logger;
    }

    public async Task SendNotificationAsync(EmailModel accountVoluteDTO)
    {
        _logger.LogInformation($"Отправка сообщения: {accountVoluteDTO.AccountModel.Email}");
        MailAddress sender = new MailAddress(_emailSettings.Email, $"Информация о валютах за {accountVoluteDTO.Date}");

        string body = $"<div>" +
            $"<h1>{accountVoluteDTO.Date}</h1>" +
            $"<p>Котировки</p>" +
            $"<ul>";
        foreach (var item in accountVoluteDTO.VoluteModel)
        {
            body += $"<li>{item.Name} : {item.Value}</li>";
        }
        body += "</ul>";
        MailAddress receiver = new MailAddress(accountVoluteDTO.AccountModel.Email);

        MailMessage message = new MailMessage(sender, receiver) 
        {
            Subject = "Информация о валютах",
            Body = body,
            IsBodyHtml = true,
        };

        try
        {
            await _smtpClient.SendMailAsync(message);
            _logger.LogInformation("Сообщение успешно отправлено.");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Ошибка при отправке сообщения: {ex.Message}");
        }
    }
}
