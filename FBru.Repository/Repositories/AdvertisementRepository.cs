using FBru.DTO;
using FBru.Repository.Entities;
using FBru.Repository.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace FBru.Repository.Repositories
{
    public class AdvertisementRepository : Repository<Advertisement>, IAdvertisementRepository
    {
        public AdvertisementRepository(FBruContext context) : base(context)
        {
        }

        private FBruContext FBruContext => Context as FBruContext;

        public async Task<IQueryable<AdvertisementSimpleDetailDto>> GetAdvertisementsWithSimpleDetailOrdered()
        {
            var advertisements = await Task.FromResult(FBruContext.Advertisements
                .Where(a => a.IsDisplay).OrderBy(a => a.Order).Select(a => new AdvertisementSimpleDetailDto()
                {
                    Title = a.Title,
                    SubTitle = a.SubTitle,
                    ImageUrl = a.ImageUrl,
                    ActionUrl = a.ActionUrl
                }));
            return advertisements;
        }

        public async Task<IQueryable<AdvertisementMoreDetailsDto>> GetAdvertisementsWithMoreDetailsOrdered()
        {
            var advertisements = await Task.FromResult(FBruContext.Advertisements
                .Where(a => a.IsDisplay).OrderBy(a => a.Order).Select(a => new AdvertisementMoreDetailsDto()
                {
                    Id = a.Id,
                    Title = a.Title,
                    Order = a.Order,
                    ImageUrl = a.ImageUrl,
                    IsDisplay = a.IsDisplay
                }));
            return advertisements;
        }
    }
}