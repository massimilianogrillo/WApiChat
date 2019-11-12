using System;
using System.Collections.Generic;
using System.Text;

namespace Lavoro.Domain
{
    public class Messages
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public int ChatId { get; set; }
        public virtual Chat Chat { get; set; }
        public DateTime Time { get; set; }
        public string Message { get; set; }
    
    }
}
