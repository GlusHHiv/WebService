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
        
        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _userRepository.GetUsers();
        }

        public async Task<IEnumerable<User>> GetFriends(int id)
        {
            return await _userRepository.GetFriends(id);
        }
    }
}
