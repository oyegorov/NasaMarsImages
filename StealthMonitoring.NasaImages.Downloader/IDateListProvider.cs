using System;
using System.Collections.Generic;
using System.Text;

namespace StealthMonitoring.NasaImages.Downloader
{
    public interface IDateListProvider
    {
        IEnumerable<DateTime> GetDates();
    }
}
