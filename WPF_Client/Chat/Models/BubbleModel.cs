using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using WPF_Client.Chat.Models.Interface;

namespace WPF_Client.Chat.Models
{
    public class MessageRecived : IBubbleModel
    {
        public string Content { get; set; }

        public HorizontalAlignment alignment => HorizontalAlignment.Right;
        public SolidColorBrush MainColor => new SolidColorBrush(Colors.White);

    }

    public class MessageSend : IBubbleModel
    {
        public string Content { get; set; }

        public HorizontalAlignment alignment => HorizontalAlignment.Left;
        public SolidColorBrush MainColor => (SolidColorBrush)new BrushConverter().ConvertFrom("#ffe599");

    }
}
