using System;
using System.Collections.Generic;
using System.Text;

namespace ClientApp.Essencial
{
    public class User
    {
        public string nickName { get; set; }
        public UserStatus status { get; set; }
        public UserProfile profile { get; set; }
    }
}
