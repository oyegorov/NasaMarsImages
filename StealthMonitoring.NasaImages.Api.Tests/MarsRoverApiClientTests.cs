using System;
using System.Threading.Tasks;
using NUnit.Framework;

namespace StealthMonitoring.NasaImages.Api.Tests
{
    [TestFixture]
    public class MarsRoverApiClientTests
    {
        [Test]
        public void Constructor_WithNullCategory_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new MarsRoverApiClient(null));
        }

        [Test]
        public void DownloadImageAsync_WithNull_Throws()
        {
            var client = new MarsRoverApiClient();
            Assert.ThrowsAsync<ArgumentNullException>(() => client.DownloadImageAsync(null));
        }

        [Test]
        [Explicit]
        public async Task GetImageInfosAsync_ReturnsImageMetaData()
        {
            var client = new MarsRoverApiClient();
            var images = await client.GetImageInfosAsync(new DateTime(2019, 1, 1));
            Assert.That(images.Length, Is.GreaterThan(0));
        }

        [Test]
        [Explicit]
        public async Task DownloadImageAsync_DownloadsImages()
        {
            var client = new MarsRoverApiClient();
            var images = await client.GetImageInfosAsync(new DateTime(2019, 1, 1));
            var imageBytes = await client.DownloadImageAsync(images[0]);
            Assert.That(imageBytes.Length, Is.GreaterThan(0));
        }
    }
}
