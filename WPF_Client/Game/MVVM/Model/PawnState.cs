using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using WPF_Client.Core;
using WPF_Client.Game.MVVM.Model.Interface;

namespace WPF_Client.Game.MVVM.Model
{
    public class WhitePawn : IPawnState
    {
        public int coefficient { get { return 1; } }
        public SolidColorBrush MainColor => new SolidColorBrush(Colors.White);

        public SolidColorBrush SeconderyColor => (SolidColorBrush)new BrushConverter().ConvertFrom("#e5e5e5");

    }
    
    public class BlackPawn : IPawnState
    {
        public int coefficient { get { return -1; } }
        public SolidColorBrush MainColor => (SolidColorBrush)new BrushConverter().ConvertFrom("#212121");

        public SolidColorBrush SeconderyColor => (SolidColorBrush)new BrushConverter().ConvertFrom("#151515");
    }

   
}
