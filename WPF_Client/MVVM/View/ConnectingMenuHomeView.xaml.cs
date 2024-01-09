using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;


namespace WPF_Client.MVVM.View
{
    /// <summary>
    /// Interaction logic for ConnectingMenuHomeView.xaml
    /// </summary>
    public partial class ConnectingMenuHomeView : UserControl
    {
        public ConnectingMenuHomeView()
        {
            InitializeComponent();

            PropertiesUpdate();

            App.OnlineServiceControl.StatusEvent += UserStatusUpdateHandler;
        }

        private void UserStatusUpdateHandler(object? sender, HubConnectionState userstatus)
        {
            PropertiesUpdate();
        }
        private void PropertiesUpdate()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                StatusTextBlock.Text = "Your Status Is " + App.OnlineServiceControl.ConnectionState.ToString();
                NameTextBlock.Text = App.OnlineServiceControl.UserName;
                if (App.OnlineServiceControl.ConnectionState == HubConnectionState.Connected)
                {
                    Close_Connection_BTN.Visibility = Visibility.Visible;
                }
                else
                {
                    Close_Connection_BTN.Visibility = Visibility.Hidden;
                }
            });
        }

        private void Close_Connection_BTN_Click(object sender, RoutedEventArgs e)
        {
            App.OnlineServiceControl.CloseConnection();
        }
    }
}
