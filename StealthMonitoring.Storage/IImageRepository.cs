using System;

namespace StealthMonitoring.Storage
{
    /// <summary>
    /// Storage-agnostic image repository interface
    /// </summary>
    public interface IImageRepository
    {
        /// <summary>
        /// Resets the image storage.
        /// </summary>
        void Reset();

        /// <summary>
        /// Adds the specified image to the storage.
        /// </summary>
        /// <param name="imageMetadata">The image metadata.</param>
        /// <param name="imageBytes">The image bytes.</param>
        /// <returns>Image ID</returns>
        string Add(ImageMetadata imageMetadata, byte[] imageBytes);

        /// <summary>
        /// Finds the images by date (only DATE part counts)
        /// </summary>
        /// <param name="imageDate">The image date.</param>
        /// <returns>Images associated with this date</returns>
        ImageMetadata[] FindByDate(DateTime imageDate);

        /// <summary>
        /// Gets the binary content.
        /// </summary>
        /// <param name="id">The image identifier.</param>
        /// <returns>Image binary content</returns>
        byte[] GetContent(string id);

        /// <summary>
        /// Gets the dates for which there are images available
        /// </summary>
        /// <returns>List of dates</returns>
        DateTime[] GetDatesWithAvailableImages();
    }
}
