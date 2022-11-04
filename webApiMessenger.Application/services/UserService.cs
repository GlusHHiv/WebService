using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using webApiMessenger.Domain;
using webApiMessenger.Domain.Entities;
using webApiMessenger.Domain.Repositories;

namespace webApiMessenger.Application.services
{
    public class UserService
    {
        private UserRepository _userRepository;
        
        public UserService(IDbContext dbContext)
        {
            _userRepository = new UserRepository(dbContext);
        }
        
        public void AddFriend(int user1id, int user2id) 
        {
            _userRepository.AddFriend(user1id, user2id);
        }

        public void UpdateUser(User user)
        {
            _userRepository.Update(user);
        }
    }
}
