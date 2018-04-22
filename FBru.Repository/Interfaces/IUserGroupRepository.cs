using FBru.DTO;
using FBru.Repository.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace FBru.Repository.Interfaces
{
    public interface IUserGroupRepository : IRepository<UserGroup>
    {
        Task<IQueryable<UserGroupDto>> GetUserGroupsWithoutUsers();
        Task<IQueryable<UserGroupWithUserCountedDto>> GetUserGroupsWithUsersCounted();
    }
}
