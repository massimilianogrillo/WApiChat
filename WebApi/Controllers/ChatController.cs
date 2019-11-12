using Lavoro.Data;
using Lavoro.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using WebApi.Hubs;
using WebApi.Repositories;

namespace WebApi.Controllers
{

    [Route("api/[controller]-[action]")]
    [ApiController]
    public class ChatController : BaseController
    {
        private IHubContext<MessageHub> hub;

        public ChatController(LaboroContext context, IHubContext<MessageHub> hubContext) : base(context)
        {
            hub = hubContext;
        }
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<object>> Contacts()
        {
            int id = 26;
            var repository = new ContactsRepository(context);
            var result = repository.GetSimpleContactsForUser(id);
            return result;
        }

        [HttpGet("{id}")]
        public ActionResult<IEnumerable<object>> Contacts(int id)
        {
           
            var repository = new ContactsRepository(context);
            var result = repository.GetSimpleContactsForUser(id);
            return result;
        }

        [HttpGet]
        public ActionResult<IEnumerable<object>> Chats()
        {
            int id = 26;
            var repository = new ChatRepository(context);
            var result = repository.GetChatsForUser(id);
            return result;
        }

        [HttpGet("{id}")]
        public ActionResult<IEnumerable<object>> Chats(int id)
        {
            var repository = new ChatRepository(context);
            var result = repository.GetChatsForUser(id);
            return result;
        }

        [HttpGet("{id}")]
        public ActionResult<object> Chat(int id)
        {
            string jsonTime = Request.Query["time"].FirstOrDefault();
            int userId = int.Parse(Request.Query["userId"].FirstOrDefault());
            var repository = new ChatRepository(context);
            object result;

            //get chat
            if (jsonTime == "null")
                result = repository.GetMessagesForChat(id);
            else
            {
                DateTime time = JsonConvert.DeserializeObject<DateTime>(jsonTime);
                result = repository.GetMessagesForChat(id, userId, time);
            }

            //notify the client apps that the chat has been opened
            var fromPanel = Request.Query["panel"].FirstOrDefault();
            if (fromPanel != null && bool.Parse(fromPanel))
                hub.Clients.Groups(userId.ToString()).SendAsync("open", id, 0);
            return result;
        }

        [HttpGet("{id}")]
        public new ActionResult<IEnumerable<object>> User(int id)
        {
            var repository = new UserRepository(context);
            var result = repository.GetUserWithChatList(id);
            return result;
        }


        [HttpPost("{id}")]
        public void Chat(object value)
        {
            string json = JsonConvert.SerializeObject(value);
            var values = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            int chatId = int.Parse(values["id"].ToString());
            int senderId = int.Parse(values["userId"].ToString());
            var convo = ( (JArray)values["dialog"] ).Select(x => new Messages {
                ChatId = chatId,
                UserId = (int)x["who"],
                Message = (string)x["message"],
                Time = DateTime.Parse(x["time"].ToString())
            }).ToList();

            var repository = new ChatRepository(context);
            var chat = repository.UpdateChat(chatId, senderId, convo);

            //notify the users substribed to the chat
            var groups = new List<string>() { chat.StarterUserId.ToString(), chat.OtherUserId.ToString() };
            hub.Clients.Groups(groups).SendAsync("send", chat.StarterUserId, chat.OtherUserId);
            // hub.Clients.All.SendAsync("send",chat.StarterUserId ,chat.OtherUserId );
        }

        [HttpPost]
        public ActionResult<object> Chats(object value)
        {
            var dictionary = ( value as JObject ).ToObject<Dictionary<string, object>>();
            var repository = new ChatRepository(context);
            var result = repository.CreateChat(Convert.ToInt32(dictionary["starterUserId"]), Convert.ToInt32(dictionary["otherUserId"]));
            return result;
        }

    }
}
