using FBru.Repository.Interfaces;
using FBru.Repository.Repositories;
using System.Threading.Tasks;

namespace FBru.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly FBruContext _context;
        public UnitOfWork()
        {
            _context = new FBruContext();
            Advertisements = new AdvertisementRepository(_context);
            Blogs = new BlogRepository(_context);
            Categories = new CategoryRepository(_context);
            Dishes = new DishRepository(_context);
            Images = new ImageRepository(_context);
            Keywords = new KeywordRepository(_context);
            Restaurants = new RestaurantRepository(_context);
            Users = new UserRepository(_context);
            UserGroups = new UserGroupRepository(_context);
        }

        public IAdvertisementRepository Advertisements { get; }
        public IBlogRepository Blogs { get; }
        public ICategoryRepository Categories { get; }
        public IDishRepository Dishes { get; }
        public IImageRepository Images { get; }
        public IKeywordRepository Keywords { get; }
        public IRestaurantRepository Restaurants { get; }
        public IUserRepository Users { get; }
        public IUserGroupRepository UserGroups { get; }

        public async Task<int> Completed()
        {
            return await _context.SaveChangesAsync();
        }
        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}