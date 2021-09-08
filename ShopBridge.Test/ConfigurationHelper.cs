using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;


namespace ShopBridge.Test
{
    public class ConfigurationHelper
    {
        public static IConfigurationRoot GetConfiguration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .AddUserSecrets("c54d3f7f-fbc7-423b-8345-d18452252dcf")
                .AddEnvironmentVariables()
                .Build();
        }

    }
}
