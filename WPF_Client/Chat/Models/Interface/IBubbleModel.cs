using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace WPF_Client.Chat.Models.Interface
{
    public interface IBubbleModel
    {
        string Content { get; }
        HorizontalAlignment alignment { get; }
        SolidColorBrush MainColor { get; }

    }
}
