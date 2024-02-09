using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseManagementSystem.ViewModels.Helpers;

namespace WarehouseManagementSystem.Services
{
    public static class AppSettings
    {
        public static void UpdateConnectionString(string newConnectionString)
        {
            //try
            //{
            //    var builder = new ConfigurationBuilder()
            //        .SetBasePath(Directory.GetCurrentDirectory())
            //        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            //    var configuration = builder.Build();

            //    configuration.GetSection("ConnectionStrings")["CurrentConnection"] = newConnectionString;

            //    var appSettingsFilePath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");

            //    var json = JsonConvert.SerializeObject(configuration, Formatting.Indented);

            //    File.WriteAllText(appSettingsFilePath, json);
            //}
            //catch (Exception ex)
            //{
            //    MessageHelper.ShowErrorMessage(ex.Message);
            //    ExceptionHelper.HandleException(ex);
            //}

            try
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

                var configuration = builder.Build();

                var appSettingsJsonString = File.ReadAllText("appsettings.json");

                var appSettingsJson = JObject.Parse(appSettingsJsonString);

                var jsonConnectionStrings = appSettingsJson["ConnectionStrings"];

                if (jsonConnectionStrings != null)
                {
                    jsonConnectionStrings["CurrentConnection"] = newConnectionString;

                    File.WriteAllText("appsettings.json", appSettingsJson.ToString());
                }
                else
                {
                    throw new InvalidOperationException("Section ConnectionStrings was not found in the file appsettings.json");
                }
            }
            catch (Exception ex)
            {
                MessageHelper.ShowErrorMessage(ex.Message);
                ExceptionHelper.HandleException(ex);
            }
        }
    }
}
