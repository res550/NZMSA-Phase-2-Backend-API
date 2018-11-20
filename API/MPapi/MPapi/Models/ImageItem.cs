using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MPapi.Models
    {
    public class ImageItem
        {
        public string userId { get; set; }
        public string Title { get; set; }
        public IFormFile Image { get; set; }
        public string Description { get; set; }
        public string Price { get; set; }
        public string email { get; set; }
        public string Seller { get; set; }
        }
    }
