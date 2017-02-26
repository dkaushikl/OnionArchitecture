
using OnionArchitectures.Data;

namespace OnionArchitectures.Service
{
    public interface IUserProfileService
    {
        UserProfile GetUserProfile(long id);
    }
}
