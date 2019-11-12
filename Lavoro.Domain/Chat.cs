using System.Collections.Generic;
using System.Linq;

namespace Lavoro.Domain
{
    public class Chat
    {
        public Chat()
        {
            Messages = new List<Messages>();
        }
        public int Id { get; set; }
        public int StarterUserId { get; set; }
        public virtual User StarterUser { get; set; }
        public int? IdLastMessageSeenByStarterUser { get; set; }
        public virtual Messages LastMessageSeenByStarterUser { get; set; }
        public int? UnreadStarterUser
        {
            get {
                if (LastMessageSeenByStarterUser == null)
                    return Messages.Count();
                return Messages.Where(m => m.Time > LastMessageSeenByStarterUser.Time).Count();
            }
        }
        public int OtherUserId { get; set; }
        public virtual User OtherUser { get; set; }
        public int? IdLastMessageSeenByOtherUser { get; set; }
        public int? UnreadOtherUser
        {
            get {
                if (LastMessageSeenByOtherUser == null)
                    return Messages.Count();
                return Messages.Where(m => m.Time > LastMessageSeenByOtherUser.Time).Count();
            }
        }
        public virtual Messages LastMessageSeenByOtherUser { get; set; }
        public virtual List<Messages> Messages { get; set; }


    }
}
