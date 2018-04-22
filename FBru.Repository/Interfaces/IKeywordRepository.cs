using FBru.DTO;
using FBru.Repository.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace FBru.Repository.Interfaces
{
    public interface IKeywordRepository : IRepository<Keyword>
    {
        Task<IQueryable<KeywordDto>> GetKeywordsWithoutDishes();
        Task<KeywordWithDishesDto> GetKeywordWithDishes(int id);
        Task<KeywordWithDishesDto> GetKeywordWithDishes(string name);
    }
}
