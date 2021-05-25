using System;
using System.Collections.Generic;
using System.Text;

namespace ClientApp.Essencial
{
    public class Message
    {
        public User userAuthor;
        public User userDest;
        public string text { get; set; }
        public DateTime sendDate { get; set; }
        public bool viewed { get; set; }

        public Message(User userAuthor, User userDest)
        {
            this.userAuthor = userAuthor;
            this.userDest = userDest;
        }
    }
}
