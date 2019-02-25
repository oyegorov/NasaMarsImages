using System;
using System.Threading.Tasks;

namespace StealthMonitoring.NasaImages.Api
{
    /// <summary>
    /// Mars Rover API client
    /// </summary>
    public interface IMarsRoverApiClient
    {
        /// <summary>
        /// Downloads the image from NASA servers.
        /// </summary>
        /// <param name="marsImageInfo">The image metadata.</param>
        /// <returns>Image content</returns>
        Task<byte[]> DownloadImageAsync(MarsImageInfo marsImageInfo);

        /// <summary>
        /// Gets the metadata for the images taken on a given day. The actual payload is not included
        /// </summary>
        /// <param name="day">The day.</param>
        /// <returns>Image metadata list</returns>
        Task<MarsImageInfo[]> GetImageInfosAsync(DateTime day);
    }
}