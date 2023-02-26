using Forum.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forum.Data.Seeding
{
    public class CategoriesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Categories.Any())
            {
                return;
            }

            var categories = new List<(string Name, string ImageUrl)>
            {
                ("Cars", "https://g1-bg.cars.bg/2020-07-18_2/5f12c42792b2e02f6d481cd3o.jpg"),
                ("Sport", "https://nationaltoday.com/wp-content/uploads/2022/10/456840956-min-1.jpg"),
                ("Music", "https://phantom-marca.unidadeditorial.es/51cffcd3bdd40680933dbc47aa45227b/resize/1320/f/jpg/assets/multimedia/imagenes/2022/12/07/16703744281238.jpg"),
            };
            foreach (var category in categories)
            {
                await dbContext.Categories.AddAsync(new Category
                {
                    Name = category.Name,
                    Description = category.Name,
                    Title = category.Name,
                    ImageUrl = category.ImageUrl,
                });
            }
        }
    }
}
