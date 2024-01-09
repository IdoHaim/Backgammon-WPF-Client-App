using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
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
using WPF_Client.Game.MVVM.Model.Interface;

namespace WPF_Client.Game.MVVM.View.Components
{
    /// <summary>
    /// Interaction logic for Pawn.xaml
    /// </summary>
    public partial class Pawn : UserControl
    {
        public readonly IPawnState PawnState;
        public Pawn(IPawnState pawnState)
        {
            InitializeComponent();
            PawnState = pawnState;
            AssignColors();

        }

        private void AssignColors()
        {
            Circle1.Fill = PawnState.MainColor;
            Circle2.Fill = PawnState.SeconderyColor;
            Circle3.Fill = PawnState.MainColor;
            Circle4.Fill = PawnState.SeconderyColor;
            Circle5.Fill = PawnState.MainColor;
        }
    }
}
