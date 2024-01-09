using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPF_Client.Chat;
using WPF_Client.Models;
using WPF_Client.Services;

namespace WPF_Client.MVVM.View
{
    /// <summary>
    /// Interaction logic for ChatView.xaml
    /// </summary>    //
    public partial class ChatView : UserControl
    {
        UserListsService listsService;
        List<UserModel> OnlineUsers;
        List<UserModel> OfflineUsers;
        public ChatView()
        {
            InitializeComponent();

            OnlineUsersTable.Items.Clear();
            OfflineUsersTable.Items.Clear();

            listsService = new UserListsService();
            var listmodel = listsService.GetLists();

            OnlineUsers = listmodel.OnlineUsersList;
            OfflineUsers = listmodel.OfflineUsersList;

            listsService.UsersListUpdateHandler += UpdateListHandler;
            UpdateTables();
        }

        private void UpdateListHandler(object? sender, UserUpdateModel user)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (user.State)
                {
                    //remove from offline
                    OfflineUsers.RemoveAll(x => x.Username == user.UserName);

                    //add to online
                    UserModel newUser = new UserModel() { Username = user.UserName };
                    OnlineUsers.Add(newUser);
                }
                else
                {
                    //remove from online
                    OnlineUsers.RemoveAll(x => x.Username == user.UserName);

                    //add to offline
                    UserModel newUser = new UserModel() { Username = user.UserName };
                    OfflineUsers.Add(newUser);
                }
                UpdateTables();
            });
        }

        private void UpdateTables()
        {
            OnlineUsersTable.Items.Clear();
            OfflineUsersTable.Items.Clear();

            foreach (var item in OnlineUsers)
            {
                OnlineUsersTable.Items.Add(item);
            }

            foreach (var item in OfflineUsers)
            {
                OfflineUsersTable.Items.Add(item);
            }
        }

        private void OpenChatBTN_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;

            DataGridCell cell = FindVisualParent<DataGridCell>(button);

            UserModel user = cell.DataContext as UserModel; //username

            if (user != null)
                App.OnlineServiceControl.OpenNewChat(user.Username);

        }

        private void InviteUserBTN_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;

            DataGridCell cell = FindVisualParent<DataGridCell>(button);

            UserModel user = cell.DataContext as UserModel; //username

            if (user != null)
                App.OnlineServiceControl.SendInvetation(user.Username);
        }


        private T FindVisualParent<T>(DependencyObject child) where T : DependencyObject
        {
            while (child != null)
            {
                DependencyObject parent = VisualTreeHelper.GetParent(child);
                if (parent is T typedParent)
                {
                    return typedParent;
                }
                child = parent;
            }
            return null;
        }

    }
}
