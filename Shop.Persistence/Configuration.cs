using Microsoft.Extensions.Configuration;

namespace Shop.Persistence
{
   public class Configuration
    {

        public static IConfiguration config;
        public static void Initialize(IConfiguration Configuration)
        {
            config = Configuration;
        }

    }
}
