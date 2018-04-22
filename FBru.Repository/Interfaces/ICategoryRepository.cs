using FBru.DTO;
using FBru.Repository.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace FBru.Repository.Interfaces
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<IQueryable<CategoryDto>> GetCategoriesWithoutDishes();
        Task<Category> GetCategoryWithDishes(int id);
        Task<IQueryable<CategoryWithIconDto>> GetCategoriesWithIcon();
        Task<IQueryable<Category>> GetCategoriesWithDishes();
        Task<CategoryWithDishesHaveRestaurantDto> GetCategoryWithDishesHaveRestaurants(int id);
    }
}
