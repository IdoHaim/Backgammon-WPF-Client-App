using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Client.Models;

namespace WPF_Client.Services
{
    public class ChatService
    {
        public event EventHandler<string> MessageRecivedHandler;
        
        List<ChatService> chatsList;
        public UserModel UserToChatWith {  get; private set; }
        
        public ChatService(List<ChatService> chatslist,UserModel userToChatWith)
        {
            UserToChatWith = userToChatWith;
            chatsList = chatslist;
            chatsList.Add(this);
        }
        
        public void MessageRecived(string message)
        {
            MessageRecivedHandler?.Invoke(this,message);
        }

        public void SendMessage(string messageFromChat)
        {//TODO:In futere version add message send successesfuly
            App.OnlineServiceControl.SendMessage(this,messageFromChat);
        }

        public void CloseChat()
        {
            chatsList.Remove(this);
        }
        
    }
}

//get target user
//OnClosing