using System;
using System.Collections.Generic;
using System.Text;

namespace ClientApp.Essencial
{
    public class UserRepo
    {
        private static UserRepo instance = new UserRepo();
        private User atualUser;
        private UserRepo() { }

        public void setUser(User user)
        {
            atualUser = user;
        }

        public User getUse()
        {
            return atualUser;
        }

        public static UserRepo getInstance()
        {
            return instance;
        }
    }
}
