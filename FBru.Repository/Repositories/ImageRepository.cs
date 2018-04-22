using FBru.DTO;
using FBru.Repository.Entities;
using FBru.Repository.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace FBru.Repository.Repositories
{
    public class ImageRepository : Repository<Image>, IImageRepository
    {
        public ImageRepository(FBruContext context) : base(context)
        {
        }

        private FBruContext FBruContext => Context as FBruContext;
        public async Task<IQueryable<ImageWithDishDto>> GetImagesWithDish()
        {
            var images = await Task.FromResult(FBruContext.Images
                .Include("Dish")
                .Select(i => new ImageWithDishDto()
                {
                    Id = i.Id,
                    Name = i.Name,
                    IsDisplay = i.IsDisplay,
                    Url = i.Url,
                    Dish = new DishSimpleDetailDto()
                    {
                        Id = i.Dish.Id,
                        Name = i.Dish.Name
                    }
                }).OrderBy(i => i.Id));

            return images;
        }
    }
}