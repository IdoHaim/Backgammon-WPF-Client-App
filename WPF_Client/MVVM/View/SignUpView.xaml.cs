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

namespace WPF_Client.MVVM.View
{
    /// <summary>
    /// Interaction logic for SignUpView.xaml
    /// </summary>
    public partial class SignUpView : UserControl
    {
        public SignUpView()
        {
            InitializeComponent();

            //protect from exceptions
            UserNameBox._Input.Text = string.Empty;
            PasswordBox._Input.Text = string.Empty;
            ConfirmPasswordBox._Input.Text = string.Empty;


            App.OnlineServiceControl.SignUpEvent += SignUpFeedbackHandler;
            App.OnlineServiceControl.StatusEvent += StatusCheckHandler;

            StatusCheckHandler(null, App.OnlineServiceControl.ConnectionState);
        }

        private void StatusCheckHandler(object? sender, HubConnectionState status)
        {
            if (status == HubConnectionState.Disconnected)
            {
                Application.Current.Dispatcher.Invoke(() => {
                    // Access or modify UI elements here
                Register_BTN.IsEnabled = true;
                });
            }
            else
            {
                Application.Current.Dispatcher.Invoke(() => {
                    // Access or modify UI elements here
                Register_BTN.IsEnabled = false;
                });
            }
        }

        private void Register_BTN_Click(object sender, RoutedEventArgs e)
        {
            if (InputValidation())
            {
                App.OnlineServiceControl.SignUp(UserNameBox._Input.Text, PasswordBox._Input.Text);
            }
        }

        private void SignUpFeedbackHandler(object? sender, string message)
        {
            if (message == "Ok")
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    App.OnlineServiceControl.LogIn(UserNameBox._Input.Text, PasswordBox._Input.Text);
                });
            }
            else
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    MessageToUserBlock.Text = message;
                });
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
            if (PasswordBox._Input.Text != ConfirmPasswordBox._Input.Text)
            {
                MessageToUserBlock.Text = "Password and Confirm Password are inconsistent";
                return false;
            }
            return true;
        }
    }
}

