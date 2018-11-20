using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MPapi.Models
{
    public class ListingItem
    {
        public int Id { get; set; }
        public string userId { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public string Price { get; set; }
        public string Uploaded { get; set; }
        public string email { get; set; }
        public string Seller { get; set; }
    }
}
