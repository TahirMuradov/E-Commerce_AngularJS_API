

using Shop.Application.ResultTypes.Abstract;

namespace Shop.Application.Abstraction.Services
{
   public interface IMailService
    {
        public Task<IResult> SendEmailAsync(string LangCode,string userEmail, string confirmationLink, string UserName, bool isForgotPass);
        public Task<IResult> SendEmailPdfAsync(string LangCode,string userEmail, string UserName, string pdfLink);
    }
}
