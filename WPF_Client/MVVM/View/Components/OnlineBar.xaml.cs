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
using WPF_Client.Models;
using WPF_Client.Services;

namespace WPF_Client.MVVM.View.Components
{
    /// <summary>
    /// Interaction logic for OnlineBar.xaml
    /// </summary>
    public partial class OnlineBar : UserControl
    {

        public OnlineBar()
        {
            InitializeComponent();

            NameBlock.Text = App.OnlineServiceControl.UserName;
            OnlineStatusCircle.Fill = new SolidColorBrush(Colors.Red);

            App.OnlineServiceControl.StatusEvent += UserStatusUpdateHandler;
            App.OnlineServiceControl.ReceiveInvitationEvent += ReceiveInvitationHandler;
            App.OnlineServiceControl.InvitationGotNoticedEvent += InvitationGotNoticedHandler;
        }

        private void InvitationGotNoticedHandler(object? sender, string e)
        {
            NewInvitationMessage.Visibility = Visibility.Hidden;
        }

        private void ReceiveInvitationHandler(object? sender, InvitationModel e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                NewInvitationMessage.Visibility = Visibility.Visible;
            });
        }

        private void UserStatusUpdateHandler(object? sender, HubConnectionState userstatus)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {

                NameBlock.Text = App.OnlineServiceControl.UserName;


                if (userstatus == HubConnectionState.Connected)
                {
                    OnlineStatusCircle.Fill = new SolidColorBrush(Colors.Green);
                }
                else
                {
                    OnlineStatusCircle.Fill = new SolidColorBrush(Colors.Red);
                }
            });
        }


    }
}
