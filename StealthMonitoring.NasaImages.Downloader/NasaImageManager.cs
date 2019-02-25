using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using StealthMonitoring.NasaImages.Api;
using StealthMonitoring.Storage;

namespace StealthMonitoring.NasaImages.Downloader
{
    public class NasaImageManager
    {
        private readonly IImageRepository _imageRepository;
        private readonly IMarsRoverApiClient _marsRoverApiClient;

        public NasaImageManager(IImageRepository imageRepository, IMarsRoverApiClient marsRoverApiClient)
        {
            _imageRepository = imageRepository ?? throw new ArgumentNullException(nameof(imageRepository));
            _marsRoverApiClient = marsRoverApiClient ?? throw new ArgumentNullException(nameof(marsRoverApiClient));
        }

        public void ClearLocalImages()
        {
            Console.WriteLine("Resetting the image repository...");
            _imageRepository.Reset();
        }

        public async Task DownloadImagesAsync(IDateListProvider dateListProvider)
        {
            if (dateListProvider == null)
                throw new ArgumentNullException(nameof(dateListProvider));

            var downloadTasks = new List<Task>();
            foreach (DateTime day in dateListProvider.GetDates())
            {
                var imageInfos = await _marsRoverApiClient.GetImageInfosAsync(day).ConfigureAwait(false);

                int imagesLeftForDay = imageInfos.Length;
                Console.WriteLine($"{imagesLeftForDay} images are vailable for the following day: {day:yyyy-MM-dd}");

                foreach (var marsImageInfo in imageInfos)
                {
                    var downloadTask = _marsRoverApiClient.DownloadImageAsync(marsImageInfo);

                    downloadTask.ContinueWith(dt =>
                    {
                        _imageRepository.Add(new ImageMetadata(day, marsImageInfo.RoverName, marsImageInfo.Name), dt.Result);
                        Console.WriteLine($"Image downloaded: {marsImageInfo.RoverName}-{marsImageInfo.Name}, left for the day: {Interlocked.Decrement(ref imagesLeftForDay)}");
                    });

                    downloadTasks.Add(downloadTask);
                }
            }

            Task.WaitAll(downloadTasks.ToArray());
        }
    }
}
