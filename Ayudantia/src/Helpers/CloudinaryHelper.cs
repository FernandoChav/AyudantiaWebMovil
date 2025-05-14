using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ayudantia.Src.Helpers
{
    public class CloudinaryHelper
    {
        public static string ExtractPublicIdFromUrl(string url)
        {
            var uri = new Uri(url);
            var path = uri.AbsolutePath;
            var segments = path.Split('/');

            if (segments.Length < 3)
                return string.Empty;

            var folder = segments[^2];
            var file = Path.GetFileNameWithoutExtension(segments[^1]);

            return $"{folder}/{file}";
        }
    }
}