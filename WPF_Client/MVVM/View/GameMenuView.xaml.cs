using Microsoft.AspNetCore.SignalR.Client;
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
using WPF_Client.Game;
using WPF_Client.Models;

namespace WPF_Client.MVVM.View
{
    /// <summary>
    /// Interaction logic for GameMenuView.xaml
    /// </summary>
    public partial class GameMenuView : UserControl
    {
        List<InvitationModel> invitationsRecived;
        public GameMenuView()
        {
            InitializeComponent();
            OnlineComponentsManegment();
            invitationsRecived = new List<InvitationModel>();

            App.OnlineServiceControl.ReceiveInvitationEvent += ReceiveInvitation2Handler;

            UpdateTables();

            App.OnlineServiceControl.InvitationNoticedAction();
        }



        private void ReceiveInvitation2Handler(object? sender, InvitationModel invitation)
        {
            invitationsRecived.Add(invitation);
            UpdateTables();
        }

        private void OnlineComponentsManegment()
        {
            Header_1.Text = "Hello " + App.OnlineServiceControl.UserName;
            if (App.OnlineServiceControl.ConnectionState == HubConnectionState.Disconnected)
            {
                MatchMakingBTN.Content = $"You Are Offline Right Now\nPlease Connect Or Play Offline";
                MatchMakingBTN.IsEnabled = false;
            }

            else
            {
                MatchMakingBTN.Content = "Click To Find a Match";
                MatchMakingBTN.IsEnabled = true;
            }
        }

        private void PlayWithFriend_Click(object sender, RoutedEventArgs e)
        {
            //GameMainWindow gameMainWindow = new GameMainWindow();
            //gameMainWindow.Show();
        }

        private void MatchMakingBTN_Click(object sender, RoutedEventArgs e)
        {

        }
        private void UpdateTables()
        {
            OnlineUsersTable.Items.Clear();
            InvitationsTable.Items.Clear();

            Application.Current.Dispatcher.Invoke(() =>
            {
                foreach (var item in App.OnlineServiceControl.UsersLists.OnlineUsersList)
                {
                    OnlineUsersTable.Items.Add(item);
                }

                foreach (var item in invitationsRecived)
                {
                    InvitationsTable.Items.Add(item);
                }
            });
        }

        private void AcceptInvitationBTN_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;

            DataGridCell cell = FindVisualParent<DataGridCell>(button);

            InvitationModel invitationModel = cell.DataContext as InvitationModel; //username

            if (invitationModel != null)
            {
                InvitationModel answer = new InvitationModel(new UserModel()
                { Username = invitationModel.TargetUser.Username }, true, string.Empty);

                ConfirmInvitation(answer, invitationModel);
            }

            UpdateTables();
        }

        private void RejectInvitationBTN_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;

            DataGridCell cell = FindVisualParent<DataGridCell>(button);

            InvitationModel invitationModel = cell.DataContext as InvitationModel;

            if (invitationModel != null)
            {
                InvitationModel answer = new InvitationModel(new UserModel()
                { Username = invitationModel.TargetUser.Username }, false, string.Empty);

                ConfirmInvitation(answer, invitationModel);
            }

            UpdateTables();
        }

        private void ConfirmInvitation(InvitationModel invitationModel, InvitationModel invitationToRemove)
        {
            App.OnlineServiceControl.ConfirmInvetation(invitationModel);

            List<InvitationModel> itemsToRemove = new List<InvitationModel>();

            //remove all invetations from this user

            if (invitationModel != null)
            {
                invitationsRecived.RemoveAll(x => x.TargetUser.Username == invitationModel.TargetUser.Username);
            }

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
