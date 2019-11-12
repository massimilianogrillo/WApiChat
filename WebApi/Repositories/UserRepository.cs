using Lavoro.Data;
using Lavoro.Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace WebApi.Repositories
{
    public class UserRepository : BaseRepository
    {
        public UserRepository(LaboroContext context) : base(context)
        { }

        /// <summary>
        /// Gets user with the list of chats
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<object> GetUserWithChatList(int userId)
        {
            var user = context.Users.Find(userId);
            var chats = user.Chats;

            //number of total unread messages
            int unread = 0;
            var chatList = user.Chats.Select(chat => {

                var otherUser = chat.StarterUserId == user.Id ? chat.OtherUser : chat.StarterUser;
                var unReadMessage = (chat.StarterUserId == user.Id ? chat.UnreadStarterUser : chat.UnreadOtherUser) ?? 0;
                unread += unReadMessage;
                var lastMsg = chat.Messages.OrderByDescending(msg => msg.Time).FirstOrDefault();
                return new {
                    id = chat.Id.ToString(),
                    contactId = otherUser.Id.ToString(),
                    name = otherUser.Name,
                    unread = unReadMessage,
                    lastMessage = lastMsg?.Message,
                    lastMessageTime = lastMsg?.Time
                };
            }).ToList();

            var thisUser = new {
                id = user.Id.ToString(),
                name = user.Name,
                avatar = user.Avatar,
                status = user.Status,
                mood = user.Mood,
                chatList = chatList,
                unread = unread
            };
            return new List<object>() { thisUser };
        }

        public object GetUserWithStarredFrequentContacts(int userId)
        {
            var user = context.Users.Find(userId);
            var contacts = user.MyContacts;

            var starred = contacts.Where(c => c.Starred).Select(c => c.ContactId.ToString()).ToList();
            var frequent = contacts.Where(c => c.Frequent).Select(c => c.ContactId.ToString()).ToList();

            var groups = user.UserGroups.Select(userGroup => {
                var group = userGroup.Group;
                return new {
                    id = group.Id.ToString(),
                    name = group.Name,
                    contactIds = group.Users.Where(u => u.UserId != user.Id).Select(u => u.UserId.ToString()).ToList()
                };
            }).ToList();

            var thisUser = new {
                id = user.Id.ToString(),
                name = user.Name,
                avatar = user.Avatar,
                starred,
                frequentContacts = frequent,
                groups

            };
            return thisUser;
        }

        public object GetUser(int id)
        {
            var user = context.Users.Find(id);
            if (user == null)
            {
                return null;
            }
            return new
            {
                username = user.Name,
                email = user.Email,
                avatar = user.Avatar,
                unreadMessages = Unread(user)
            };

        }

        /// <summary>
        /// Gests the number of messages a user hasnt read yet
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private int Unread(User user)
        {
            var chatRepository = new ChatRepository(context);
            return user.Chats.Sum(chat => chatRepository.Unread(user.Id, chat));
        }

        public User GetUser(string name)
        {
            var user = (from u in context.Users
                        where u.Name.ToLower().Equals(name.ToLower())
                        select u
                         ).FirstOrDefault();
            return user;
        }
        public int AddUser(User user)
        {
            user.UserGroups = new List<UserGroup> {
                 new UserGroup
                 {
                      GroupId = 2,
                      UserId = user.Id
                 }
            };
            bool wBot = user.Bot;
            List<Contact> contacts = new List<Contact>();
            var allUsers = context.Users.Where(w => !wBot).ToList();
            foreach (var item in allUsers)
            {
                Contact c = new Contact
                {
                    ContactId = item.Id,
                    UserId = user.Id,
                    Starred = false,
                    Frequent = false
                };
                contacts.Add(c);
            }
            user.MyContacts = contacts;
            context.Users.Add(user);
            context.SaveChanges();
            return user.Id;
        }
    }

}
