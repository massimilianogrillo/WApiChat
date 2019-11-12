using System;
using System.Collections.Generic;
using System.Text;

namespace Lavoro.Domain
{
    public class Contact
    {
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public int ContactId { get; set; }
        public virtual User ContactUser { get; set; }
        public bool Starred { get; set; }
        public bool Frequent { get; set; }
    }
}
