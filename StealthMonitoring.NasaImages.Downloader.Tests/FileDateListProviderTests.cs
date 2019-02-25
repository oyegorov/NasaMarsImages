using System;
using System.Linq;
using NUnit.Framework;

namespace StealthMonitoring.NasaImages.Downloader.Tests
{
    [TestFixture]
    public class FileDateListProviderTests
    {
        [Test]
        public void Constructor_WithNullFilePath_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new FileDateListProvider(null));
        }

        [Test]
        public void FileDateListProvider_GetDates_SupportsDifferentFormats()
        {
            /*
             *  02/27/17
                June 2, 2018
                Jul-13-2016
                April 31, 2018 - this date is ignored, April 31 does not exist!
             */

            var datesListProvider = new FileDateListProvider("Data\\dates.txt");
            var dates = datesListProvider.GetDates().ToArray();

            Assert.That(dates.Length, Is.EqualTo(3));
            Assert.That(dates[0], Is.EqualTo(new DateTime(2017, 2, 27)));
            Assert.That(dates[1], Is.EqualTo(new DateTime(2018, 6, 2)));
            Assert.That(dates[2], Is.EqualTo(new DateTime(2016, 7, 13)));
        }
    }
}
