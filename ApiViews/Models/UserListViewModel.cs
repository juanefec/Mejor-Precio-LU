using System;
using System.Collections.Generic;
using mejor_precio_2.Models;

namespace ApiViews.Models
{
    public class UserListViewModel
    {
        private List<UserInfo> userList;
        public List<UserInfo> UserList
        {
            get { return userList;}
            set { userList = value;}
        }
    }
}