using System;
using System.Collections.Generic;
using System.Text;

namespace Lavoro.Domain
{
    
    public class Group
    {
        public Group()
        {
            Users = new List<UserGroup>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual List<UserGroup> Users{ get; set; }
    }
}
