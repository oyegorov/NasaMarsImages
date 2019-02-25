using System.Collections.Generic;

namespace StealthMonitoring.NasaImages.Api.Models
{
    internal class Rover
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string LandingDate { get; set; }

        public string LaunchDate { get; set; }

        public string Status { get; set; }

        public int MaxSol { get; set; }

        public string MaxDate { get; set; }

        public int TotalPhotos { get; set; }

        public IList<Camera> Cameras { get; set; }
    }
}
