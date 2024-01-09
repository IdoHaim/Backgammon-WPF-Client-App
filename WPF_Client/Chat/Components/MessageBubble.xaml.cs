using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
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
using WPF_Client.Chat.Models.Interface;

namespace WPF_Client.Chat.Components
{
    /// <summary>
    /// Interaction logic for MessageBubble.xaml
    /// </summary>
    public partial class MessageBubble : UserControl
    {
        const int MaxWidth = 600;


        IBubbleModel _bubbleModel;
        public MessageBubble(IBubbleModel bubbleModel)
        {
            InitializeComponent();
            _bubbleModel = bubbleModel;
            this.HorizontalAlignment = _bubbleModel.alignment;
            TimeView.Text = DateTime.Now.ToString("HH:mm");
            this.Margin = new Thickness(10);
            BubbleBorder.Background = _bubbleModel.MainColor;

            WrapText(_bubbleModel.Content,MessageCentent.FontFamily.ToString(), ((int)MessageCentent.FontSize),MaxWidth);
        }

        

        private void WrapText(string inputText, string fontFamily, float fontSize, int maxWidth)
        {
            StringBuilder wrappedText = new StringBuilder();
            int lineLength = 0;
            string[] words = inputText.Split(' ');

            using (Font font = new Font(fontFamily, fontSize))
            {
                foreach (string word in words)
                {
                    SizeF wordSize = MeasureTextSize(word, font);

                    // Check if adding the word would exceed the maximum width
                    if (lineLength + (int)wordSize.Width > maxWidth)
                    {
                        wrappedText.AppendLine();
                        lineLength = 0;
                    }

                    wrappedText.Append(word + " ");
                    lineLength += (int)wordSize.Width;
                }
            }

            MessageCentent.Text = wrappedText.ToString();
        }

        private  SizeF MeasureTextSize(string text, Font font)
        {
            using (Graphics graphics = Graphics.FromImage(new Bitmap(1, 1)))
            {
                return graphics.MeasureString(text, font);
            }
        }
    }


}

