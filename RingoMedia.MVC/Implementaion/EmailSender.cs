using Hangfire.Server;
using Microsoft.Extensions.Options;
using MimeKit;
using NETCore.MailKit;
using RingoMedia.MVC.Interface;
using RingoMedia.MVC.Models.DataBase;
using RingoMedia.MVC.Models.Models;
namespace RingoMedia.MVC.Implementaion;

public class EmailSender(IMailKitProvider _mailKitProvider, IOptions<Urls> _urls, IWebHostEnvironment _hostEnvironment, ILogs _logs) : IEmailSender
{
    public async Task<bool> SendEmailAsync(Reminder reminder, PerformContext context)
    {
        string fullPathHtml = Path.Combine(_hostEnvironment.WebRootPath, "email-template", "EmailTemplate.html");
        string emailHtmlBody = File.ReadAllText(fullPathHtml).Replace("{{Title}}", reminder.Title)
                             .Replace("{{DateTime}}", reminder.SendingDateTime.ToString("g"));

        //Log Email

        MimeMessage mimeMessage = new();

        mimeMessage.To.Add(new MailboxAddress(reminder.Email, reminder.Email));

        mimeMessage.Subject = reminder.Title;
        var builder = new BodyBuilder { HtmlBody = emailHtmlBody };

        //Add Attachment If needed
        mimeMessage.Body = builder.ToMessageBody();
        EmailLog emailLog = await _logs.AddEmailLog(new EmailLog { Email = reminder.Email, SendingDate = reminder.SendingDateTime, HangfireJobId = context.BackgroundJob.Id });
        await _logs.AddEmailLog(emailLog);
        await SendAsync(mimeMessage, emailLog);
        return true;
    }

    private async Task SendAsync(MimeMessage message, EmailLog emailLog)
    {
        message.From.Add(new MailboxAddress(_mailKitProvider.Options.SenderName, _mailKitProvider.Options.SenderEmail));
        using (var emailClient = _mailKitProvider.SmtpClient)
        {
            if (!emailClient.IsConnected)
            {
                await emailClient.AuthenticateAsync(_mailKitProvider.Options.Account,
                _mailKitProvider.Options.Password);
                await emailClient.ConnectAsync(_mailKitProvider.Options.Server,
                _mailKitProvider.Options.Port, _mailKitProvider.Options.Security);
            }
            await emailClient.SendAsync(message);
            await emailClient.DisconnectAsync(true);
            emailLog.Sent = true;
            await _logs.AddEmailLog(emailLog);
        }
    }
}