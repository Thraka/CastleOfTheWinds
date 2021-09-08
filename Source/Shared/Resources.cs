using System.Reflection;
using System.Drawing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CastleOfTheWinds
{
    public static class Resources
    {
        private static readonly string Prefix;
        private static readonly Assembly Assembly;
        private static readonly Dictionary<string, Image> ImageResources;
        private static readonly Dictionary<string, string> TextResources;
        private static readonly HashSet<string> ResourceNames;

        static Resources()
        {
            Prefix = typeof(Resources).FullName!;
            Assembly = typeof(Resources).Assembly;
            ResourceNames = Assembly.GetManifestResourceNames().ToHashSet();
            TextResources = new Dictionary<string, string>();
            ImageResources = new Dictionary<string, Image>();
        }

        public static Image ReadImage(string path)
        {
            var resourceName = GetResourceName("Images", path);

            if (!ImageResources.TryGetValue(resourceName, out var image))
            {
                if (!ResourceNames.Contains(resourceName))
                {
                    throw new ArgumentException($"Image not found {path}", nameof(path));
                }
                using var stream = Assembly.GetManifestResourceStream(resourceName)!;
                image = Image.FromStream(stream);
                ImageResources[resourceName] = image;
            }

            return image;
        }

        public static string ReadText(string path)
        {
            var resourceName = GetResourceName("Text", path);

            if (!TextResources.TryGetValue(resourceName, out var text))
            {
                if (!ResourceNames.Contains(resourceName))
                {
                    throw new ArgumentException($"Text not found {path}", nameof(path));
                }
                using var stream = Assembly.GetManifestResourceStream(resourceName)!;
                using var reader = new StreamReader(stream);
                text = reader.ReadToEnd();
                TextResources[resourceName] = text;
            }

            return text;
        }

        public static string ReadMap(string path)
        {
            var resourceName = GetResourceName("Maps", path);

            if (!TextResources.TryGetValue(resourceName, out var text))
            {
                if (!ResourceNames.Contains(resourceName))
                {
                    throw new ArgumentException($"Map not found {path}", nameof(path));
                }
                using var stream = Assembly.GetManifestResourceStream(resourceName)!;
                using var reader = new StreamReader(stream);
                text = reader.ReadToEnd();
                TextResources[resourceName] = text;
            }

            return text;
        }

        private static string GetResourceName(string resourceType, string path)
        {
            var builder = new StringBuilder(Prefix);
            builder.Append('.');
            builder.Append(resourceType);
            builder.Append('.');
            builder.Append(path.TrimStart('/').TrimStart('\\'));
            builder.Replace('\\', '.');
            builder.Replace('/', '.');
            return builder.ToString();
        }
    }
}
