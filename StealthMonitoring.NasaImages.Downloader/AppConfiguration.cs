using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace StealthMonitoring.NasaImages.Downloader
{
    public static class AppConfiguration
    {
        private static readonly IConfigurationRoot Config = BuildConfig();
        private static object _lockObject = new object();
        private static string _imageGalleryPath;
        
        public static string ImageGalleryPath { get; } = GetImageGalleryPath();

        private static IConfigurationRoot BuildConfig()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();

            return configuration;
        }

        private static string GetImageGalleryPath()
        {
            if (_imageGalleryPath != null)
                return _imageGalleryPath;

            string imageGalleryPath;
            lock (_lockObject)
            {
                if (_imageGalleryPath != null)
                    return _imageGalleryPath;

                imageGalleryPath = Config["imageGalleryPath"];
                if (String.IsNullOrEmpty(imageGalleryPath))
                {
                    imageGalleryPath = Path.Combine(Path.GetTempPath(), "MarsImages");
                    Directory.CreateDirectory(imageGalleryPath);
                }

            }

            _imageGalleryPath = imageGalleryPath;
            return _imageGalleryPath;
        }
    }
}
