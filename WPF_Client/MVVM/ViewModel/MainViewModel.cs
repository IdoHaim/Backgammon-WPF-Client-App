using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Client.Core;
using WPF_Client.MVVM.View;

namespace WPF_Client.MVVM.ViewModel
{
    class MainViewModel:ObservableObject
    {
        public RelayCommand HomeViewCommand { get; set; }
        public RelayCommand CennectingMenuViewCommand { get; set; }
        public RelayCommand GameMenuViewCommand { get; set; }
        public RelayCommand ChatViewCommand { get; set; }
    

        public HomeViewModel HomeVM { get; set; }
        public CennectingMenuViewModel CennectingMenuVM { get; set; }
        public GameMenuViewModel GameMenuVM { get; set; }
        public ChatViewModel ChatVM { get; set; }

        
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

        public MainViewModel()
        {
            //Assigning Views to View models
            HomeVM = new HomeViewModel();
            CennectingMenuVM = new CennectingMenuViewModel();
            GameMenuVM = new GameMenuViewModel();
            ChatVM = new ChatViewModel();

            //Setting defult page
            CurrentView = HomeVM;

            //Assigning commands
            HomeViewCommand = new RelayCommand(o =>
            {
                CurrentView = HomeVM;
            });
            CennectingMenuViewCommand = new RelayCommand(o =>
            {
                CurrentView = CennectingMenuVM;
            });
            GameMenuViewCommand = new RelayCommand(o =>
            {
                CurrentView = GameMenuVM;
            });
            ChatViewCommand = new RelayCommand(o =>
            {
                CurrentView = ChatVM;
            });


        }
    }
}
