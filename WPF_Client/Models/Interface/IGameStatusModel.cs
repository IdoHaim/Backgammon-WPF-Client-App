using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using WPF_Client.Game;
using WPF_Client.Game.MVVM.Model.Interface;
using WPF_Client.Game.MVVM.View;
using WPF_Client.Game.MVVM.View.Components;
using WPF_Client.Services.Interface;

namespace WPF_Client.Models.Interface
{
    public interface IGameStatusModel
    {
        
        ////Fields
        GameMainWindow GameWindow { get; set; }
        IGameService GameService { get; set; }
        IPawnState PawnState { get; set; }
        bool IsYourTurn { get; set; }


        ////Functions
        //void ChangeStatus();
        
        public void Move(StackPanel clickedPanel);
        public void Quite();
      
        public void RollDice();


        public void GetMove(string move);
        
        public void GetRollDice(int yourNum,int oppNum);

    }
}
