using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace StealthMonitoring.Storage.Tests
{
    [TestFixture]
    public class ImageMetadataTests
    {
        [Test]
        public void Constructor_WithNullCategory_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new ImageMetadata(DateTime.Now, null, "name"));
        }

        [Test]
        public void Constructor_WithNullName_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new ImageMetadata(DateTime.Now, "category", null));
        }

        [Test]
        public void Constructor_Initializes_MembersCorrectly()
        {
            var date = DateTime.Now.AddDays(-3);
            string category = "mycategory";
            string name = "myname";
            
            var imageMetadata = new ImageMetadata(date, category, name);
            Assert.That(imageMetadata.Date, Is.EqualTo(date));
            Assert.That(imageMetadata.Name, Is.EqualTo(name));
            Assert.That(imageMetadata.Category, Is.EqualTo(category));
        }
    }
}
