using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;

namespace StealthMonitoring.NasaImages.Downloader
{
    public class FileDateListProvider : IDateListProvider
    {
        private readonly string _fileName;

        public FileDateListProvider(string fileName)
        {
            _fileName = fileName ?? throw new ArgumentNullException(nameof(fileName));
        }

        public IEnumerable<DateTime> GetDates()
        {
            string[] allLines = File.ReadAllLines(_fileName);

            foreach (var line in allLines)
            {
                if (DateTime.TryParse(line, CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate))
                    yield return parsedDate;
                else
                    Console.WriteLine($"Could not parse input date: {line}, ignoring!");
            }
        }
    }
}
