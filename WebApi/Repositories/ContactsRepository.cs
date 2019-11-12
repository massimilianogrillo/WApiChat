using Lavoro.Data;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Linq;
using WebApi.Hubs;

namespace WebApi.Repositories
{
    public class ContactsRepository : BaseRepository
    {
        public ContactsRepository(LaboroContext context) : base(context)
        { }

        /// <summary>
        /// Gets contacts of a user. Includes the users that have started a chat with this user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>list of contacts</returns>
        public List<object> GetSimpleContactsForUser(int userId)
        {

            var user = context.Users.Find(userId);

            //find contacts
            var contacts = ( from c in user.MyContacts
                             join u in context.Users on c.ContactId equals u.Id
                             select new {
                                 id = u.Id.ToString(),
                                 name = u.Name,
                                 avatar = u.Avatar,
                                 status = u.Status,
                                 mood = u.Mood,
                                 bot= u.Bot
                             }
                          );
            //finds the users that have contacted this user
            var chatList = user.Chats.Select(chat => {
                var otherUser = chat.StarterUserId == user.Id ? chat.OtherUser : chat.StarterUser;
                return new {
                    id = otherUser.Id.ToString(),
                    name = otherUser.Name,
                    avatar = otherUser.Avatar,
                    status = otherUser.Status,
                    mood = otherUser.Mood,
                    bot = otherUser.Bot
                };
            });

            var union = contacts.Union(chatList);
            return union.ToList<object>();
        }

        /// <summary>
        /// Get contacts that a user has saved with details
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<object> GetDetailedContactsForUser(int userId)
        {

            var contacts = ( from c in context.Contacts
                             join u in context.Users on c.ContactId equals u.Id
                             where c.UserId == userId
                             select new {
                                 id = u.Id.ToString(),
                                 name = u.Name,
                                 lastName = u.LastName,
                                 avatar = u.Avatar,
                                 nickname = u.NickName,
                                 company = u.Company,
                                 jobTitle = u.JobTitle,
                                 email = u.Email,
                                 phone = u.Phone,
                                 address = u.Address,
                                 birthday = u.Birthday.ToString(),
                                 notes = "",
                                 bot = u.Bot
                             }
                           );
            return contacts.ToList<object>();
        }


    }
}
