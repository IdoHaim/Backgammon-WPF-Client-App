using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
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


namespace WPF_Client.MVVM.View
{

    public partial class LogInView : UserControl
    {
        public LogInView()
        {
            InitializeComponent();

            //protect from exceptions
            UserNameBox._Input.Text = string.Empty;
            PasswordBox._Input.Text = string.Empty;

            App.OnlineServiceControl.LogInEvent += LogInFeedbackHandler;
            App.OnlineServiceControl.StatusEvent += StatusCheckHandler;

            StatusCheckHandler(null, App.OnlineServiceControl.ConnectionState);
        }

        private void StatusCheckHandler(object? sender, HubConnectionState status)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (status == HubConnectionState.Disconnected)
                {
                    Log_In_BTN.IsEnabled = true;
                }
                else
                {
                    Log_In_BTN.IsEnabled = false;
                }
            });
        }

        private void LogInFeedbackHandler(object? sender, string message)
        {
            if (message != "Ok")
            {
                Application.Current.Dispatcher.Invoke(() => {
                    // Access or modify UI elements here
                MessageToUserBlock.Text = message;
                });
            }
        }

        private void Log_In_BTN_Click(object sender, RoutedEventArgs e)
        {
            if (InputValidation())
            {
                App.OnlineServiceControl.LogIn(UserNameBox._Input.Text, PasswordBox._Input.Text);
            }
        }

        private bool InputValidation()
        {
            if (UserNameBox._Input.Text.Length < 3)
            {
                MessageToUserBlock.Text = "Name should have at least 3 charecters";
                return false;
            }
            if (PasswordBox._Input.Text.Length < 6)
            {
                MessageToUserBlock.Text = "Password should have at least 6 charecters";
                return false;
            }
            return true;
        }
    }
}
