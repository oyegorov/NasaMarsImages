using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace StealthMonitoring.Storage
{
    public class FileSystemImageRepository : IImageRepository
    {
        private readonly string _basePath;

        public FileSystemImageRepository(string basePath)
        {
            _basePath = basePath ?? throw new ArgumentNullException(nameof(basePath));

            if (!Directory.Exists(basePath))
                throw new DirectoryNotFoundException($"Directory not found: {basePath}");
        }

        public string Add(ImageMetadata imageMetadata, byte[] imageBytes)
        {
            if (imageMetadata == null)
                throw new ArgumentNullException(nameof(imageMetadata));
            if (imageBytes == null)
                throw new ArgumentNullException(nameof(imageBytes));

            if (imageMetadata.Name.Contains("."))
                throw new ArgumentException("Image name cannot contain two _ characters in a row");

            string directoryPath = GetPathForDate(imageMetadata.Date);

            try
            {
                var directory = Directory.Exists(directoryPath) ? new DirectoryInfo(GetPathForDate(imageMetadata.Date)) : Directory.CreateDirectory(directoryPath);
                File.WriteAllBytes(Path.Combine(directory.FullName, $"{imageMetadata.Category}__{imageMetadata.Name}.jpg"), imageBytes);
                return imageMetadata.Id;
            }
            catch (Exception ex)
            {
                throw new ImageRepositoryOperationFailedException($"Failed to add an image to repository: {imageMetadata.Name}", ex);
            }
        }

        public ImageMetadata[] FindByDate(DateTime imageDate)
        {
            string path = GetPathForDate(imageDate);
            if (!Directory.Exists(path))
                return null;

            IEnumerable<string> files;

            try
            {
                files = Directory.EnumerateFiles(path, "*.jpg");
            }
            catch (Exception ex)
            {
                throw new ImageRepositoryOperationFailedException($"Failed to find images by date", ex);
            }

            return files.Select(f => GetMetadata(imageDate, Path.GetFileNameWithoutExtension(f))).Where(m => m != null).ToArray();
        }

        public DateTime[] GetDatesWithAvailableImages()
        {
            try
            {
                List<DateTime> availableDays = new List<DateTime>();

                foreach (var d in Directory.EnumerateDirectories(_basePath))
                {
                    if (DateTime.TryParse(Path.GetFileNameWithoutExtension(d), CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate))
                        availableDays.Add(parsedDate);
                }

                return availableDays.ToArray();
            }
            catch (Exception ex)
            {
                throw new ImageRepositoryOperationFailedException($"Failed to enumerate available days", ex);
            }
        }

        public byte[] GetContent(string id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            var imageMetadata = new ImageMetadata(id);

            string path = Path.Combine(GetPathForDate(imageMetadata.Date), $"{imageMetadata.Category}__{imageMetadata.Name}.jpg");
            if (!File.Exists(path))
                return null;

            try
            {
                return File.ReadAllBytes(path);
            }
            catch (Exception ex)
            {
                throw new ImageRepositoryOperationFailedException($"Failed to get image {imageMetadata.Name}", ex);
            }
        }

        public void Reset()
        {
            var directory = new DirectoryInfo(_basePath);

            try
            {
                foreach (var file in directory.GetFiles())
                    file.Delete();
                foreach (var subDirectory in directory.GetDirectories())
                    subDirectory.Delete(true);
            }
            catch (Exception ex)
            {
                throw new ImageRepositoryOperationFailedException($"Failed to reset the image repository", ex);
            }
        }

        private string GetPathForDate(DateTime date)
        {
            return Path.Combine(_basePath, date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
        }

        private ImageMetadata GetMetadata(DateTime date, string fileName)
        {
            var tokens = fileName.Split(new [] { "__" }, StringSplitOptions.RemoveEmptyEntries);
            if (tokens.Length != 2)
                return null;

            return new ImageMetadata(date, tokens[0], tokens[1]);
        }
    }
}
