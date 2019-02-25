using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StealthMonitoring.NasaImages.WebApp.Controllers.Models;
using StealthMonitoring.Storage;

namespace StealthMonitoring.NasaImages.WebApp.Controllers
{
    [Route("api/images")]
    public class ImageController : Controller
    {
        private readonly IImageRepository _imageRepository;

        public ImageController(IImageRepository imageRepository)
        {
            _imageRepository = imageRepository ?? throw new ArgumentNullException(nameof(imageRepository));
        }

        [HttpGet("")]
        public IEnumerable<ImageInfo> GetImages(DateTime date)
        {
            return _imageRepository.FindByDate(date).Select(i => new ImageInfo
            {
                Date = date,
                Name = i.Name,
                Category = i.Category,
                Id = i.Id
            });
        }

        [HttpGet("dates")]
        public AvailableDates GetDates()
        {
            return new AvailableDates
            {
                Dates = _imageRepository.GetDatesWithAvailableImages()
            };
        }

        [HttpGet("{id}")]
        public IActionResult GetImagesBytes(string id)
        {
            return File(_imageRepository.GetContent(id), "image/jpeg");
        }
    }
}
