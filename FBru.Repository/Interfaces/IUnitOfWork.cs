using System;
using System.Threading.Tasks;

namespace FBru.Repository.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IAdvertisementRepository Advertisements { get; }
        IBlogRepository Blogs { get; }
        ICategoryRepository Categories { get; }
        IDishRepository Dishes { get; }
        IImageRepository Images { get; }
        IKeywordRepository Keywords { get; }
        IRestaurantRepository Restaurants { get; }
        IUserRepository Users { get; }
        IUserGroupRepository UserGroups { get; }
        Task<int> Completed();
    }
}
