namespace Aristino.SendMail
{
    public class MailRequest
    {
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string MailBody { get; set; }
        public List<IFormFile>? Attachment { get; set; }
    }
}
