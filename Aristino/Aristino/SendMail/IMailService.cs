namespace Aristino.SendMail
{
    public interface IMailService
    {
        Task SendMailAsync(MailRequest mailrequest);
    }
}
