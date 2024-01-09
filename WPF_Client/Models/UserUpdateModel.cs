using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Client.Models
{
    public class UserUpdateModel
    {
        public string UserName { get; set; }
        public bool State { get; set; }  //true => Connect | false => Disconnect

        public UserUpdateModel(string username,bool state)
        {
            UserName = username;
            State = state;
        }
    }
}
