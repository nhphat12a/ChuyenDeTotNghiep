using Aristino.SendMail;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Aristino.Areas.AristinoAdmin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Mail : ControllerBase
    {
        private readonly IMailService _mailService;
        public Mail(IMailService mailService)
        {
            _mailService = mailService;
        }
        [HttpPost("send")]
        public async Task<IActionResult> SendMail([FromForm]MailRequest mailRequest)
        {
            try
            {
                await _mailService.SendMailAsync(mailRequest);
                return Ok();
            }
            catch(Exception ex)
            {
                throw;
            }
        }
    }
}
