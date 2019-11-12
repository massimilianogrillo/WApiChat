using Extensions;
using Lavoro.Data;
using Lavoro.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WebApi.Repositories
{
    public class ChatRepository : BaseRepository
    {
        /// <summary>
        /// Handles the chat API
        /// </summary>
        /// <param name="context"></param>
        public ChatRepository(LaboroContext context) : base(context)
        { }

        /// <summary>
        /// Gets all the chats that a user is ingaged in
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<object> GetChatsForUser(int userId)
        {

            var chats = ( from c in context.Chats
                          join m in context.Messages on c.Id equals m.ChatId
                          where c.StarterUserId == userId || c.OtherUserId == userId
                          group m by m.ChatId into chat
                          select new {
                              who = chat.Key.ToString(),
                              dialog = chat.Select(s =>
                               new {
                                   who = s.UserId.ToString(),
                                   message = s.Message,
                                   time = s.Time,
                                   id = s.Id
                               }).OrderBy(m => m.time).ThenBy(m => m.id).ToList()
                          }
                          );
            return chats.ToList<object>();
        }

        /// <summary>
        /// Gets all the messages from a chat. Use this when the user opens a chat
        /// </summary>
        /// <param name="chatId"></param>
        /// <param name="userId">the user that requires the chat</param>
        /// <param name="lastSeen">the last time user updated the chat</param>
        /// <returns>Chat dialog</returns>
        public object GetMessagesForChat(int chatId, int userId, DateTime lastSeen)
        {
            Chat chat = context.Chats.Find(chatId);

            var dialog = chat.Messages.Select(m => new {
                who = m.UserId.ToString(),
                message = m.Message,
                time = m.Time,
                id = m.Id
            }).OrderBy(m => m.time).ThenBy(m => m.id).ToList();

            // update the last message that the user has seen
            if (dialog.Count > 0)
            {
                if (chat.StarterUserId == userId)
                    chat.IdLastMessageSeenByStarterUser = dialog.LastOrDefault().id;
                else
                {
                    chat.IdLastMessageSeenByOtherUser = dialog.LastOrDefault().id;
                }
                context.SaveChanges();
            }

            return new {
                id = chatId,
                dialog = dialog
            };


        }

        /// <summary>
        /// Gets all the messages from a chat. Use the other overload instead
        /// </summary>
        /// <param name="chatId"></param>
        /// <param name="userId">the user that requires the chat</param>
        /// <param name="lastSeen">the last time user updated the chat</param>
        /// <returns></returns>
        public object GetMessagesForChat(int chatId)
        {
            Chat chat = context.Chats.Find(chatId);

            var dialog = chat.Messages.Select(m => new {
                who = m.UserId.ToString(),
                message = m.Message,
                time = m.Time,
                id = m.Id
            }).OrderBy(m => m.time).ThenBy(m => m.id).ToList();

            return new {
                id = chatId,
                dialog = dialog
            };


        }

        /// <summary>
        /// Updates the chat in the database
        /// </summary>
        /// <param name="chatId"></param>
        /// <param name="userId"></param>
        /// <param name="convo">The chat as the user knows</param>
        /// <returns>The new chat with possibly new messages from the other user</returns>
        public Chat UpdateChat(int chatId, int userId, IEnumerable<Messages> convo)
        {
            var chat = context.Chats.Find(chatId);
            var messages = chat.Messages;

            //predicates to filter the new messages from the conversation that the user has.
            var predicates = new List<Func<Messages, object>> {
                m => m.ChatId,
                m => m.UserId,
                m => m.Message,
                m => m.Time.ToString()
            };
            //filter new messages
            var newMessages = messages.ElementsNotExist(convo, predicates);

            //set last read message from the users. Bots dont read messages
            var lastReadMessage = ( chat.OtherUser.Bot ? newMessages.Where(m => m.UserId == userId) : newMessages )
                .OrderByDescending(m => m.Time)
                .FirstOrDefault();
            if (chat.StarterUserId == userId)
                chat.LastMessageSeenByStarterUser = lastReadMessage;
            else
                chat.LastMessageSeenByOtherUser = lastReadMessage;

            //save new messages
            messages.AddRange(newMessages);
            context.SaveChanges();
            return chat;
        }

        /// <summary>
        /// Creates a new chat between 2 users
        /// </summary>
        /// <param name="starterUserId"></param>
        /// <param name="otherUserId"></param>
        /// <returns>The newly created chat id</returns>
        public object CreateChat(int starterUserId, int otherUserId)
        {
            Chat chat = new Chat {
                StarterUserId = starterUserId,
                OtherUserId = otherUserId
            };
            //save the chat into the database
            chat = context.Chats.AddIfNotExists(chat,
                c => ( c.StarterUserId == chat.StarterUserId && c.OtherUserId == chat.OtherUserId )
                || ( c.StarterUserId == chat.OtherUserId && c.OtherUserId == chat.StarterUserId )
            );
            context.SaveChanges();

            return new {
                id = chat.Id,
            };
        }

        /// <summary>
        /// Returns number of unread messages that a user has in a chat
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="chat"></param>
        /// <returns></returns>
        public int Unread(int userId, Chat chat)
        {
            int? unread;
            if (userId == chat.OtherUserId)
                unread = chat.UnreadOtherUser;
            else if (userId == chat.StarterUserId)
                unread = chat.UnreadStarterUser;
            else
                throw new ArgumentException();
            return unread ?? 0;
        }

    }
}
