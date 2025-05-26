using FluentValidation.Resources;
using Microsoft.Extensions.Configuration;
using Shop.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.CustomLanguageMessage
{
   public class CustomValidationLanguageMessage:LanguageManager
    {
        public CustomValidationLanguageMessage()
        {
            string[] supportedLaunguages = Configuration.config.GetSection("SupportedLanguage:Launguages").Get<string[]>();
            string languagesList = string.Join(", ", supportedLaunguages);
            #region CategoryValidationMessages
            #region CategoryAddValidationMessages


            AddTranslation("az", "ContentEmpty", "Məzmun boş ola bilməz!");
            AddTranslation("az", "InvalidLangCode", $"Dil Kodları yalnız {languagesList} ola bilər!");
            AddTranslation("az", "LangContentTooShort", $"Məzmun siyahısının uzunluğu {supportedLaunguages.Length}-dən az ola bilməz!");

            AddTranslation("ru", "LangContentTooShort", $"Длина списка содержимого не может быть меньше {supportedLaunguages.Length}!");
            AddTranslation("ru", "InvalidLangCode", $"Коды языков могут быть только {languagesList}!");
            AddTranslation("ru", "ContentEmpty", "Содержимое не может быть пустым!");

            AddTranslation("en", "LangContentTooShort", $"The length of the content list cannot be less than {supportedLaunguages.Length}!");
            AddTranslation("en", "InvalidLangCode", $"Language codes can only be {languagesList}!");
            AddTranslation("en", "ContentEmpty", "Content cannot be empty!");

            #endregion
            #region CategoryUpdateValidationMessages

            AddTranslation("az", "IdIsRequired", "ID boş ola bilməz!");
            AddTranslation("ru", "IdIsRequired", "ID не может быть пустым!");
            AddTranslation("en", "IdIsRequired", "ID is required!");

            AddTranslation("az", "InvalidGuid", "ID etibarlı bir GUID olmalıdır!");
            AddTranslation("ru", "InvalidGuid", "ID должен быть действительным GUID!");
            AddTranslation("en", "InvalidGuid", "ID must be a valid GUID!");


            AddTranslation("az", "LangDictionaryIsRequired", "Məzmun boş ola bilməz!");
            AddTranslation("az", "LangKeyAndValueRequired", "Hər bir Dil Kodu və Məzmunu dolu olmalıdır!");
            AddTranslation("az", "InvalidLangKey", $"Dil Kodlari yalnız {languagesList} ola bilər!");

            AddTranslation("ru", "LangDictionaryIsRequired", "Содержимое не может быть пустым!");
            AddTranslation("ru", "LangKeyAndValueRequired", "Каждый код языка и содержимое должны быть заполнены!");
            AddTranslation("ru", "InvalidLangKey", $"Коды языков могут быть только {languagesList}!");

            AddTranslation("en", "LangDictionaryIsRequired", "Content cannot be empty!");
            AddTranslation("en", "LangKeyAndValueRequired", "Each language code and content must be filled!");
            AddTranslation("en", "InvalidLangKey", $"Language codes can only be {languagesList}!");


            #endregion
            #endregion
            #region SizeValidationMessages
            AddTranslation("az", "NewSizeNumberIsRequired", "Yeni ölçü boş ola bilməz!!");
            AddTranslation("az", "SizeNumberIsRequiredd", "Ölçü boş ola bilməz!!");
            AddTranslation("ru", "NewSizeNumberIsRequired", "Новый размер не может быть пустым!!");
            AddTranslation("en", "NewSizeNumberIsRequired", "New size cannot be empty!!");

            AddTranslation("ru", "SizeNumberIsRequired", "Размер не может быть пустым!!");
            AddTranslation("en", "SizeNumberIsRequired", "Size cannot be empty!!");

            #endregion

            #region SubCategoryValidationMessages
            #region AddSubCategoryValidationMessages
            // Azerbaijani
            AddTranslation("az", "CategoryIdInvalid", "Kateqoriya  boş  ola bilməz!");




            // Russian
            AddTranslation("ru", "CategoryIdInvalid", "Категория не может быть пустой!");





            // English
            AddTranslation("en", "CategoryIdInvalid", "Category cannot be empty!");




            #endregion
            #region UpdateSubCategoryValidationMessages
            AddTranslation("az", "SubCategoryIdInvalid", "Alt Kateqoriya məzmunu boş  ola bilməz!");
            AddTranslation("az", "LangSubCategoryContentTooShort", "Alt Kateqoriya id-si boş  ola bilməz!");

            AddTranslation("ru", "SubCategoryIdInvalid", "Содержимое подкатегории не может быть пустым!");
            AddTranslation("ru", "LangSubCategoryContentTooShort", "ID подкатегории не может быть пустым!");

            AddTranslation("en", "SubCategoryIdInvalid", "Subcategory content cannot be empty!");
            AddTranslation("en", "LangSubCategoryContentTooShort", "Subcategory ID cannot be empty!");

            #endregion
            #endregion
            #region ShippingMethodValidationMessages

            AddTranslation("az", "DisCountNegativeNumberCheck", "Endirimli Qiymet  0-dan kiçik ola bilməz!");
            AddTranslation("az", "PriceNegativeNumberCheck", "Qiymət  0-a bərabər və ya kiçik ola bilməz!");

            AddTranslation("en", "DisCountNegativeNumberCheck", "Discount price cannot be less than 0!");
            AddTranslation("en", "PriceNegativeNumberCheck", "Price cannot be equal to or less than 0!");

            AddTranslation("ru", "DisCountNegativeNumberCheck", "Цена со скидкой не может быть меньше 0!");
            AddTranslation("ru", "PriceNegativeNumberCheck", "Цена не может быть равна 0 или меньше 0!");



            #endregion

            #region ProductDtoValidation

            AddTranslation("az", "DisCountChecked", "Endirimli Qiymət 0-dan kiçik ola bilməz!");
            AddTranslation("az", "PriceChecked", " Qiymət 0-dan böyük olamalıdır!");
            AddTranslation("az", "PictureİsRequired", "Məhsulun şəklini əlavə edin!");
            AddTranslation("az", "SubCategoryİsRequired", "Məhsulun Kateqoriyasını əlavə edin!");
            AddTranslation("az", "ProductCodeİsRequired", "Məhsulun Kodu əlavə edin!");

            AddTranslation("ru", "DisCountChecked", "Цена со скидкой не может быть меньше 0!");
            AddTranslation("ru", "PriceChecked", "Цена должна быть больше 0!");
            AddTranslation("ru", "PictureİsRequired", "Добавьте изображение продукта!");
            AddTranslation("ru", "SubCategoryİsRequired", "Добавьте категорию продукта!");
            AddTranslation("ru", "ProductCodeİsRequired", "Добавьте код продукта!");

            AddTranslation("en", "DisCountChecked", "Discounted price cannot be less than 0!");
            AddTranslation("en", "PriceChecked", "Price must be greater than 0!");
            AddTranslation("en", "PictureİsRequired", "Please add a product image!");
            AddTranslation("en", "SubCategoryİsRequired", "Please add a product category!");
            AddTranslation("en", "ProductCodeİsRequired", "Please add a product code!");
            //update dto validation message
            // Azerbaijani
            AddTranslation("az", "IdRequired", "Id boş olmamalıdır!");
            AddTranslation("az", "IdInvalid", "Id düzgün deyil!");
            AddTranslation("az", "ProductNameRequired", "Məhsul adı boş olmamalıdır!");
            AddTranslation("az", "DescriptionRequired", "Məhsul açıqlaması boş olmamalıdır!");
            AddTranslation("az", "SubCategoriesIDRequired", "Kateqoriya boş olmamalıdır!");
            AddTranslation("az", "SubCategoriesIDLength", "Minimum 1 dənə kateqoriya olmalıdır!");
            AddTranslation("az", "PictureUrlsOrNewPicturesRequired", "Məhsul şəkli əlavə edilməlidir!");
            AddTranslation("az", "SizesRequired", "Ölçü boş olmamalıdır!");
            AddTranslation("az", "DiscountPriceInvalid", "Endirimli Qiymət 0-dan kiçik ola bilməz!");
            AddTranslation("az", "PriceInvalid", "Qiymət 0-dan böyük olmalıdır!");
            AddTranslation("az", "ProductCodeRequired", "Məhsul kodu boş olmamalıdır!");

            // Russian
            AddTranslation("ru", "IdRequired", "Id не может быть пустым!");
            AddTranslation("ru", "IdInvalid", "Id некорректен!");
            AddTranslation("ru", "ProductNameRequired", "Название продукта не может быть пустым!");
            AddTranslation("ru", "DescriptionRequired", "Описание продукта не может быть пустым!");
            AddTranslation("ru", "SubCategoriesIDRequired", "Категория не может быть пустой!");
            AddTranslation("ru", "SubCategoriesIDLength", "Должна быть хотя бы одна категория!");
            AddTranslation("ru", "PictureUrlsOrNewPicturesRequired", "Необходимо добавить изображение продукта!");
            AddTranslation("ru", "SizesRequired", "Размеры не могут быть пустыми!");
            AddTranslation("ru", "DiscountPriceInvalid", "Скидочная цена не может быть меньше 0!");
            AddTranslation("ru", "PriceInvalid", "Цена должна быть больше 0!");
            AddTranslation("ru", "ProductCodeRequired", "Код продукта не может быть пустым!");


            // English
            AddTranslation("en", "IdRequired", "Id cannot be null or empty!");
            AddTranslation("en", "IdInvalid", "Id cannot be invalid!");
            AddTranslation("en", "ProductNameRequired", "Product name cannot be null or empty!");
            AddTranslation("en", "DescriptionRequired", "Product description cannot be null or empty!");
            AddTranslation("en", "SubCategoriesIDRequired", "Subcategories cannot be null or empty!");
            AddTranslation("en", "SubCategoriesIDLength", "At least one subcategory is required!");
            AddTranslation("en", "PictureUrlsOrNewPicturesRequired", "Product picture must be provided!");
            AddTranslation("en", "SizesRequired", "Sizes cannot be null or empty!");
            AddTranslation("en", "DiscountPriceInvalid", "Discount price cannot be less than 0!");
            AddTranslation("en", "PriceInvalid", "Price must be greater than 0!");
            AddTranslation("en", "ProductCodeRequired", "Product code cannot be null or empty!");




            #endregion
            #region PictureValidationsMessage
            AddTranslation("az", "ProductIdRequired", "Məhsul ID tələb olunur.");
            AddTranslation("ru", "ProductIdRequired", "Требуется идентификатор продукта.");
            AddTranslation("en", "ProductIdRequired", "Product ID is required.");

            AddTranslation("az", "PicturesRequired", "Ən azı bir şəkil tələb olunur.");
            AddTranslation("ru", "PicturesRequired", "Требуется хотя бы одно изображение.");
            AddTranslation("en", "PicturesRequired", "At least one picture is required.");

            AddTranslation("az", "EmptyFileNotAllowed", "Boş fayllara icazə verilmir.");
            AddTranslation("ru", "EmptyFileNotAllowed", "Пустые файлы недопустимы.");
            AddTranslation("en", "EmptyFileNotAllowed", "Empty files are not allowed.");

            AddTranslation("az", "InvalidImageFormat", "Yalnız JPEG və PNG formatlarına icazə verilir.");
            AddTranslation("ru", "InvalidImageFormat", "Допускаются только форматы JPEG и PNG.");
            AddTranslation("en", "InvalidImageFormat", "Only JPEG and PNG formats are allowed.");
            #endregion
            #region AuthDtOsValidationMessages
            AddTranslation("az", "FirstnameRequired", "Ad tələb olunur!");
            AddTranslation("ru", "FirstnameRequired", "Имя обязательно!");
            AddTranslation("en", "FirstnameRequired", "Firstname is required!");

            AddTranslation("az", "LastnameRequired", "Soyad tələb olunur!");
            AddTranslation("ru", "LastnameRequired", "Фамилия обязательна!");
            AddTranslation("en", "LastnameRequired", "Lastname is required!");

            AddTranslation("az", "EmailRequired", "Email tələb olunur!");
            AddTranslation("ru", "EmailRequired", "Email обязателен!");
            AddTranslation("en", "EmailRequired", "Email is required!");

            AddTranslation("az", "EmailInvalid", "Email yanlışdır!");
            AddTranslation("ru", "EmailInvalid", "Email недействителен!");
            AddTranslation("en", "EmailInvalid", "Email is invalid!");

            AddTranslation("az", "PhoneNumberRequired", "Telefon nömrəsi tələb olunur!");
            AddTranslation("ru", "PhoneNumberRequired", "Номер телефона обязателен!");
            AddTranslation("en", "PhoneNumberRequired", "Phone number is required!");

            AddTranslation("az", "PhoneNumberInvalid", "Telefon nömrəsi yanlışdır!");
            AddTranslation("ru", "PhoneNumberInvalid", "Номер телефона недействителен!");
            AddTranslation("en", "PhoneNumberInvalid", "Phone number is invalid!");

            AddTranslation("az", "AddressRequired", "Ünvan tələb olunur!");
            AddTranslation("ru", "AddressRequired", "Адрес обязателен!");
            AddTranslation("en", "AddressRequired", "Address is required!");

            AddTranslation("az", "UsernameRequired", "İstifadəçi adı tələb olunur!");
            AddTranslation("ru", "UsernameRequired", "Имя пользователя обязательно!");
            AddTranslation("en", "UsernameRequired", "Username is required!");

            AddTranslation("az", "PasswordRequired", "Şifrə tələb olunur!");
            AddTranslation("ru", "PasswordRequired", "Пароль обязателен!");
            AddTranslation("en", "PasswordRequired", "Password is required!");

            AddTranslation("az", "ConfirmPasswordRequired", "Şifrə təsdiqi tələb olunur!");
            AddTranslation("ru", "ConfirmPasswordRequired", "Подтверждение пароля обязательно!");
            AddTranslation("en", "ConfirmPasswordRequired", "Confirm password is required!");

            AddTranslation("az", "PasswordsDoNotMatch", "Şifrələr uyğun deyil!");
            AddTranslation("ru", "PasswordsDoNotMatch", "Пароли не совпадают!");
            AddTranslation("en", "PasswordsDoNotMatch", "Passwords do not match!");

            AddTranslation("az", "UserIdRequired", "İstifadəçi ID-si tələb olunur!");
            AddTranslation("ru", "UserIdRequired", "ID пользователя обязательно!");
            AddTranslation("en", "UserIdRequired", "User ID is required!");

            AddTranslation("az", "UserIdInvalid", "Yanlış istifadəçi ID-si!");
            AddTranslation("ru", "UserIdInvalid", "Неверный ID пользователя!");
            AddTranslation("en", "UserIdInvalid", "Invalid User ID!");

            AddTranslation("az", "RoleIdRequired", "Rol ID-si tələb olunur!");
            AddTranslation("ru", "RoleIdRequired", "ID роли обязательно!");
            AddTranslation("en", "RoleIdRequired", "Role ID is required!");

            AddTranslation("az", "RoleIdInvalid", "Yanlış rol ID-si!");
            AddTranslation("ru", "RoleIdInvalid", "Неверный ID роли!");
            AddTranslation("en", "RoleIdInvalid", "Invalid Role ID!");

            AddTranslation("az", "UserIdRequired", "İstifadəçi ID-si tələb olunur!");
            AddTranslation("ru", "UserIdRequired", "ID пользователя обязательно!");
            AddTranslation("en", "UserIdRequired", "User ID is required!");

            AddTranslation("az", "UserIdInvalid", "Yanlış istifadəçi ID-si!");
            AddTranslation("ru", "UserIdInvalid", "Неверный ID пользователя!");
            AddTranslation("en", "UserIdInvalid", "Invalid User ID!");

            AddTranslation("az", "RoleIdRequired", "Rol ID-si tələb olunur!");
            AddTranslation("ru", "RoleIdRequired", "ID роли обязательно!");
            AddTranslation("en", "RoleIdRequired", "Role ID is required!");

            AddTranslation("az", "RoleIdInvalid", "Yanlış rol ID-si!");
            AddTranslation("ru", "RoleIdInvalid", "Неверный ID роли!");
            AddTranslation("en", "RoleIdInvalid", "Invalid Role ID!");

            AddTranslation("az", "EmailRequired", "E-poçt tələb olunur!");
            AddTranslation("ru", "EmailRequired", "Электронная почта обязательна!");
            AddTranslation("en", "EmailRequired", "Email is required!");

            AddTranslation("az", "EmailInvalid", "Yanlış e-poçt formatı!");
            AddTranslation("ru", "EmailInvalid", "Неверный формат электронной почты!");
            AddTranslation("en", "EmailInvalid", "Invalid email format!");

            AddTranslation("az", "PasswordRequired", "Şifrə tələb olunur!");
            AddTranslation("ru", "PasswordRequired", "Пароль обязателен!");
            AddTranslation("en", "PasswordRequired", "Password is required!");



            #endregion
            AddTranslation("az", "SubCategoryAndCategoryIdIsrequired", "Kateqoriya və ya Alt Kateqoriyadan 1-i seçilməlidir!");
            AddTranslation("en", "SubCategoryAndCategoryIdIsrequired", "One of Category or SubCategory must be selected!");
            AddTranslation("ru", "SubCategoryAndCategoryIdIsrequired", "Необходимо выбрать либо категорию, либо подкатегорию!");

            #region CuponValidations

            AddTranslation("az", "CategoryIdrequired", "Kateqoriya boş ola bilməz!");
            AddTranslation("az", "SubCategoryIdrequired", "Alt Kateqoriya boş ola bilməz!");
            AddTranslation("az", "ProductIdrequired", "Məhsul boş ola bilməz!");
            AddTranslation("az", "UserIdrequired", "İstifadəçi boş ola bilməz!");
            AddTranslation("az", "DisCountPrecentBeetwen", "Endirim Faizi 0-100 aralığında olmalıdır!");

            // Russian
            AddTranslation("ru", "CategoryIdrequired", "Категория не может быть пустой!");
            AddTranslation("ru", "SubCategoryIdrequired", "Подкатегория не может быть пустой!");
            AddTranslation("ru", "ProductIdrequired", "Продукт не может быть пустым!");
            AddTranslation("ru", "UserIdrequired", "Пользователь не может быть пустым!");
            AddTranslation("ru", "DisCountPrecentBeetwen", "Скидка должна быть в диапазоне от 0 до 100!");

            // English
            AddTranslation("en", "CategoryIdrequired", "Category cannot be empty!");
            AddTranslation("en", "SubCategoryIdrequired", "Subcategory cannot be empty!");
            AddTranslation("en", "ProductIdrequired", "Product cannot be empty!");
            AddTranslation("en", "UserIdrequired", "User cannot be empty!");
            AddTranslation("en", "DisCountPrecentBeetwen", "Discount percentage must be between 0 and 100!");


            //update dto
            AddTranslation("az", "CuponIdrequired", "Cupon Id boş ola bilməz!");
            AddTranslation("az", "RealetedIdrequired", "Kateqoriya ,Alt Kateqoriya,Məhsul,Istifadəçi hər hansisa biri seçilməlidir!");
            AddTranslation("ru", "CuponIdrequired", "ID купона не может быть пустым!");
            AddTranslation("ru", "RealetedIdrequired", "Должен быть выбран хотя бы один: категория, подкатегория, продукт или пользователь!");
            AddTranslation("en", "CuponIdrequired", "Cupon ID cannot be empty!");
            AddTranslation("en", "RealetedIdrequired", "At least one must be selected: category, subcategory, product, or user!");

            #endregion
            AddTranslation("az", "TokenRequired", "Token boş ola bilməz!");
            AddTranslation("en", "TokenRequired", "Token cannot be empty!");
            AddTranslation("ru", "TokenRequired", "Токен не может быть пустым!");
        }
    }
}
