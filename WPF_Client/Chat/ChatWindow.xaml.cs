using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using WPF_Client.Chat.Components;
using WPF_Client.Chat.Models;
using WPF_Client.Models;
using WPF_Client.Services;

namespace WPF_Client.Chat
{
    /// <summary>
    /// Interaction logic for ChatWindow.xaml
    /// </summary>
    public partial class ChatWindow : Window
    {
        ChatService chatService;
        bool IsConnected = true;
        public ChatWindow(ChatService chat)
        {
            InitializeComponent();
            chatService = chat;

            Header.Text = $"Chat With \n{chatService.UserToChatWith.Username}";

            App.OnlineServiceControl.UsersListUpdateEvent += UpdateListsHandler;
            chat.MessageRecivedHandler += ChatMessageRecivedHandler;
            Closing += ChatWindow_Closing;

            IsConnectedFunc();
        }

        private void ChatWindow_Closing(object? sender, CancelEventArgs e)
        {
            chatService.CloseChat();
        }

        private void ChatMessageRecivedHandler(object? sender, string messagerecived)
        {
            MessageRecived messageSend = new MessageRecived();
            messageSend.Content = messagerecived;
            
            MessageBubble messageBubble = new MessageBubble(messageSend);
            ContentPanel.Children.Add(messageBubble);
        }

        private void UpdateListsHandler(object? sender, UserUpdateModel e)
        {
            var u= App.OnlineServiceControl.UsersLists.OnlineUsersList.Where(user =>
            user.Username == chatService.UserToChatWith.Username).FirstOrDefault();

            if (u != null) 
            {
                IsConnected = true;
            }
            else
            {
                IsConnected= false;
            }
            IsConnectedFunc();
        }

        private void SendMessage()
        {
            if (MessageBlock.Text == null || MessageBlock.Text == string.Empty)
            { return; }

            //sending the message
            chatService.SendMessage(MessageBlock.Text);

            // creating the visual message
            MessageSend messageSend = new MessageSend();
            messageSend.Content = MessageBlock.Text;

            MessageBubble messageBubble = new MessageBubble(messageSend);
            ContentPanel.Children.Add(messageBubble);

            MessageBlock.Text = string.Empty;
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            SendMessage();
        }

        

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SendMessage();
            }
        }



        private void MessageBlock_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (MessageBlock.Text == null || MessageBlock.Text == string.Empty)
                PlaceHolder_1.Visibility = Visibility.Visible;
            else
                PlaceHolder_1.Visibility = Visibility.Hidden;


        }

        private void IsConnectedFunc()
        {

            Application.Current.Dispatcher.Invoke(() =>
            {
                if (IsConnected)
                {
                    DisconnectedIndicator.Text = string.Empty;
                    SendButton.IsEnabled = true;
                }
                else
                {
                    DisconnectedIndicator.Text = $"{chatService.UserToChatWith.Username} disconnected";
                    SendButton.IsEnabled = false;
                }
            });
        }

    }
}