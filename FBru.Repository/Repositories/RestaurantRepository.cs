using FBru.DTO;
using FBru.Repository.Entities;
using FBru.Repository.Interfaces;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace FBru.Repository.Repositories
{
    public class RestaurantRepository : Repository<Restaurant>, IRestaurantRepository
    {
        public RestaurantRepository(FBruContext context) : base(context)
        {
        }

        public async Task<IQueryable<RestaurantWithNumberOfDishes>> GetRestaurantsWithNumberOfDishes()
        {
            var restaurants = await Task.FromResult(FBruContext.Restaurants.Include("Dishes").Select(r =>
                new RestaurantWithNumberOfDishes()
                {
                    Id = r.Id,
                    Name = r.Name,
                    ImageUrl = r.ImageUrl,
                    OpenTime = r.OpenTime,
                    CloseTime = r.CloseTime,
                    Description = r.Description,
                    PhoneNumber = r.PhoneNumber,
                    IsHalal = r.IsHalal,
                    Address = r.Address,
                    NumberOfDishes = r.Dishes.Count
                }).OrderBy(r => r.Id));

            return restaurants;
        }

        public async Task<IQueryable<RestaurantWithDishesHaveRestaurantDto>> GetRestaurantsWithDishesHaveRestaurantDto()
        {
            var restaurants = await Task.FromResult(FBruContext.Restaurants.Include("Dishes").Select(r =>
                new RestaurantWithDishesHaveRestaurantDto()
                {
                    Id = r.Id,
                    Name = r.Name,
                    Dishes = r.Dishes.Select(d => new DishWithRestaurantDto()
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
                    ImageUrl = r.ImageUrl,
                    OpenTime = r.OpenTime,
                    Description = r.Description,
                    CloseTime = r.CloseTime,
                    PhoneNumber = r.PhoneNumber,
                    IsHalal = r.IsHalal,
                    Address = r.Address,
                    NumberOfDishes = r.Dishes.Count
                }).OrderBy(r => r.Id));

            return restaurants;
        }

        public async Task<IQueryable<RestaurantWithDishesHaveCategoryDto>> GetRestaurantsWithDishesHaveCategoryDto()
        {
            var restaurants = await Task.FromResult(FBruContext.Restaurants.Include("Dishes").Select(r =>
                new RestaurantWithDishesHaveCategoryDto()
                {
                    Id = r.Id,
                    Name = r.Name,
                    Dishes = r.Dishes.Select(d => new DishWithCategoryDto()
                    {
                        Id = d.Id,
                        Name = d.Name,
                        Category = new CategoryDto()
                        {
                            Id = d.Category.Id,
                            Name = d.Category.Name
                        },
                        ImageUrl = d.Images.Count > 0 ? d.Images.FirstOrDefault().Url : null,
                        Price = d.Price
                    }),
                    ImageUrl = r.ImageUrl,
                    OpenTime = r.OpenTime,
                    Description = r.Description,
                    CloseTime = r.CloseTime,
                    PhoneNumber = r.PhoneNumber,
                    IsHalal = r.IsHalal,
                    Address = r.Address,
                    NumberOfDishes = r.Dishes.Count
                }).OrderBy(r => r.Id));

            return restaurants;
        }

        public async Task<RestaurantWithDishesHaveCategoryDto> GetRestaurantWithDishesHaveCategoryDto(int id)
        {
            var restaurant = await FBruContext.Restaurants.Include("Dishes").Where(d => d.Id == id).Select(r =>
                new RestaurantWithDishesHaveCategoryDto()
                {
                    Id = r.Id,
                    Name = r.Name,
                    Dishes = r.Dishes.Select(d => new DishWithCategoryDto()
                    {
                        Id = d.Id,
                        Name = d.Name,
                        Category = new CategoryDto()
                        {
                            Id = d.Category.Id,
                            Name = d.Category.Name
                        },
                        ImageUrl = d.Images.Count > 0 ? d.Images.FirstOrDefault().Url : null,
                        Price = d.Price
                    }),
                    ImageUrl = r.ImageUrl,
                    OpenTime = r.OpenTime,
                    Description = r.Description,
                    CloseTime = r.CloseTime,
                    PhoneNumber = r.PhoneNumber,
                    IsHalal = r.IsHalal,
                    Address = r.Address,
                    NumberOfDishes = r.Dishes.Count
                }).SingleOrDefaultAsync();

            return restaurant;
        }

        public async Task<RestaurantWithDishesHaveRestaurantDto> GetRestaurantWithDishesHaveRestaurantDto(int id)
        {
            var restaurant = await FBruContext.Restaurants.Include("Dishes").Where(d => d.Id == id)
                .Select(r => new RestaurantWithDishesHaveRestaurantDto()
                {
                    Id = r.Id,
                    Name = r.Name,
                    Dishes = r.Dishes.Select(d => new DishWithRestaurantDto()
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
                    ImageUrl = r.ImageUrl,
                    OpenTime = r.OpenTime,
                    Description = r.Description,
                    CloseTime = r.CloseTime,
                    PhoneNumber = r.PhoneNumber,
                    IsHalal = r.IsHalal,
                    Address = r.Address,
                    NumberOfDishes = r.Dishes.Count
                }).SingleOrDefaultAsync();

            return restaurant;
        }

        public async Task<IQueryable<RestaurantDto>> GetRestaurantsWithoutDishes()
        {
            var restaurants = await Task.FromResult(FBruContext.Restaurants.Select(r => new RestaurantDto()
            {
                Id = r.Id,
                Name = r.Name
            }).OrderBy(r => r.Id));

            return restaurants;
        }


        private FBruContext FBruContext => Context as FBruContext;
    }
}