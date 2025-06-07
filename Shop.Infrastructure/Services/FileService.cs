using iText.Html2pdf;

using Shop.Application.Abstraction.Services;
using Shop.Application.DTOs.OrderPdfGeneratorDTOs;
using Shop.Application.ResultTypes.Abstract;
using Shop.Application.ResultTypes.Concrete.ErrorResults;
using Shop.Application.ResultTypes.Concrete.SuccessResults;
using Shop.Persistence;
using System.Net;
using System.Text;
using System.Web;

namespace Shop.Infrastructure.Services
{
    public class FileService : IFileService
    {
        public IResult RemoveFile(string FilePaths)
        {
            string filePath = Path.Combine(WebRootPathProvider.GetwwwrootPath + FilePaths);

            if (File.Exists(filePath))
            
                File.Delete(filePath);            
            else return new ErrorResult(HttpStatusCode.NotFound);
            return new SuccessResult(HttpStatusCode.OK);
        }

        public IResult RemoveFileRange(List<string> FilePaths)
        {
            foreach (var path in FilePaths)
            {
         var result= RemoveFile(path);

                if (!result.IsSuccess)
                    return result;

            }
            return new SuccessResult(HttpStatusCode.OK);
        }

        public async Task<IDataResult<string>> SaveImageAsync(Microsoft.AspNetCore.Http.IFormFile image, bool isProductImage = false)
        {

            string filePath = string.Empty;
            filePath = isProductImage ? Path.Combine(WebRootPathProvider.GetwwwrootPath, "uploads", "ProductPictures") :
                Path.Combine(WebRootPathProvider.GetwwwrootPath, "uploads", "WebUIPictures");

            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            var path = isProductImage ? "/uploads/ProductPictures/" + Guid.NewGuid().ToString() + image.FileName.Replace(' ', '_') :
        "/uploads/WebUIPictures/" + Guid.NewGuid().ToString() + image.FileName.Replace(' ', '_');
      
            using FileStream fileStream = new(Path.Combine(WebRootPathProvider.GetwwwrootPath + path), FileMode.Create);
            await image.CopyToAsync(fileStream);
            return new SuccessDataResult<string>(data:path,HttpStatusCode.OK);
        }
        /// <summary>
        /// /// Saves a list of files to the specified WebUIPictures folder in web root path.
        /// 
        /// </summary>
        /// <param name="file">File is photo or other file type</param>
        /// <param name="WebRootPath"> WebRootPath is wwwroot folder`s path</param>
        /// <returns></returns>
        public async Task<IDataResult<List<string>>> SaveImageRangeAsync(Microsoft.AspNetCore.Http.IFormFileCollection images, bool isProductImages = false)
        {
            SuccessDataResult< List<string>> result = new SuccessDataResult<List<string>>(new List<string> (),HttpStatusCode.OK);

            foreach (var image in images)
            {
                IDataResult<string> path = await SaveImageAsync(image,isProductImages);
                if (result.IsSuccess)
                {
                    
                result.Data.Add(path.Data);
                }
            }
            return result;
        }
        /// <summary>
        /// /// Saves an order PDF file with the specified items, shipping method, and payment method.
        /// return a list of strings containing the file path and a unique identifier of order pdf.
        /// list index 0 is file path and index 1 is unique identifier of order pdf.
        /// </summary>
        /// <param name="items">Products</param>
        /// <param name="shippingMethod">Shipping method</param>
        /// <param name="paymentMethod"> Payment Method</param>
        /// <returns></returns>
        public IDataResult<List<string>> SaveOrderPdf(List<OrderProductDTO> items, OrderShippingMethodDTO shippingMethod, OrderPaymentMethodDTO paymentMethod, OrderUserInfoDTO userInfoDTO)
        {
            decimal totalPrice = items.Sum(x => x.Price);
            var tableProductBuilder = new StringBuilder();
            var guid = Guid.NewGuid();
            var shortGuid = guid.ToString().Substring(0, 6);

            foreach (var item in items)
            {
                tableProductBuilder.AppendLine($@"
<tr>
    <td style='border: 1px solid #ebebeb; padding: 10px;'>{HttpUtility.HtmlEncode(item.ProductCode)}</td>
    <td style='border: 1px solid #ebebeb; padding: 10px;'>{HttpUtility.HtmlEncode(item.ProductName)}</td>   
    <td style='border: 1px solid #ebebeb; padding: 10px;'>{HttpUtility.HtmlEncode(item.ProductName)}</td>
    <td style='border: 1px solid #ebebeb; padding: 10px;'>{HttpUtility.HtmlEncode(item.size)}</td>
    <td style='border: 1px solid #ebebeb; padding: 10px;'>{item.Quantity}</td>
    <td style='border: 1px solid #ebebeb; padding: 10px;'>{item.Price} &#x20BC;</td>
</tr>");
            }

            totalPrice += shippingMethod.Price;

            var htmlContent = $@"
<!DOCTYPE html>
<html lang='az'>
<head>
    <meta charset='UTF-8'>
    <title>Sifariş Detalları</title>

</head>
<body>
   <body style=' margin: 0;
        padding: 2rem;
        font-size: 12px;
        line-height: 1.4;
        color: #000;
         box-sizing: border-box;
        font-family: Arial, sans-serif;
            '>
    <div style='width: 100%;
        max-width: 21cm;
        margin: 0 auto;
        padding: 20px;
        border: 1px solid #ccc;'>
      <div style='width: 100%; border-collapse: collapse;margin-bottom: 20px;'>
        <div style='width: 100%; border-collapse: collapse;margin-bottom: 20px;'>
        <p>
          <h5 style='margin: 0;
        font-size: 16px;'>Elektron Çek</h5>
          <p>№123456</p>
          <span>Tarix: 12.12.25</span>
        </p>
        <p style='text-align:center;font-weight: bold;'>
         KARL FASHION
         <hr>
          Tel: +994552784344
        </p>
      </div>

      <div style='margin-bottom: 20px;'>
        <table style='width: 100%;
        border-collapse: collapse;
        word-break: break-word;
        table-layout: fixed;'>
          <thead>
            <tr>
              <th style='width: 15%'>Məhsul Kodu</th>
              <th style='width: 35%'>Məhsul adı</th>
              <th style='width: 35%'>Məhsul Kateqoriyası</th>
              <th style='width: 10%'>Ölçü</th>
              <th style='width: 10%'>Say</th>
              <th style='width: 15%'>Qiymət</th>
            </tr>
          </thead>
       
            <tbody>
                {tableProductBuilder.ToString()}
    <tr>
         
              <td style='text-align: left;border: 1px solid #ccc;
        padding: 8px;
        text-align: left;
        vertical-align: top;
        overflow-wrap: break-word;
        word-wrap: break-word;
        word-break: break-word;' colspan=""5"">Çatdırılma -{shippingMethod.ShippingContent}</td>
              <td style='
              font-weight: bold;
        text-align: right;border: 1px solid #ccc;
        padding: 8px;
        vertical-align: top;
        overflow-wrap: break-word;
        word-wrap: break-word;
        word-break: break-word;'>{shippingMethod.Price} AZN</td>
            </tr>
            <tr>
              
              <td style='text-align: left;border: 1px solid #ccc;
        padding: 8px;
        text-align: left;
        vertical-align: top;
        overflow-wrap: break-word;
        word-wrap: break-word;
        word-break: break-word;' colspan='5'>Cəmi</td>
              <td style='border: 1px solid #ccc;
        padding: 8px;
        text-align: left;
        vertical-align: top;
        overflow-wrap: break-word;
        word-wrap: break-word;
        word-break: break-word;
              font-weight: bold;
        text-align: right;'>{totalPrice} AZN</td>
            </tr>
            </body>
        </table>
      </div>

      <div>
        <label style='font-weight: bold;
        font-size: 14px;
        display: block;
        margin: 15px 0 10px;'>Müştəri Məlumatları</label>
        <table style='  width: 100%;
        border-collapse: collapse;
        word-break: break-word;
        table-layout: fixed;'>
          <tbody>

     <tr>
              <th style='background-color: #f0f0f0;
        font-weight: bold;   border: 1px solid #ccc;
        padding: 8px;
        text-align: left;
        vertical-align: top;
        overflow-wrap: break-word;
        word-wrap: break-word;
        word-break: break-word;'>Ad Soyad</th>
              <td style=' border: 1px solid #ccc;
        padding: 8px;
        text-align: left;
        vertical-align: top;
        overflow-wrap: break-word;
        word-wrap: break-word;
        word-break: break-word;'>{HttpUtility.HtmlEncode(userInfoDTO.FullName)}</td>
            </tr>
            <tr>
              <th style='background-color: #f0f0f0;
        font-weight: bold;   border: 1px solid #ccc;
        padding: 8px;
        text-align: left;
        vertical-align: top;
        overflow-wrap: break-word;
        word-wrap: break-word;
        word-break: break-word;'>Əlaqə nömrəsi</th>
              <td style=' border: 1px solid #ccc;
        padding: 8px;
        text-align: left;
        vertical-align: top;
        overflow-wrap: break-word;
        word-wrap: break-word;
        word-break: break-word;'>{HttpUtility.HtmlEncode(userInfoDTO.PhoneNumber)}</td>
            </tr>
            <tr>
              <th style='background-color: #f0f0f0;
        font-weight: bold;   border: 1px solid #ccc;
        padding: 8px;
        text-align: left;
        vertical-align: top;
        overflow-wrap: break-word;
        word-wrap: break-word;
        word-break: break-word;'>Ünvan</th>
              <td style=' border: 1px solid #ccc;
        padding: 8px;
        text-align: left;
        vertical-align: top;
        overflow-wrap: break-word;
        word-wrap: break-word;
        word-break: break-word;'>
{HttpUtility.HtmlEncode(userInfoDTO.Address)}
              </td>
            </tr>
            <tr>
              <th style='background-color: #f0f0f0;
        font-weight: bold;   border: 1px solid #ccc;
        padding: 8px;
        text-align: left;
        vertical-align: top;
        overflow-wrap: break-word;
        word-wrap: break-word;
        word-break: break-word;'>Qeyd</th>
              <td style=' border: 1px solid #ccc;
        padding: 8px;
        text-align: left;
        vertical-align: top;
        overflow-wrap: break-word;
        word-wrap: break-word;
        word-break: break-word;'>
           {HttpUtility.HtmlEncode(userInfoDTO.Note)}
              </td>
            </tr>

</tbody>
        </table>
      </div>
    </div>
  </body>
</html>";

            string folderPath = Path.Combine(WebRootPathProvider.GetwwwrootPath, "uploads", "OrderPDFs");
            Directory.CreateDirectory(folderPath);
            string htmlPath = Path.Combine(folderPath, $"{shortGuid}.html");
            string pdfPath = Path.Combine(folderPath, $"{shortGuid}.pdf");

            File.WriteAllText(htmlPath, htmlContent, Encoding.UTF8);

            using (FileStream htmlSource = File.Open(htmlPath, FileMode.Open))
            using (FileStream pdfDest = File.Open(pdfPath, FileMode.Create))
            {
                var converterProperties = new ConverterProperties();
                converterProperties.SetCharset("UTF-8");
                HtmlConverter.ConvertToPdf(htmlSource, pdfDest, converterProperties);
            }

            File.Delete(htmlPath);

            var result = new SuccessDataResult<List<string>>(new List<string>
    {
        $"\\uploads\\OrderPDFs\\{shortGuid}.pdf",
        shortGuid
    }, HttpStatusCode.OK);

            return result;
        }

    }
}
