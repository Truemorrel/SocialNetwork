using SocialNetwork.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocialNetwork.DAL.Repositories
{
    public interface IFriendRepository
    {
        int Create(FriendEntity friendEntity);
        IEnumerable<FriendEntity> FindAllByUserId(int userId);
        int Delete(int id);
    }
}
