using Shop.Application.Abstraction.Services;
using Shop.Application.ResultTypes.Abstract;
using Shop.Application.ResultTypes.Concrete.ErrorResults;
using Shop.Application.ResultTypes.Concrete.SuccessResults;
using MailKit.Net.Smtp;
using System.Net;
using MimeKit;
using Microsoft.Extensions.Configuration;

namespace Shop.Infrastructure.Services
{
    public class MailServce : IMailService
    {
        private readonly IConfiguration _configuration;

        public MailServce(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<IResult> SendEmailAsync(string LangCode,string userEmail, string confirmationLink, string UserName,bool isForgotPass)
        {

            try
            {
                string htmlBody2 = $"<!DOCTYPE html>\r\n <html lang=\"en\">\r\n    <head>\r\n    <!-- If you delete this tag, the sky will fall on your head -->\r\n        <meta charset=\"utf-8\">\r\n        <meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\">\r\n        <meta name=\"viewport\" content=\"width=device-width, initial-scale=1\">\r\n        <!-- The above 3 meta tags *must* come first in the head; any other head content must come *after* these tags -->\r\n        <title>Email Sample</title>\r\n        <link rel=\"stylesheet\" href=\"https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.5.1/css/all.min.css\" integrity=\"sha512-DTOQO9RWCH3ppGqcWaEA1BIZOC6xxalwEsw9c2QQeAIftl+Vegovlnee1c9QX4TctnWMn13TZye+giMm8e2LwA==\" crossorigin=\"anonymous\" referrerpolicy=\"no-referrer\" />\r\n        <link rel=\"stylesheet\" href=\"https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css\">\r\n        <!-- Bootstrap -->\r\n        <link rel=\"stylesheet\" href=\"https://cdnjs.cloudflare.com/ajax/libs/bootstrap/5.3.2/css/bootstrap.min.css\" integrity=\"sha512-b2QcS5SsA8tZodcDtGRELiGv5SaKSk1vDHDaQRda0htPYWZ6046lr3kJ5bAAQdpV2mmA/4v0wQF9MyU6/pDIAg==\" crossorigin=\"anonymous\" referrerpolicy=\"no-referrer\" />\r\n        <link rel=\"stylesheet\" href=\"cailon.css\" />\r\n        <link rel=\"stylesheet\" href=\"css/robotonewfont.css\" />\r\n        <!--Text-->\r\n        <link href=\"https://fonts.googleapis.com/css?family=Lobster\" rel=\"stylesheet\" type=\"text/css\">\r\n\r\n    \r\n\r\n    </head>\r\n\r\n    <body style=\"background-color: rgb(136,189,191)\">\r\n        <div class=\"form\" style=\"text-align: center; background-color: rgb(243, 243, 243);padding: 40px; max-width: 600px;margin: 30px auto;\">\r\n            <div>\r\n                <img style=\"width: 40%;border-radius: 50%;\" class=\"\" alt=\"logo\" src=\"https://thumbs.dreamstime.com/b/istanbul-silhouette-tulip-traditional-touristic-shape-tourism-72962774.jpg\">           \r\n            </div>\r\n        \r\n            <div class=\"form-small\"; style=\"padding: 40px;background-color: white;max-width: 600px;margin: 10px auto;\">\r\n\r\n                <h1 class=\"h1-font\" style=\"font-family: cursive;text-align: center;\">Email Confirmation<h1>\r\n                <p class=\"p-font\" style=\"margin: 20px 10px 20px;font-size: 20px; font-family: 'Times New Roman', Times, serif;text-align: center;\">Hey Tahir, you're almost ready to start enjoying <b>Elephantry</b>.  \r\n                Simply click the BIG yellow button below to verify your email address.</p>\r\n                <div class=\"wrapper\" style=\"padding: 20px 100px 40px;\">\r\n                    <a class=\"button\"  style=\" \r\n                    font-family: Montserrat, Roboto, Helvetica, Arial, sans-serif; \r\n                    font-size: 1.75rem;\r\n                    min-width: 8.23em;\r\n                    text-decoration: none;\r\n                    position: relative;\r\n                     border-radius: 1em; \r\n                     line-height: 1; \r\n                     padding: 1.18em 1.32em 1.03em;\r\n                      font-weight: bold; color:rgb(243, 243, 243) ;\r\n                       display: inline-block;\r\n                        text-align: center;\r\n                         background-color: #FFB200;\r\n                         \"  href='{confirmationLink}'>Verify email address</a>\r\n                </div>\r\n\r\n                <!-- Filter: https://css-tricks.com/gooey-effect/ -->\r\n                          \r\n            </div>\r\n\r\n            <div>\r\n                <h3 class=\"contact-font\">Stay in touch<h3>\r\n\r\n                <div class=\"social-buttons\"\r\n             \r\n                >\r\n                    <a class=\"facebook\" target=\"_blank\" style=\"text-decoration: none;\" href=\"https://www.facebook.com/istanbulayaqqabi\">\r\n                        <i style=\"color: #FFB200;\" class=\"fa fa-facebook\">\r\n                        </i>\r\n                    </a>\r\n                    \r\n                    <a class=\"whatsapp\" target=\"_blank\" style=\"text-decoration: none; margin: 0 20px;\" href=\"https://api.whatsapp.com/send/?phone=994556735478&text&type=phone_number&app_absent=0\">\r\n                        <i style=\"color: #FFB200;\" class=\"fa fa-whatsapp\">\r\n                        </i>\r\n                    </a>\r\n                    \r\n                    <a class=\"instagram\" target=\"_blank\" style=\"text-decoration: none;\" href=\"https://www.instagram.com/ayaqqabi_istanbul/\">\r\n                        <i style=\"color: #FFB200;\" class=\"fa fa-instagram\">\r\n                        </i>\r\n                    </a>\r\n                </div>\r\n\r\n                <p class=\"p-footer\" style=\"\r\n                margin: 20px 10px 20px;\r\n                padding: 10px 50px 0px;\r\n                  font-size: 11px;  \r\n                  font-family: Arial, Helvetica, sans-serif;\r\n                  text-align: center;\">Email sent by TM Company <br>\r\n                Copyright © {DateTime.UtcNow.Year} </p>\r\n            </div>\r\n        </div>\r\n\r\n        <!-- jQuery (necessary for Bootstrap's JavaScript plugins) -->\r\n        <script src=\"https://ajax.googleapis.com/ajax/libs/jquery/1.12.4/jquery.min.js\"></script>\r\n        <!-- Include all compiled plugins (below), or include individual files as needed -->\r\n        <script src=\"https://cdnjs.cloudflare.com/ajax/libs/bootstrap/5.3.2/js/bootstrap.min.js\" integrity=\"sha512-WW8/jxkELe2CAiE4LvQfwm1rajOS8PHasCCx+knHG0gBHt8EXxS6T6tJRTGuDQVnluuAvMxWF4j8SNFDKceLFg==\" crossorigin=\"anonymous\" referrerpolicy=\"no-referrer\"></script>\r\n    </body>\r\n</html>";
                var email = new MimeMessage();
                email.From.Add(new MailboxAddress(_configuration["EmailServices:FromName"], _configuration["EmailServices:FromEmail"]));
                email.To.Add(new MailboxAddress(UserName, userEmail));
                email.Importance = MessageImportance.High;
                email.Subject = isForgotPass ?
                    "Forgot Password" :
                    "Email Confirmation";
           

                email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = htmlBody2 };

                using (var smtp = new SmtpClient())
                {

                    await smtp.ConnectAsync(_configuration["EmailServices:ServiceName"], Convert.ToInt32(_configuration["EmailServices:ServicePort"]), MailKit.Security.SecureSocketOptions.StartTls);
                    await smtp.AuthenticateAsync(_configuration["EmailServices:FromEmail"], _configuration["EmailServices:FromEmailPassword"]);//mail gonderen emailin sifre parolu
                    await smtp.SendAsync(email);
                    await smtp.DisconnectAsync(true);
                }
                return new SuccessResult(LangCode,statusCode: HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return new ErrorResult(LangCode,message: ex.Message, statusCode: HttpStatusCode.BadRequest);
            }
        }

        public async Task<IResult> SendEmailPdfAsync(string LangCode,string userEmail, string UserName, string pdfLink)
        {
            try
            {
                string htmlBody2 = "Sizin Elktron Çekiniz";
                var email = new MimeMessage();
                email.From.Add(new MailboxAddress(_configuration["EmailServices:FromName"], _configuration["EmailServices:FromEmail"]));
                email.To.Add(new MailboxAddress(UserName, userEmail));
                email.Importance = MessageImportance.High;
                email.Subject = "Elektron Çek";

                var attachment = new MimePart("application", "pdf")
                {
                    Content = new MimeContent(File.OpenRead(pdfLink)),
                    ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                    ContentTransferEncoding = ContentEncoding.Base64,
                    FileName = Path.GetFileName(pdfLink)
                };

                var multipart = new Multipart("mixed");
                multipart.Add(attachment);

                email.Body = multipart;
                using (var smtp = new SmtpClient())
                {

                    await smtp.ConnectAsync(_configuration["EmailServices:ServiceName"], Convert.ToInt32(_configuration["EmailServices:ServicePort"]), MailKit.Security.SecureSocketOptions.StartTls);
                    await smtp.AuthenticateAsync(_configuration["EmailServices:FromEmail"], _configuration["EmailServices:FromEmailPassword"]);
                    await smtp.SendAsync(email);
                    await smtp.DisconnectAsync(true);
                }
                return new SuccessResult(LangCode,statusCode: HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return new ErrorResult(LangCode,message: ex.Message, statusCode: HttpStatusCode.BadRequest);
            }
        }
    }
}
