using FBru.DTO;
using FBru.Repository.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace FBru.Repository.Interfaces
{
    public interface IDishRepository : IRepository<Dish>
    {
        Task<IQueryable<DishWithCategoryAndRestaurantDto>> GetDishesWithRestaurantAndCategory();
        Task<DishDto> GetDishWithFullDetails(int id);
        Task<Dish> GetDishWithImagesAndKeywords(int id);
        Task<Dish> GetDishWithKeywords(int id);
        Task<IQueryable<DishSimpleDetailDto>> GetDishesWithSimpleDetail();
        Task<IQueryable<DishWithPrice>> GetDishesWithPrice();
        Task<IQueryable<DishWithPrice>> GetDishesWithPriceByCategory(int id);
    }
}
