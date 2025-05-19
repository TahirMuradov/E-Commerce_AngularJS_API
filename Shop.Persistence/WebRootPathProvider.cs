
namespace Shop.Persistence
{
    public static class WebRootPathProvider
    {
        public static string GetwwwrootPath
        {
            get
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                return path;

            }
        }
    }
}
