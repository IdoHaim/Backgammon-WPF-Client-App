using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WPF_Client.MVVM.View;
using WPF_Client.Services;

namespace WPF_Client
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static OnlineService OnlineServiceControl { get;private set; }
        
        public App()
        {
            OnlineServiceControl = new OnlineService();     
        }
    }
}
