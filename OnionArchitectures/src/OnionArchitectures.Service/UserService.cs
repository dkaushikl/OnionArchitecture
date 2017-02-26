
using System.Collections.Generic;
using OnionArchitectures.Data;
using OnionArchitectures.Repository;

namespace OnionArchitectures.Service
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<UserProfile> _userProfileRepository;

        public UserService(IRepository<User> userRepository, IRepository<UserProfile> userProfileRepository)
        {
            _userRepository = userRepository;
            _userProfileRepository = userProfileRepository;
        }

        public IEnumerable<User> GetUsers()
        {
            return _userRepository.GetAll();
        }

        public User GetUser(long id)
        {
            return _userRepository.Get(id);
        }

        public void InsertUser(User user)
        {
            _userRepository.Insert(user);
        }
        public void UpdateUser(User user)
        {
            _userRepository.Update(user);
        }

        public void DeleteUser(long id)
        {
            UserProfile userProfile = _userProfileRepository.Get(id);
            _userProfileRepository.Remove(userProfile);
            User user = GetUser(id);
            _userRepository.Remove(user);
            _userRepository.SaveChanges();
        }
    }
}
