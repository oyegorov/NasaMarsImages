using System;
using System.ComponentModel.DataAnnotations;

namespace StealthMonitoring.NasaImages.WebApp.Controllers.Models
{
    public class ImageInfo
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
