using OnionArchitectures.Data;
using OnionArchitectures.Repository;

namespace OnionArchitectures.Service
{
    public class UserProfileService : IUserProfileService
    {
        private readonly IRepository<UserProfile> _userProfileRepository;

        public UserProfileService(IRepository<UserProfile> userProfileRepository)
        {
            this._userProfileRepository = userProfileRepository;
        }

        public UserProfile GetUserProfile(long id)
        {
            return _userProfileRepository.Get(id);
        }
    }
}
