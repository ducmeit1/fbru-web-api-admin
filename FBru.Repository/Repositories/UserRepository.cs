using FBru.DTO;
using FBru.Repository.Entities;
using FBru.Repository.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace FBru.Repository.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(FBruContext context) : base(context)
        {
        }

        private FBruContext FBruContext => Context as FBruContext;

        public async Task<IQueryable<UserDto>> GetUsersWithUserGroup()
        {
            var users = await Task.FromResult(FBruContext.Users
                .Include("UserGroup")
                .Select(u => new UserDto()
                {
                    Id = u.Id,
                    Name = u.Name,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    UserGroup = new UserGroupDto()
                    {
                        Id = u.UserGroup.Id,
                        Name = u.UserGroup.Name
                    }
                }).OrderBy(u => u.Id));

            return users;
        }
    }
}
