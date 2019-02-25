using System;
using System.IO;
using NUnit.Framework;

namespace StealthMonitoring.Storage.Tests
{
    [TestFixture]
    public class FileSystemImageRepositoryTests
    {
        [Test]
        public void Constructor_WithNullBasePath_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new FileSystemImageRepository(null));
        }

        [Test]
        public void Constructor_WithNonExistingBasePath_Throws()
        {
            Assert.Throws<DirectoryNotFoundException>(() => new FileSystemImageRepository(@"C:\NonExisting"));
        }

        [Test]
        public void Reset_Wipes_BasePath()
        {
            string basePath = GetTempBaseDirectory();

            string fileName1 = Path.Combine(basePath, "1.txt");
            string fileName2 = Path.Combine(basePath, "2.txt");
            string dirName = Path.Combine(basePath, "subdir");
            File.WriteAllText(fileName1, "1");
            File.WriteAllText(fileName2, "2");
            Directory.CreateDirectory(dirName);

            var fileSystemImageRepository = new FileSystemImageRepository(basePath);
            fileSystemImageRepository.Reset();

            Assert.That(File.Exists(fileName1), Is.False);
            Assert.That(File.Exists(fileName2), Is.False);
            Assert.That(Directory.Exists(dirName), Is.False);
        }
        
        [Test]
        public void AddingAndGetting_SingleImage_Succeeds()
        {
            string basePath = GetTempBaseDirectory();
            var fileSystemImageRepository = new FileSystemImageRepository(basePath);

            DateTime date = DateTime.Today;
            var imageMetadata = new ImageMetadata(date, "test", "first");
            
            fileSystemImageRepository.Add(imageMetadata, new byte[] { 66 });

            var retrievedFiles = fileSystemImageRepository.FindByDate(date);
            Assert.That(retrievedFiles.Length, Is.EqualTo(1));
            Assert.That(retrievedFiles[0], Is.EqualTo(imageMetadata));

            var fileData = fileSystemImageRepository.GetContent(retrievedFiles[0].Id);
            Assert.That(fileData[0], Is.EqualTo(66));
        }

        [Test]
        public void AddingAndGetting_TwoImagesOneDate_Succeeds()
        {
            string basePath = GetTempBaseDirectory();
            var fileSystemImageRepository = new FileSystemImageRepository(basePath);

            DateTime date = DateTime.Today;
            var imageMetadata1 = new ImageMetadata(date, "test", "first");
            var imageMetadata2 = new ImageMetadata(date, "test", "second");

            fileSystemImageRepository.Add(imageMetadata1, new byte[] { 1 });
            fileSystemImageRepository.Add(imageMetadata2, new byte[] { 2 });

            var retrievedFiles = fileSystemImageRepository.FindByDate(date);
            Assert.That(retrievedFiles.Length, Is.EqualTo(2));
            Assert.That(retrievedFiles[0], Is.EqualTo(imageMetadata1));
            Assert.That(retrievedFiles[1], Is.EqualTo(imageMetadata2));

            var fileData = fileSystemImageRepository.GetContent(retrievedFiles[0].Id);
            Assert.That(fileData[0], Is.EqualTo(1));

            fileData = fileSystemImageRepository.GetContent(retrievedFiles[1].Id);
            Assert.That(fileData[0], Is.EqualTo(2));
        }

        [Test]
        public void AddingAndGetting_TwoImagesTwoDates_Succeeds()
        {
            string basePath = GetTempBaseDirectory();
            var fileSystemImageRepository = new FileSystemImageRepository(basePath);

            DateTime date = DateTime.Today;
            DateTime date2 = DateTime.Today.AddDays(-1);
            var imageMetadata1 = new ImageMetadata(date, "test", "first");
            var imageMetadata2 = new ImageMetadata(date2, "test", "second");

            fileSystemImageRepository.Add(imageMetadata1, new byte[] { 1 });
            fileSystemImageRepository.Add(imageMetadata2, new byte[] { 2 });

            var retrievedFiles = fileSystemImageRepository.FindByDate(date);
            Assert.That(retrievedFiles.Length, Is.EqualTo(1));
            Assert.That(retrievedFiles[0], Is.EqualTo(imageMetadata1));
            var fileData = fileSystemImageRepository.GetContent(retrievedFiles[0].Id);
            Assert.That(fileData[0], Is.EqualTo(1));

            retrievedFiles = fileSystemImageRepository.FindByDate(date2);
            Assert.That(retrievedFiles.Length, Is.EqualTo(1));
            Assert.That(retrievedFiles[0], Is.EqualTo(imageMetadata2));
            fileData = fileSystemImageRepository.GetContent(retrievedFiles[0].Id);
            Assert.That(fileData[0], Is.EqualTo(2));
        }

        [Test]
        public void Find_TwoImagesTwoDates_Succeeds()
        {
            string basePath = GetTempBaseDirectory();
            var fileSystemImageRepository = new FileSystemImageRepository(basePath);

            DateTime date = DateTime.Today;
            DateTime date2 = DateTime.Today.AddDays(-1);
            var imageMetadata1 = new ImageMetadata(date, "test", "first");
            var imageMetadata2 = new ImageMetadata(date2, "test", "second");

            fileSystemImageRepository.Add(imageMetadata1, new byte[] { 1 });
            fileSystemImageRepository.Add(imageMetadata2, new byte[] { 2 });

            var retrievedFiles = fileSystemImageRepository.FindByDate(date);
            Assert.That(retrievedFiles.Length, Is.EqualTo(1));
            Assert.That(retrievedFiles[0], Is.EqualTo(imageMetadata1));
            var fileData = fileSystemImageRepository.GetContent(retrievedFiles[0].Id);
            Assert.That(fileData[0], Is.EqualTo(1));

            retrievedFiles = fileSystemImageRepository.FindByDate(date2);
            Assert.That(retrievedFiles.Length, Is.EqualTo(1));
            Assert.That(retrievedFiles[0], Is.EqualTo(imageMetadata2));
            fileData = fileSystemImageRepository.GetContent(retrievedFiles[0].Id);
            Assert.That(fileData[0], Is.EqualTo(2));
        }

        [Test]
        public void GetDatesWithAvailableImages_TwoImagesTwoDates_Succeeds()
        {
            string basePath = GetTempBaseDirectory();
            var fileSystemImageRepository = new FileSystemImageRepository(basePath);

            DateTime date = DateTime.Today;
            DateTime date2 = DateTime.Today.AddDays(-1);
            var imageMetadata1 = new ImageMetadata(date, "test", "first");
            var imageMetadata2 = new ImageMetadata(date2, "test", "second");

            fileSystemImageRepository.Add(imageMetadata1, new byte[] { 1 });
            fileSystemImageRepository.Add(imageMetadata2, new byte[] { 2 });

            var dates = fileSystemImageRepository.GetDatesWithAvailableImages();
            CollectionAssert.AreEquivalent(dates, new[] {  date, date2 });
        }

        [Test]
        public void GetDatesWithAvailableImages_NoImages_ReturnsEmptyResult()
        {
            string basePath = GetTempBaseDirectory();
            var fileSystemImageRepository = new FileSystemImageRepository(basePath);
            var dates = fileSystemImageRepository.GetDatesWithAvailableImages();
            Assert.AreEqual(dates.Length, 0);
        }

        private string GetTempBaseDirectory()
        {
            string path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(path);
            return path;
        }
    }
}
