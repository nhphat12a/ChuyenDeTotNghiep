using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Aristino.SendMail
{
    public class MailService:IMailService
    {
        private readonly MailSettings _settings;
        private readonly IWebHostEnvironment _environment;
        public MailService(IOptions<MailSettings> settings, IWebHostEnvironment environment)
        {
            _settings = settings.Value;
            _environment = environment;
        }
        public async Task SendMailAsync(MailRequest mailrequest)
        {
            //string FilePath = Path.Combine(_environment.WebRootPath, @"MailTemplate\index.html");
            //StreamReader str=new StreamReader(FilePath);
            //string MailText=str.ReadToEnd();
            //str.Close();
            //MailText = MailText.Replace("[username]", mailrequest.ToEmail).Replace("[email]", mailrequest.MailBody);
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_settings.Mail);
            email.To.Add(MailboxAddress.Parse(mailrequest.ToEmail));
            email.Subject=mailrequest.Subject;
            var builder = new BodyBuilder();
           // builder.HtmlBody = MailText;
            if(mailrequest.Attachment!= null)
            {
                byte[] Filebytes;
                foreach(var file in mailrequest.Attachment)
                {
                    if(file.Length > 0)
                    {
                        using(var ms=new MemoryStream())
                        {
                            file.CopyTo(ms);
                            Filebytes=ms.ToArray();
                        }
                        builder.Attachments.Add(file.FileName, Filebytes, ContentType.Parse(file.ContentType));
                    }
                }
            }
            builder.HtmlBody = mailrequest.MailBody;
            email.Body=builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_settings.Host,_settings.Port,SecureSocketOptions.StartTls);
            smtp.Authenticate(_settings.Mail, "izkrpcyubvfblznf");
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
    }
}
