using System;
using System.Text;
using SocialNetwork.DAL.Entities;
using System.Collections.Generic;

namespace SocialNetwork.DAL.Repositories
{
    public interface IUserRepository
    {
        int Create(UserEntity userEntity);
        UserEntity FindByEmail(string email);
        IEnumerable<UserEntity> FindAll();
        UserEntity FindById(int id);
        int Update(UserEntity userEntity);
        int DeleteById(int id);
    }
}
