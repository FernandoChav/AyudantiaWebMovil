using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ayudantia.Src.RequestHelpers
{
    public class ProductParams : PaginationParams
    {
        public string? OrderBy { get; set; }
        public string? Search { get; set; }           // <-- requerida por .Search()
        public string? Brands { get; set; }
        public string? Categories { get; set; }
    }
}