using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ayudantia.Src.Dtos.Product
{
    public class PhotoUploadDto
    {
        public string PublicId { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
    }
}