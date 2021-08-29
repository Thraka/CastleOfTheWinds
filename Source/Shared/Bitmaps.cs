using System.Reflection;
using System.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CastleOfTheWinds
{
    public static class Bitmaps
    {
        private static readonly Assembly Assembly = typeof(Bitmaps).Assembly;
        private static readonly Dictionary<string, Image> Images;
        
        static Bitmaps()
        {
            var prefixLength = typeof(Bitmaps).FullName!.Length;

            Images = Assembly.GetManifestResourceNames()
                .ToDictionary(
                    x => x.Substring(prefixLength, x.LastIndexOf('.') - prefixLength).Replace('.', '/'),
                    x => {
                        using var stream = Assembly.GetManifestResourceStream(x)!;
                        return Image.FromStream(stream);
                    });
        }

        public static Image Read(string path)
        {
            return Images.TryGetValue(path, out var image)
                ? image
                : throw new ArgumentException("Image not found", nameof(path));
        }
    }
}
