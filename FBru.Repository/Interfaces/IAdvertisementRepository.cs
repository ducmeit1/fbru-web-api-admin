using FBru.DTO;
using FBru.Repository.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace FBru.Repository.Interfaces
{
    public interface IAdvertisementRepository : IRepository<Advertisement>
    {
        Task<IQueryable<AdvertisementSimpleDetailDto>> GetAdvertisementsWithSimpleDetailOrdered();
        Task<IQueryable<AdvertisementMoreDetailsDto>> GetAdvertisementsWithMoreDetailsOrdered();
    }
}