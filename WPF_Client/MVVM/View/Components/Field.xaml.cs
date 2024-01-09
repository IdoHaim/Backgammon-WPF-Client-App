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

namespace WPF_Client.MVVM.View.Components
{
    /// <summary>
    /// Interaction logic for Field.xaml
    /// </summary>
    public partial class Field : UserControl
    {
        public string Title
        {
            get { return _Title.Text; }
            set { _Title.Text = value; }
        }
        public string Input
        {
            get { return _Input.Text; }
            set { _Input.Text = value; }
        }
        public Field()
        {
            InitializeComponent();
        }
    }
}
