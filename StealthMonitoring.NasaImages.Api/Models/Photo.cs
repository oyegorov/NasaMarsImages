namespace StealthMonitoring.NasaImages.Api.Models
{
    internal class Photo
    {
        public int Id { get; set; }

        public int Sol { get; set; }

        public Camera Camera { get; set; }

        public string ImgSrc { get; set; }

        public string EarthDate { get; set; }

        public Rover Rover { get; set; }
    }
}
