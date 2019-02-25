using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using StealthMonitoring.NasaImages.Api;
using StealthMonitoring.Storage;

namespace StealthMonitoring.NasaImages.Downloader
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            var config = BuildConfig();
            string datesFileName = args.Length > 0 ? args[0] : "dates.txt";

            try
            {
                var imageRepository = new FileSystemImageRepository(GetImageGalleryPath(config));
                var apiClient = new MarsRoverApiClient();
                var nasaImageManager = new NasaImageManager(imageRepository, apiClient);

                nasaImageManager.ClearLocalImages();
                await nasaImageManager.DownloadImagesAsync(new FileDateListProvider(datesFileName)).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Console.WriteLine($"An error has occurred: {e.Message}");
                return -1;
            }

            return 0;
        }

        private static IConfigurationRoot BuildConfig()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();

            return configuration;
        }

        private static string GetImageGalleryPath(IConfigurationRoot config)
        {
            string imageGalleryPath = config["imageGalleryPath"];
            if (String.IsNullOrEmpty(imageGalleryPath))
            {
                imageGalleryPath = Path.Combine(Path.GetTempPath(), "MarsImages");
                Directory.CreateDirectory(imageGalleryPath);
            }

            return imageGalleryPath;
        }
    }
}
