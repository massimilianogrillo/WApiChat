using System;
using System.Collections.Generic;

namespace Lavoro.Domain
{
    public enum UserStatus
    {
        Online,
        Away,
        Do_Not_Disturb,
        Offline
    }
    public class User
    {
        public User()
        {
            MyContacts = new List<Contact>();
            TheirContacts = new List<Contact>();
            UserGroups = new List<UserGroup>();
            InChats = new List<Chat>();
            OutChats = new List<Chat>();
            Files = new List<File>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Avatar { get; set; }
        public string NickName { get; set; }
        public string Company { get; set; }
        public string JobTitle { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Mood { get; set; }
        public UserStatus Online { get; set; }
        public string Status
        {
            get {
                return Online.ToString().ToLower().Replace('_', '-');
            }
            set {
                if (value == "online")
                    Online = UserStatus.Online;
                else if (value == "away")
                    Online = UserStatus.Away;
                else if (value == "offline")
                    Online = UserStatus.Offline;
                else
                    Online = UserStatus.Do_Not_Disturb;
            }

        }
        public DateTime Birthday { get; set; }
        public bool Bot { get; set; }
        public virtual List<Contact> MyContacts { get; set; }
        public virtual List<Contact> TheirContacts { get; set; }
        public virtual List<UserGroup> UserGroups { get; set; }
        public virtual List<Chat> InChats { get; set; }
        public virtual List<Chat> OutChats { get; set; }
        public virtual List<Chat> Chats
        {
            get {
                var chats = new List<Chat>();
                chats.AddRange(InChats);
                chats.AddRange(OutChats);
                return chats;
            }
        }
        public virtual List<File> Files { get; set; }
    }
}
