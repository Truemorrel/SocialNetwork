using SocialNetwork.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocialNetwork.BLL.Services
{

    public interface IMessageRepository
    {
        int Create(MessageEntity messageEntity);
        IEnumerable<MessageEntity> FindBySenderId(int senderId);
        IEnumerable<MessageEntity> FindByRecipientId(int recipientId);
        int DeleteById(int messageId);
    }

}
