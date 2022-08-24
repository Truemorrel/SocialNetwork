using SocialNetwork.BLL.Exceptions;
using SocialNetwork.BLL.Models;
using SocialNetwork.DAL.Entities;
using SocialNetwork.DAL.Repositories;
using SocialNetwork.PLL.Helpers;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SocialNetwork.BLL.Services
{
    public class UserService
    {
        MessageService messageService;
        IUserRepository userRepository;
        IFriendRepository friendsRepository;
        public UserService()
        {
            userRepository = new UserRepository();
            messageService = new MessageService();
            friendsRepository = new FriendRepository();
        }

        public void Register(UserRegistrationData userRegistrationData)
        {
            if (string.IsNullOrEmpty(userRegistrationData.FirstName))
                throw new ArgumentNullException();

            if (string.IsNullOrEmpty(userRegistrationData.LastName))
                throw new ArgumentNullException();

            if (string.IsNullOrEmpty(userRegistrationData.Password))
                throw new ArgumentNullException();

            if (string.IsNullOrEmpty(userRegistrationData.Email))
                throw new ArgumentNullException();

            if (userRegistrationData.Password.Length < 8)
                throw new ArgumentNullException();

            if (!new EmailAddressAttribute().IsValid(userRegistrationData.Email))
                throw new ArgumentNullException();

            if (userRepository.FindByEmail(userRegistrationData.Email) != null)
                throw new ArgumentNullException();

            var userEntity = new UserEntity()
            {
                firstname = userRegistrationData.FirstName,
                lastname = userRegistrationData.LastName,
                password = userRegistrationData.Password,
                email = userRegistrationData.Email
            };

            if (this.userRepository.Create(userEntity) == 0)
                throw new Exception();

        }


        public User Authenticate(UserAuthenticationData userAuthenticationData)
        {
            var findUserEntity = userRepository.FindByEmail(userAuthenticationData.Email);
            if (findUserEntity is null) throw new UserNotFoundException();

            if (findUserEntity.password != userAuthenticationData.Password)
                throw new WrongPasswordException();

            return ConstructUserModel(findUserEntity);
        }

        public User FindByEmail(string email)
        {
            var findUserEntity = userRepository.FindByEmail(email);
            if (findUserEntity is null) throw new UserNotFoundException();

            return ConstructUserModel(findUserEntity);
        }

        public User FindById(int id)
        {
            var findUserEntity = userRepository.FindById(id);
            if (findUserEntity is null) throw new UserNotFoundException();

            return ConstructUserModel(findUserEntity);
        }

        public void AddFriend(string friendsEmail, User user)
        {
            try
            {
                var friend = FindByEmail(friendsEmail).Id;
                var friendEntity = new FriendEntity()
                {
                    user_id = user.Id,
                    friend_id = friend
                };
                friendsRepository.Create(friendEntity);
                SuccessMessage.Show("Пользователь успешно добавлен в друзья");
            }
            catch (UserNotFoundException)
            {
                AlertMessage.Show("Пользователь не найден!");
            }
            catch (Exception ex)
            {
                AlertMessage.Show($"Ошибка базы данных ...\"{ex.Message}\""); ;
            }
        }
        public void DeliteFriend(string friendsEmail, User user)
        {
            try
            {
                var exFriend = friendsRepository.FindAllByUserId(user.Id).
                    Select(friendsEntity => (FindById(friendsEntity.friend_id), friendsEntity.id)).
                    First(friend => friend.Item1.Email == friendsEmail).id;
                friendsRepository.Delete(exFriend);
            }
            catch (UserNotFoundException)
            {
                AlertMessage.Show("Пользователь не обнаружен");
            }
            catch (Exception ex)
            {
                AlertMessage.Show($"Ошибка базы данных ...\"{ex.Message}\""); ;
            }
        }

        public void Update(User user)
        {
            var updatableUserEntity = new UserEntity()
            {
                id = user.Id,
                firstname = user.FirstName,
                lastname = user.LastName,
                password = user.Password,
                email = user.Email,
                photo = user.Photo,
                favorite_movie = user.FavoriteMovie,
                favorite_book = user.FavoriteBook
            };

            if (this.userRepository.Update(updatableUserEntity) == 0)
                throw new Exception();
        }

        private User ConstructUserModel(UserEntity userEntity)
        {
            var incomingMessages = messageService.GetIncomingMessagesByUserId(userEntity.id);

            var outgoingMessages = messageService.GetOutcomingMessagesByUserId(userEntity.id);

            var friends = friendsRepository.FindAllByUserId(userEntity.id).
                Select(friendEntitiy => FindById(friendEntitiy.friend_id));

            return new User(userEntity.id,
                          userEntity.firstname,
                          userEntity.lastname,
                          userEntity.password,
                          userEntity.email,
                          userEntity.photo,
                          userEntity.favorite_movie,
                          userEntity.favorite_book,
                          incomingMessages,
                          outgoingMessages,
                          friends);
        }
    }
}