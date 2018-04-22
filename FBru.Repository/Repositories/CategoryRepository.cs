using FBru.DTO;
using FBru.Repository.Entities;
using FBru.Repository.Interfaces;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace FBru.Repository.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(FBruContext context) : base(context)
        {
        }

        private FBruContext FBruContext => Context as FBruContext;
        public async Task<IQueryable<CategoryDto>> GetCategoriesWithoutDishes()
        {
            var categories = await Task.FromResult(FBruContext.Categories.Select(c => new CategoryDto()
            {
                Id = c.Id,
                Name = c.Name
            }).OrderBy(c => c.Id));

            return categories;
        }

        public async Task<Category> GetCategoryWithDishes(int id)
        {
            var category = await Task.FromResult(FBruContext.Categories
                .Include("Dishes").SingleOrDefault(c => c.Id == id));

            return category;
        }

        public async Task<IQueryable<CategoryWithIconDto>> GetCategoriesWithIcon()
        {
            var categories = await Task.FromResult(FBruContext.Categories.Select(c => new CategoryWithIconDto()
            {
                Id = c.Id,
                Name = c.Name,
                Icon = c.Icon
            }).OrderBy(c => c.Id));

            return categories;
        }

        public async Task<IQueryable<Category>> GetCategoriesWithDishes()
        {
            var categories = await Task.FromResult(FBruContext.Categories
                .Include("Dishes").OrderBy(c => c.Id));

            return categories;
        }

        public async Task<CategoryWithDishesHaveRestaurantDto> GetCategoryWithDishesHaveRestaurants(int id)
        {
            var category = await FBruContext.Categories.Include("Dishes").Where(c => c.Id == id)
                .Select(c => new CategoryWithDishesHaveRestaurantDto()
                {
                    Id = c.Id,
                    Name = c.Name,
                    Dishes = c.Dishes.Select(d => new DishWithRestaurantDto()
                    {
                        Id = d.Id,
                        Name = d.Name,
                        Restaurant = new RestaurantDto()
                        {
                            Id = d.Restaurant.Id,
                            Name = d.Restaurant.Name
                        },
                        ImageUrl = d.Images.Count > 0 ? d.Images.FirstOrDefault().Url : null,
                        Price = d.Price
                    }),
                    Icon = c.Icon,
                    NumberOfDishes = c.Dishes.Count
                }).SingleOrDefaultAsync();

            return category;
        }
    }
}