using FBru.DTO;
using FBru.Repository.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace FBru.Repository.Interfaces
{
    public interface IImageRepository : IRepository<Image>
    {
        Task<IQueryable<ImageWithDishDto>> GetImagesWithDish();
    }
}
