using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Client.Game;
using WPF_Client.Game.MVVM.Model.Interface;
using WPF_Client.Game.MVVM.View;
using WPF_Client.Game.MVVM.View.Components;
using WPF_Client.Models;
using WPF_Client.Models.Interface;

namespace WPF_Client.Services.Interface
{
    public interface IGameService
    {
        Guid GameId { get; }
        UserModel UserToPlayWith { get; }
        IGameStatusModel GameStatus { get; set; }
        bool IsDiceRolled { get; set; }

        public void OpponentQuite();
        public void TimesUp();
        public void Move(string move);
        public void Quite();
        public void RollDice();
        public void FirstRollDice();
        
    }
}
