using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MPapi.Models
    {
    public class SeedData
        {
        public static void Initialize(IServiceProvider serviceProvider)
            {
            using (var context = new MPapiContext(
                serviceProvider.GetRequiredService<DbContextOptions<MPapiContext>>()))
                {
                if (context.ListingItem.Count() > 0)
                    {
                    return;
                    }
                context.ListingItem.AddRange(
                    new ListingItem
                        {

                        Id = "1",
                        Title = "My sale listing",
                        Url = "https://example.com/url-to-meme-img.jpg",
                        Description = "Here is my very long description of the item I want to sell I should remember to also include a field for the price of this item. Since not including a field for the price of this item would be very bad",
                        Price = "20",
                        Uploaded = "11/10/2018 10:09:52 PM",
                        email = "myemail@email.com",
                        Seller = "Mary lynn"
                        }
                    );
                context.SaveChanges();
                }
            }
        }
    }
