using FBru.DTO;
using FBru.Repository.Entities;
using FBru.Repository.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace FBru.Repository.Repositories
{
    public class KeywordRepository : Repository<Keyword>, IKeywordRepository
    {
        public KeywordRepository(FBruContext context) : base(context)
        {
        }

        private FBruContext FBruContext => Context as FBruContext;
        public async Task<IQueryable<KeywordDto>> GetKeywordsWithoutDishes()
        {
            var keywords = await Task.FromResult(FBruContext.Keywords
                .Select(k => new KeywordDto()
                {
                    Id = k.Id,
                    Name = k.Name
                }).OrderBy(k => k.Id));

            return keywords;
        }

        public async Task<KeywordWithDishesDto> GetKeywordWithDishes(int id)
        {
            var keyword = await Task.FromResult(FBruContext.Keywords
                .Include("Dishes")
                .Where(k => k.Id == id)
                .Select(k => new KeywordWithDishesDto()
                {
                    Id = k.Id,
                    Name = k.Name,
                    Dishes = k.Dishes.Select(d => new DishWithCategoryAndRestaurantDto()
                    {
                        Id = d.Id,
                        Name = d.Name,
                        Category = new CategoryDto()
                        {
                            Id = d.Category.Id,
                            Name = d.Category.Name
                        },
                        Restaurant = new RestaurantDto()
                        {
                            Id = d.Restaurant.Id,
                            Name = d.Restaurant.Name
                        },
                        ImageUrl = d.Images.Count > 0 ? d.Images.FirstOrDefault().Url : null,
                        Price = d.Price
                    })
                }).SingleOrDefault());

            return keyword;
        }

        public async Task<KeywordWithDishesDto> GetKeywordWithDishes(string name)
        {
            var keyword = await Task.FromResult(FBruContext.Keywords
                .Include("Dishes")
                .Where(k => k.Name.ToLower() == name.ToLower())
                .Select(k => new KeywordWithDishesDto()
                {
                    Id = k.Id,
                    Name = k.Name,
                    Dishes = k.Dishes.Select(d => new DishWithCategoryAndRestaurantDto()
                    {
                        Id = d.Id,
                        Name = d.Name,
                        Category = new CategoryDto()
                        {
                            Id = d.Category.Id,
                            Name = d.Category.Name
                        },
                        Restaurant = new RestaurantDto()
                        {
                            Id = d.Restaurant.Id,
                            Name = d.Restaurant.Name
                        },
                        ImageUrl = d.Images.Count > 0 ? d.Images.FirstOrDefault().Url : null,
                        Price = d.Price
                    })
                }).SingleOrDefault());

            return keyword;
        }
    }
}