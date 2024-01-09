using System;

namespace WPF_Client.Models
{
    public class InvitationModel
    {
        public UserModel TargetUser { get; private set; }
        public bool IsAccepted { get; private set; }
        public string TimeRecived { get; private set; }
        public InvitationModel(UserModel targetUser,bool isAccepted, string timeRecived)
        {
            TargetUser = targetUser;
            IsAccepted = isAccepted;
            TimeRecived = timeRecived;
        }
    }
}
