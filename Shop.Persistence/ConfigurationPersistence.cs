using Microsoft.Extensions.Configuration;

namespace Shop.Persistence
{
    public class ConfigurationPersistence
    {
        static public string DefaultConnectionString
        {
            get
            {



                ConfigurationManager configurationManager = new();
                try
                {
                    configurationManager.SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../ShopAPI"));
                    configurationManager.AddJsonFile("appsettings.json");
                }
                catch
                {
                    configurationManager.AddJsonFile("appsettings.Production.json");
                }

                     ;

                return config.GetConnectionString("Default")
                ??
                configurationManager.GetConnectionString("Default");

            }
        }

        static public string[] SupportedLaunguageKeys
        {
            get
            {
                if (string.IsNullOrEmpty(config.GetConnectionString("Default")))
                {

                    ConfigurationManager configurationManager = new();
                    try
                    {
                        configurationManager.SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../ShopAPI"));
                        configurationManager.AddJsonFile("appsettings.json");
                    }
                    catch
                    {
                        configurationManager.AddJsonFile("appsettings.Production.json");
                    }
                    return configurationManager.GetSection("SupportedLanguage:Launguages").Get<string[]>();



                }

                return config.GetSection("SupportedLanguage:Launguages").Get<string[]>();

            }
        }
        static public string DefaultLanguageKey
        {
            get
            {
                if (string.IsNullOrEmpty(config.GetConnectionString("Default")))
                {

                    ConfigurationManager configurationManager = new();
                    try
                    {
                        configurationManager.SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../ShopAPI"));
                        configurationManager.AddJsonFile("appsettings.json");
                    }
                    catch
                    {
                        configurationManager.AddJsonFile("appsettings.Production.json");
                    }
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
