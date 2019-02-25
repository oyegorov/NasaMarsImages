using System;

namespace StealthMonitoring.NasaImages.Api
{
    public class MarsImageInfo
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public string Name { get; set; }

        public string RoverName { get; set; }

        public string ImageLink { get; set; }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
