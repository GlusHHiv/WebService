using webApiMessenger.Domain;
using webApiMessenger.Domain.Entities;
using webApiMessenger.Persistence.Repositories;

namespace webApiMessenger.Application.services
{
    public class UserService
    {
        private UserRepository _userRepository;
        public UserService(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task AddFriend(int user1id, int user2id) 
        {
            await _userRepository.AddFriend(user1id, user2id);
        }
        
        public IEnumerable<User> GetUsers()
        {
            return _userRepository.GetUsers();
        }

        public IEnumerable<User> GetFriends(int id)
        {
            return _userRepository.GetFriends(id);
        }
    }
}
