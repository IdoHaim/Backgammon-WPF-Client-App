using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for HomeView.xaml
    /// </summary>
    public partial class HomeView : UserControl
    {


        public HomeView()
        {
            InitializeComponent();

            ShowRules();

            HeaderContent();

            
        }


        private void HeaderContent()
        {      
             Header_1.Text = "Welcome " + App.OnlineServiceControl.UserName;
        }

        private void ShowRules()
        {
            string path = "../../../GameRules.txt";
            string[] lines;

            try
            {
                StreamReader reader = new StreamReader(path);
                lines = reader.ReadToEnd().Split(".");
            }
            catch
            {
                return;
            }

                foreach(var item in lines)
                {
                    Label label = new Label();
                    label.Content = item;
                    ParagraphRules.Children.Add(label);
            
                }

           
        }
    }
}
