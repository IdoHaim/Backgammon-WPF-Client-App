using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Client.Core;
using WPF_Client.MVVM.ViewModel.Connection;

namespace WPF_Client.MVVM.ViewModel
{
    class ConnectingMenuChangerViewModel : ObservableObject
    {
        public RelayCommand ConnectingMenuHomeViewCommand { get; set; }
        public RelayCommand LogInViewCommand { get; set; }
        public RelayCommand SignUpViewCommand { get; set; }

        public ConnectingMenuHomeViewModel ConnectingMenuHomeVM { get; set; }
        public LogInViewModel LogInVM { get; set; }
        public SignUpViewModel SignUpVM { get; set; }

        private object _currentView;

        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }
        public ConnectingMenuChangerViewModel()  //TODO: add connection main window "status"
        {
            // Open the signalr connection only when the app is offline
            if (App.OnlineServiceControl.ConnectionState != HubConnectionState.Connected ||
                App.OnlineServiceControl.ConnectionState != HubConnectionState.Reconnecting)
            {
                App.OnlineServiceControl.OpenConnection();
            }

            //Assigning Views to View models
            ConnectingMenuHomeVM = new ConnectingMenuHomeViewModel();
            SignUpVM = new SignUpViewModel();
            LogInVM = new LogInViewModel();

            //Setting defult page
            CurrentView = ConnectingMenuHomeVM;

            //Assigning commands
            ConnectingMenuHomeViewCommand = new RelayCommand(o =>
            {
                CurrentView = ConnectingMenuHomeVM;
            });
            LogInViewCommand = new RelayCommand(o =>
            {
                CurrentView = LogInVM;
            });
            SignUpViewCommand = new RelayCommand(o =>
            {
                CurrentView = SignUpVM;
            });


        }
    }
}
