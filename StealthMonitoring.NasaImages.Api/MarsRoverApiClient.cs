using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using RestSharp;
using StealthMonitoring.NasaImages.Api.Models;

namespace StealthMonitoring.NasaImages.Api
{
    public class MarsRoverApiClient : IMarsRoverApiClient
    {
        private const string ApiBaseUrl = "https://api.nasa.gov/mars-photos/api/v1/";
        private readonly string _apiKey;
        private readonly RestClient _client;

        public MarsRoverApiClient(string apiKey = "DEMO_KEY")
        {
            _apiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
            _client = new RestClient();
        }

        public async Task<MarsImageInfo[]> GetImageInfosAsync(DateTime day)
        {
            var request = new RestRequest($"{ApiBaseUrl}/rovers/{{rover}}/photos", Method.GET);
            request.AddUrlSegment("rover", "curiosity");
            request.AddParameter("earth_date", day.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
            request.AddParameter("api_key", _apiKey); 

            var apiResponse = await _client.ExecuteTaskAsync<MarsImagesApiResponse>(request).ConfigureAwait(false);

            var photos = apiResponse?.Data?.Photos;
            if (photos == null)
                return null;

            var result = photos.Select(p => GetMarsImageInfo(day, p)).ToArray();
            return result;
        }

        public async Task<byte[]> DownloadImageAsync(MarsImageInfo marsImageInfo)
        {
            if (marsImageInfo == null)
                throw new ArgumentNullException(nameof(marsImageInfo));
            if (marsImageInfo.ImageLink == null)
                throw new ArgumentException("ImageLink must be available");

            var request = new RestRequest(marsImageInfo.ImageLink);
            var executionResult = await _client.ExecuteGetTaskAsync(request).ConfigureAwait(false);
            return executionResult.RawBytes;
        }

        private MarsImageInfo GetMarsImageInfo(DateTime day, Photo p)
        {
            Uri uri = new Uri(p.ImgSrc);
            return new MarsImageInfo
            {
                Date = day,
                Id = p.Id,
                ImageLink = p.ImgSrc,
                Name = Path.GetFileNameWithoutExtension(uri.LocalPath),
                RoverName = p.Rover?.Name?.ToUpper(CultureInfo.InvariantCulture)
            };
        }
    }
}
