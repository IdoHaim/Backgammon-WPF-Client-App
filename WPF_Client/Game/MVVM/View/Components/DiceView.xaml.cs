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

namespace WPF_Client.Game.MVVM.View.Components
{
    /// <summary>
    /// Interaction logic for DiceView.xaml
    /// </summary>
    public partial class DiceView : UserControl
    {
        Image visibleImage;
        public int Value { get; private set; }
        public DiceView()
        {
            InitializeComponent();
            visibleImage = img1;
        }

        public void SetNumber(int number)
        {
            if (number<1 || number >6)
            {
                throw new ArgumentException("Invalid number");
            }

            visibleImage.Visibility = Visibility.Hidden;
            visibleImage = DiceGV.FindName($"img{number}") as Image;
            visibleImage.Visibility = Visibility.Visible;
            Value = number;
        }

        public void ChangeColorToGray()
        {        
                SolidColorBrush newBrush = new SolidColorBrush(Colors.Gray);
                MainBorder.Background = newBrush;       
        }
        public void ChangeColorToWhite()
        {
            SolidColorBrush newBrush = new SolidColorBrush(Colors.White);
            MainBorder.Background = newBrush;
        }


    }
}
