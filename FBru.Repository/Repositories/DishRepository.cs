using FBru.DTO;
using FBru.Repository.Entities;
using FBru.Repository.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace FBru.Repository.Repositories
{
    public class DishRepository : Repository<Dish>, IDishRepository
    {
        public DishRepository(FBruContext context) : base(context)
        {
        }

        private FBruContext FBruContext => Context as FBruContext;
        public async Task<IQueryable<DishWithCategoryAndRestaurantDto>> GetDishesWithRestaurantAndCategory()
        {
            var dishes = await Task.FromResult(FBruContext.Dishes
                .Include("Restaurant")
                .Include("Category")
                .Include("Images")
                .Select(d => new DishWithCategoryAndRestaurantDto()
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
                }).OrderByDescending(d => d.Id));

            return dishes;
        }

        public async Task<DishDto> GetDishWithFullDetails(int id)
        {
            var dish = await Task.FromResult(FBruContext.Dishes
                .Include("Restaurant")
                .Include("Category")
                .Include("Keywords")
                .Include("Images")
                .Where(d => d.Id == id).Select(d => new DishDto()
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
                    Images = d.Images.Select(i => new ImageDto()
                    {
                        Id = i.Id,
                        Name = i.Name,
                        Url = i.Url,
                        IsDisplay = i.IsDisplay
                    }),
                    Keywords = d.Keywords.Select(k => new KeywordDto()
                    {
                        Id = k.Id,
                        Name = k.Name
                    }),
                    Description = d.Description,
                    Price = d.Price
                }).SingleOrDefault());

            return dish;
        }

        public async Task<Dish> GetDishWithImagesAndKeywords(int id)
        {
            var dishes = await Task.FromResult(FBruContext.Dishes
                .Include("Images")
                .Include("Keywords")
                .SingleOrDefault(d => d.Id == id));

            return dishes;
        }

        public async Task<Dish> GetDishWithKeywords(int id)
        {
            var dish = await Task.FromResult(FBruContext.Dishes
                .Include("Keywords")
                .SingleOrDefault(d => d.Id == id));

            return dish;
        }

        public async Task<IQueryable<DishSimpleDetailDto>> GetDishesWithSimpleDetail()
        {
            var dishes = await Task.FromResult(FBruContext.Dishes
                .Select(d => new DishSimpleDetailDto()
                {
                    Id = d.Id,
                    Name = d.Name
                }).OrderByDescending(d => d.Id));

            return dishes;
        }

        public async Task<IQueryable<DishWithPrice>> GetDishesWithPrice()
        {
            var dishes = await Task.FromResult(FBruContext.Dishes
                .Include("Images").Select(d => new DishWithPrice()
                {
                    Id = d.Id,
                    Name = d.Name,
                    ImageUrl = d.Images.Count > 0 ? d.Images.FirstOrDefault().Url : null,
                    Price = d.Price
                }).OrderByDescending(d => d.Id));

            return dishes;
        }

        public async Task<IQueryable<DishWithPrice>> GetDishesWithPriceByCategory(int id)
        {
            var dishes = await Task.FromResult(FBruContext.Dishes
                .Include("Images").Where(d => d.CategoryId == id).Select(d => new DishWithPrice()
                {
                    Id = d.Id,
                    Name = d.Name,
                    ImageUrl = d.Images.Count > 0 ? d.Images.FirstOrDefault().Url : null,
                    Price = d.Price
                }).OrderByDescending(d => d.Id));

            return dishes;
        }
    }
}