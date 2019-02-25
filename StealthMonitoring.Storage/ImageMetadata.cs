using System;
using System.ComponentModel.Design;
using System.Globalization;

namespace StealthMonitoring.Storage
{
    public class ImageMetadata
    {
        public string Id => ToString();

        public DateTime Date { get; }

        public string Category { get; }

        public string Name { get; }

        internal ImageMetadata(string id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            string[] tokens = id.Split('$');

            Date = DateTime.Parse(tokens[0], CultureInfo.InvariantCulture);
            Category = tokens[1];
            Name = tokens[2];
        }

        public ImageMetadata(DateTime date, string category, string name)
        {
            Date = date;
            Category = category ?? throw new ArgumentNullException(nameof(category));
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public override bool Equals(object obj)
        {
            if (!(obj is ImageMetadata imageMetadata))
                return false;

            return Date == imageMetadata.Date && Name == imageMetadata.Name && Category == imageMetadata.Category;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override string ToString()
        {
            return $"{Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}${Category}${Name}";
        }
    }
}
