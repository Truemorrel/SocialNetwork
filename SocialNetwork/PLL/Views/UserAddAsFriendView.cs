using SocialNetwork.BLL.Exceptions;
using SocialNetwork.BLL.Models;
using SocialNetwork.BLL.Services;
using SocialNetwork.DAL.Entities;
using SocialNetwork.DAL.Repositories;
using SocialNetwork.PLL.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SocialNetwork.PLL.Views
{
    public class UserAddAsFriendView
    {
        private IFriendRepository FriendsRepository;
        private UserService UserService;
        public UserAddAsFriendView(UserService userService)
        {
            FriendsRepository = new FriendRepository();
            UserService = userService;
        }
        public void Show(User user)
        {
            // Console.ForegroundColor = ConsoleColor.Green;
            while (true)
            {
                Console.WriteLine($"Количество друзей: {user.Friends.Count()}");
                Console.WriteLine("Вывести список друзей: 1");
                Console.WriteLine("Добавить друга: 2");
                Console.WriteLine("Удалить друга: 3");
                Console.WriteLine("Выход: 4");

                string input = Console.ReadLine();

                if (input == "4") break;

                switch (input)
                {
                    case "1":
                        {
                            if (user.Friends.Count() == 0)
                            {
                                Console.WriteLine();
                                AlertMessage.Show("у вас нет друзей");
                            }
                            else
                            {
                                user.Friends.ToList().ForEach(friend =>
                                {
                                    Console.WriteLine("имя: {0}, фамилия: {1}", friend.FirstName, friend.LastName);
                                });
                            }
                            break;
                        }
                    case "2":
                        {
                            Console.Write("Введите email вашего друга: ");
                            var friendsEmail = Console.ReadLine();
                            UserService.AddFriend(friendsEmail,user);
                            break;
                        }
                    case "3":
                        {
                            Console.Write("Введите email вашего бывшего друга: ");
                            var friendsEmail = Console.ReadLine();
                            UserService.DeliteFriend(friendsEmail,user);
                            break;
                        }
                }
            }
        }
    }
}