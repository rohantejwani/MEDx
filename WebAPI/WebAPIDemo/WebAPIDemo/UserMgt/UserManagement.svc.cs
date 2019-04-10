using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using DTO;

namespace UserManagement
{
    public class UserManagement : IUserManagement
    {
        public List<LoginInfo> GetUsers(string loginType)
        {
            return new UserMgtBL().GetUsers(loginType);
        }

        public List<LoginInfo> GetDriver()
        {
            return new UserMgtBL().GetDriver();
        }
        public List<LoginInfo> GetEmployee()
        {
            return new UserMgtBL().GetEmployee();
        }

        public LoginInfo SignIn(string userId, String password)
        {
            return new UserMgtBL().SignIn(userId, password);
        }

        public string SignUp(LoginInfo loginInfo)
        {
            return new UserMgtBL().SignUp(loginInfo);
        }
    }
}
