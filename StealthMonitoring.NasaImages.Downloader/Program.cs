using System;
using System.Threading.Tasks;
using StealthMonitoring.NasaImages.Api;
using StealthMonitoring.Storage;

namespace StealthMonitoring.NasaImages.Downloader
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            string datesFileName = args.Length > 0 ? args[0] : "dates.txt";

            try
            {
                await DownloadImages(datesFileName).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Console.WriteLine($"An error has occurred: {e.Message}");
                return -1;
            }

            return 0;
        }

        private static async Task DownloadImages(string datesFileName)
        {
            var imageRepository = new FileSystemImageRepository(AppConfiguration.ImageGalleryPath);
            var apiClient = new MarsRoverApiClient();
            var nasaImageManager = new NasaImageManager(imageRepository, apiClient);

            nasaImageManager.ClearLocalImages();
            await nasaImageManager.DownloadImagesAsync(new FileDateListProvider(datesFileName)).ConfigureAwait(false);
        }
    }
}
