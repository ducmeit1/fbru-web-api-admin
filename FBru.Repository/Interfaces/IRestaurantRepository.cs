using FBru.DTO;
using FBru.Repository.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace FBru.Repository.Interfaces
{
    public interface IRestaurantRepository : IRepository<Restaurant>
    {
        Task<IQueryable<RestaurantWithNumberOfDishes>> GetRestaurantsWithNumberOfDishes();
        Task<IQueryable<RestaurantWithDishesHaveRestaurantDto>> GetRestaurantsWithDishesHaveRestaurantDto();
        Task<IQueryable<RestaurantWithDishesHaveCategoryDto>> GetRestaurantsWithDishesHaveCategoryDto();
        Task<RestaurantWithDishesHaveCategoryDto> GetRestaurantWithDishesHaveCategoryDto(int id);
        Task<RestaurantWithDishesHaveRestaurantDto> GetRestaurantWithDishesHaveRestaurantDto(int id);
        Task<IQueryable<RestaurantDto>> GetRestaurantsWithoutDishes();

    }
}
