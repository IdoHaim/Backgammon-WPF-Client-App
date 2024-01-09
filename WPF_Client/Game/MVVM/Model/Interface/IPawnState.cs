using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WPF_Client.Game.MVVM.Model.Interface
{
    public interface IPawnState
    {
        int coefficient { get; }
        SolidColorBrush MainColor { get; }
        SolidColorBrush SeconderyColor { get; }
    }
}
