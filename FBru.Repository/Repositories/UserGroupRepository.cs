using FBru.DTO;
using FBru.Repository.Entities;
using FBru.Repository.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace FBru.Repository.Repositories
{
    public class UserGroupRepository : Repository<UserGroup>, IUserGroupRepository
    {
        public UserGroupRepository(FBruContext context) : base(context)
        {
        }

        private FBruContext FBruContext => Context as FBruContext;
        public async Task<IQueryable<UserGroupDto>> GetUserGroupsWithoutUsers()
        {
            var userGroups = await Task.FromResult(FBruContext.UserGroups.Select(u => new UserGroupDto()
            {
                Id = u.Id,
                Name = u.Name
            }).OrderBy(u => u.Id));

            return userGroups;
        }

        public async Task<IQueryable<UserGroupWithUserCountedDto>> GetUserGroupsWithUsersCounted()
        {
            var userGroups = await Task.FromResult(FBruContext.UserGroups.Include("Users").Select(u => new UserGroupWithUserCountedDto()
            {
                Id = u.Id,
                Name = u.Name,
                NumberOfUsers = u.Users.Count
            }).OrderBy(u => u.Id));

            return userGroups;
        }
    }
}
