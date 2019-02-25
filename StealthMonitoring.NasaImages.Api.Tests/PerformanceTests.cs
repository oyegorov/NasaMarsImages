using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using NUnit.Framework;

namespace StealthMonitoring.NasaImages.Api.Tests
{
    /// <summary>
    /// The purpose of this test is to compare image downloading performance - parallel vs sequential
    /// </summary>
    [TestFixture]
    [Explicit]
    public class PerformanceTests
    { 
        private Stopwatch _stopwatch;
        private string[] _urlList;

        [OneTimeSetUp]
        public void Setup()
        {
            _urlList = System.IO.File.ReadAllLines("Data\\Urls.txt");
        }

        [Test]
        public void BenchmarkLoadingInParallel()
        {
            var marsRoverApiClient = new MarsRoverApiClient();

            List<Task> tasks = new List<Task>();
            foreach (var url in _urlList)
            {
                tasks.Add(marsRoverApiClient.DownloadImageAsync(new MarsImageInfo()
                {
                    ImageLink = url
                }));
            }

            Task.WaitAll(tasks.ToArray());
        }

        [Test]
        public async Task BenchmarkLoadingSequentially()
        {
            var marsRoverApiClient = new MarsRoverApiClient();

            List<Task> tasks = new List<Task>();
            foreach (var url in _urlList)
            {
                await marsRoverApiClient.DownloadImageAsync(new MarsImageInfo()
                {
                    ImageLink = url
                });
            }
        }

        [SetUp]
        public void Init()
        {
            _stopwatch = Stopwatch.StartNew();
        }

        [TearDown]
        public void Cleanup()
        {
            _stopwatch.Stop();
            TestContext.WriteLine("Excution time for {0} - {1} ms",
                TestContext.CurrentContext.Test.Name,
                _stopwatch.ElapsedMilliseconds);
        }
    }
}
