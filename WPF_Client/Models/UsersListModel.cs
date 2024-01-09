using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Client.Models
{
    public class UsersListModel
    {
        public List<UserModel> OnlineUsersList {  get; set; }
        public List<UserModel> OfflineUsersList { get; set; }

        public UsersListModel(List<UserModel> onlineUsersList, List<UserModel> offlineUsersList)
        {
            OnlineUsersList = onlineUsersList;
            OfflineUsersList = offlineUsersList;
        }
    }
}
