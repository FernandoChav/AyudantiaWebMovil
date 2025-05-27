using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Ayudantia.Src.Models;

namespace Ayudantia.Src.RequestHelpers
{
    public class ProductParams : PaginationParams
    {
        public string? OrderBy { get; set; }
        public string? Search { get; set; }
        public string? Brands { get; set; }
        public string? Categories { get; set; }
        public int? Conditions { get; set; }

        public decimal? MinPrice { get; set; }    
        public decimal? MaxPrice { get; set; }
    }
}