namespace Zz.Services;

using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Zz.App.Core;
using Zz.Configs;
using MLK = MailKit;
using MMK = MimeKit;

public class EmailSender : IEmailSender
{
    private readonly SmtpEmailConfigs _emailConfigs;
    private readonly ILogger _logger;

    public EmailSender(SmtpEmailConfigs emailConfigs, ILogger<EmailSender> logger)
    {
        _emailConfigs = emailConfigs;
        _logger = logger;
    }

    public async Task SendEmailAsync(
        IEnumerable<IEmailSender.Recipient> contacts,
        string subject,
        IEmailSender.BodyContent? body = null,
        IEnumerable<IEmailSender.Attachment>? attachments = null,
        IEmailSender.EmailContact? sender = null
    )
    {
#if DEBUG
        _logger.LogDebug("Preparing Message...");
#endif

        var message = new MMK.MimeMessage();
        message.From.Add(new MMK.MailboxAddress(_emailConfigs.SenderName, _emailConfigs.Email));

        foreach (var recipient in contacts)
        {
            if (recipient.As == IEmailSender.ReceiveAsOption.Main)
                message.To.Add(
                    new MMK.MailboxAddress(recipient.Contact.Name, recipient.Contact.Email)
                );
            else if (recipient.As == IEmailSender.ReceiveAsOption.CC)
                message.Cc.Add(
                    new MMK.MailboxAddress(recipient.Contact.Name, recipient.Contact.Email)
                );
            else if (recipient.As == IEmailSender.ReceiveAsOption.BCC)
                message.Bcc.Add(
                    new MMK.MailboxAddress(recipient.Contact.Name, recipient.Contact.Email)
                );
        }

        ArgumentException.ThrowIfNullOrWhiteSpace(subject);
        message.Subject = subject;

        var bb = new MMK.BodyBuilder();

        if (body is not null)
        {
            if (body.Type == IEmailSender.BodyContentType.Html)
                bb.HtmlBody = body.Content;
            else if (body.Type == IEmailSender.BodyContentType.Text)
                bb.TextBody = body.Content;
        }

        if (attachments is not null)
            foreach (var attachment in attachments)
            {
                bb.Attachments.Add(
                    new MMK.MimePart(attachment.MediaType, attachment.MediaSubType)
                    {
                        Content = new MMK.MimeContent(attachment.Data),
                        IsAttachment = true,
                        FileName = attachment.FileName,
                    }
                );
            }

        message.Body = bb.ToMessageBody();

        using var client = new MLK.Net.Smtp.SmtpClient();

#if DEBUG
        _logger.LogDebug("Preparing and Connecting Client...");
#endif

        await client.ConnectAsync(
            _emailConfigs.SmtpUrl,
            _emailConfigs.Port ?? 0,
            _emailConfigs.SecurityProtocolType switch
            {
                SecurityProtocolType.TLS => MLK.Security.SecureSocketOptions.StartTls,
                SecurityProtocolType.SSL => MLK.Security.SecureSocketOptions.SslOnConnect,
                _ => MLK.Security.SecureSocketOptions.StartTlsWhenAvailable,
            }
        );

#if DEBUG
        _logger.LogDebug("Authenticating Client...");
#endif

        await client.AuthenticateAsync(_emailConfigs.Email, _emailConfigs.Password);

#if DEBUG
        _logger.LogDebug("Sending Email...");
#endif

        await client.SendAsync(message);

#if DEBUG
        _logger.LogDebug("Email Sent Successfully.");
#endif
    }
}
