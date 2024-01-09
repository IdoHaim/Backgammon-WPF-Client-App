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
using System.Windows.Shapes;

namespace WPF_Client.Game.MVVM.View.Components
{
    /// <summary>
    /// Interaction logic for PopOutMessage.xaml
    /// </summary>
    public partial class PopOutMessage : Window
    {
        GameMainWindow GameMainWindow;
        public PopOutMessage(GameMainWindow gameMainWindow)
        {
            InitializeComponent();
            GameMainWindow = gameMainWindow;
        }

        private void YesBTN_Click(object sender, RoutedEventArgs e)
        {
            GameMainWindow.QuiteConfirmed();
            this.Close();
        }

        private void NoBTN_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
