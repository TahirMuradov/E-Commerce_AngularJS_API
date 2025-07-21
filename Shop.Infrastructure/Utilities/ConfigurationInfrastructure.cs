

using Microsoft.Extensions.Configuration;

namespace Shop.Infrastructure.Utilities
{
    public static class ConfigurationInfrastructure
    {
       

        static public string[] SupportedLaunguageKeys
        {
            get
            {
                if (config.GetSection("SupportedLanguage:Launguages").Get<string[]>() is null)
                {

                    ConfigurationManager configurationManager = new();
                  

                    return configurationManager.GetSection("SupportedLanguage:Launguages").Get<string[]>();

                }

                return config.GetSection("SupportedLanguage:Launguages").Get<string[]>();

            }
        }
        static public string DefaultLanguageKey
        {
            get
            {
                if (string.IsNullOrEmpty(config.GetConnectionString("SupportedLanguage:Default")))
                {

                    ConfigurationManager configurationManager = new();
            
                    return configurationManager.GetSection("SupportedLanguage:Default").Get<string>();



                }

                return config.GetSection("SupportedLanguage:Default").Get<string>();
            }
        }

        public static IConfiguration config;
        public static void Initialize(IConfiguration Configuration)
        {
            config = Configuration;
        }
    }
}
