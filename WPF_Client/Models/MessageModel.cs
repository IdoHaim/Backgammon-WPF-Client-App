using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Client.Models
{
    public class MessageModel
    {
        public string Sender { get;private set; }
        public string Message { get;private set; }
        public MessageModel(string sender, string message)
        { 
            Sender = sender;
            Message = message;
        }

    }
}
