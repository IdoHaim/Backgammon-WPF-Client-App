using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Client.Models;

namespace WPF_Client.Services
{
    /// <summary>
    /// The purpose of this service is to avoid this exception:  
    /// System.InvalidOperationException: 'The calling thread cannot access this object
    /// because a different thread owns it.'
    /// by copying the lists 
    /// </summary>
    public class UserListsService
    {
        // lists
        UsersListModel _usersListModel;

        //event
        public event EventHandler<UserUpdateModel> UsersListUpdateHandler;

        public UserListsService()
        {
            _usersListModel = App.OnlineServiceControl.UsersLists;

            App.OnlineServiceControl.UsersListReceivedEvent += NewListsHandler;
            App.OnlineServiceControl.UsersListUpdateEvent += UpdateListsHandler;

        }

        public UsersListModel GetLists()
        {
            return CloneLists();
        }

        private void UpdateListsHandler(object? sender, UserUpdateModel user)
        {
            UserUpdateModel userUpdateModel = new UserUpdateModel(user.UserName,user.State);
            if (user.State)
            {
                //remove from offline
                _usersListModel.OfflineUsersList.RemoveAll(x => x.Username == user.UserName);

                //add to online
                UserModel newUser = new UserModel() { Username = user.UserName};
                _usersListModel.OnlineUsersList.Add(newUser);
            }
            else
            {
                //remove from online
                _usersListModel.OnlineUsersList.RemoveAll(x => x.Username == user.UserName);

                //add to offline
                UserModel newUser = new UserModel() { Username = user.UserName };
                _usersListModel.OfflineUsersList.Add(newUser);
            }

            UsersListUpdateHandler?.Invoke(this, userUpdateModel);
        }

        private void NewListsHandler(object? sender, UsersListModel lists)
        {
            _usersListModel = lists;

        }

        private UsersListModel CloneLists()
        {
            List<UserModel> onlinelist = new List<UserModel>(_usersListModel.OnlineUsersList.Count);
            List<UserModel> offlinelist = new List<UserModel>(_usersListModel.OfflineUsersList.Count);

            _usersListModel.OnlineUsersList.ForEach((item) =>
            {
                onlinelist.Add(new UserModel() { Username = item.Username });
            });
            _usersListModel.OfflineUsersList.ForEach((item) =>
            {
                offlinelist.Add(new UserModel() { Username = item.Username });
            });

            UsersListModel usersListModel = new UsersListModel(onlinelist,offlinelist);
            return usersListModel;
        }
    }
}
