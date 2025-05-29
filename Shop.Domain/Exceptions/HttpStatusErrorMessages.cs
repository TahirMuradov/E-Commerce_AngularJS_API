namespace Shop.Domain.Exceptions
{
    public static class HttpStatusErrorMessages
    {
        public static readonly Dictionary<string, string> StockQuantityNotEnough = new()
{
    { "en", "The requested quantity exceeds available stock!" },
    { "ru", "Запрашиваемое количество превышает доступный остаток!" },
    { "az", "İstənilən say mövcud stokdan çoxdur!" }
};

        public static readonly Dictionary<string, string> SizeNotFound = new()
{
    { "en", "The selected size was not found!" },
    { "ru", "Выбранный размер не найден!" },
    { "az", "Seçilmiş ölçü tapılmadı!" }
};

        public static readonly Dictionary<string, string> PriceNotMatch = new()
{
    { "en", "Product price does not match the current price!" },
    { "ru", "Цена товара не соответствует текущей цене!" },
    { "az", "Məhsulun qiyməti cari qiymətlə uyğun gəlmir!" }
};

        public static readonly Dictionary<string, string> NotFoundProduct = new()
{
    { "en", "Product not found!" },
    { "ru", "Товар не найден!" },
    { "az", "Məhsul tapılmadı!" }
};

        public static readonly Dictionary<string, string> FileUploadFailed = new()
    {
        { "en", "An error occurred while uploading the file. Please try again!" },
        { "ru", "Произошла ошибка при загрузке файла. Попробуйте снова!" },
        { "az", "Fayl yüklənərkən xəta baş verdi. Yenidən cəhd edin!" }
    };
        public static readonly Dictionary<string, string> UnsupportedLanguage = new()
    {
        { "en", "One or more of the provided language codes are not supported." },
        { "ru", "Один или несколько предоставленных языковых кодов не поддерживаются." },
        { "az", "Verilən dil kodlarından biri və ya bir neçəsi dəstəklənmir." }
    };
        // 1xx Informational (Rarely used in APIs)
        public static Dictionary<string, string> Continue => new()
        {
            { "az", "Davam edin" },
            { "ru", "Продолжить" },
            { "en", "Continue" }
        };

        // 2xx Success (Including your existing success message)
        public static Dictionary<string, string> Success => new()
        {
            { "az", "Əməliyyat uğurla tamamlandı!" },
            { "ru", "Операция успешно завершена!" },
            { "en", "Operation completed successfully!" }
        };

        public static Dictionary<string, string> Created => new()
        {
            { "az", "Uğurla yaradıldı!" },
            { "ru", "Успешно создано!" },
            { "en", "Successfully created!" }
        };

        public static Dictionary<string, string> NoContent => new()
        {
            { "az", "Məzmun yoxdur!" },
            { "ru", "Нет содержимого!" },
            { "en", "No content!" }
        };

        // 3xx Redirection
        public static Dictionary<string, string> MovedPermanently => new()
        {
            { "az", "Daimi olaraq yönləndirildi!" },
            { "ru", "Перенаправлено навсегда!" },
            { "en", "Moved permanently!" }
        };

        public static Dictionary<string, string> TemporaryRedirect => new()
        {
            { "az", "Müvəqqəti yönləndirmə!" },
            { "ru", "Временное перенаправление!" },
            { "en", "Temporary redirect!" }
        };

        // 4xx Client Errors
        public static Dictionary<string, string> BadRequest => new()
        {
            { "az", "Yanlış sorğu!" },
            { "ru", "Неверный запрос!" },
            { "en", "Bad request!" }
        };

        public static Dictionary<string, string> Unauthorized => new()
        {
            { "az", "İcazəsiz giriş!" },
            { "ru", "Неавторизованный доступ!" },
            { "en", "Unauthorized access!" }
        };

        public static Dictionary<string, string> Forbidden => new()
        {
            { "az", "Giriş qadağandır!" },
            { "ru", "Доступ запрещен!" },
            { "en", "Forbidden!" }
        };

        public static Dictionary<string, string> NotFound => new()
        {
            { "az", "Tapılmadı!" },
            { "ru", "Не найдено!" },
            { "en", "Not found!" }
        };

        public static Dictionary<string, string> MethodNotAllowed => new()
        {
            { "az", "Metod icazə verilmir!" },
            { "ru", "Метод не разрешен!" },
            { "en", "Method not allowed!" }
        };

        public static Dictionary<string, string> Conflict => new()
        {
            { "az", "Münaqişə xətası!" },
            { "ru", "Конфликт данных!" },
            { "en", "Conflict error!" }
        };

        public static Dictionary<string, string> TooManyRequests => new()
        {
            { "az", "Çox sayda sorğu!" },
            { "ru", "Слишком много запросов!" },
            { "en", "Too many requests!" }
        };

        // 5xx Server Errors
        public static Dictionary<string, string> InternalServerError => new()
        {
            { "az", "Daxili server xətası!" },
            { "ru", "Внутренняя ошибка сервера!" },
            { "en", "Internal server error!" }
        };

        public static Dictionary<string, string> NotImplemented => new()
        {
            { "az", "Həyata keçirilməyib!" },
            { "ru", "Не реализовано!" },
            { "en", "Not implemented!" }
        };

        public static Dictionary<string, string> BadGateway => new()
        {
            { "az", "Yanlış şlüz!" },
            { "ru", "Ошибка шлюза!" },
            { "en", "Bad gateway!" }
        };

        public static Dictionary<string, string> ServiceUnavailable => new()
        {
            { "az", "Xidmət əlçatan deyil!" },
            { "ru", "Сервис недоступен!" },
            { "en", "Service unavailable!" }
        };

        public static Dictionary<string, string> GatewayTimeout => new()
        {
            { "az", "Şlüz zaman aşımı!" },
            { "ru", "Тайм-аут шлюза!" },
            { "en", "Gateway timeout!" }
        };
    }
}