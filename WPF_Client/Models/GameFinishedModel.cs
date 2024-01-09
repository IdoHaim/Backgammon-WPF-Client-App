using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using WPF_Client.Game;
using WPF_Client.Game.MVVM.Model;
using WPF_Client.Game.MVVM.Model.Interface;
using WPF_Client.Models.Interface;
using WPF_Client.Services.Interface;

namespace WPF_Client.Models
{
    public class GameFinishedModel : IGameStatusModel
    {
        public GameMainWindow GameWindow { get; set; }
        public IGameService GameService { get; set; }
        
        public IPawnState PawnState { get; set; }

        public bool IsYourTurn {  get; set; }

       

        public void GameEnded(string reason = "")
        {
            string yourcolor;
            string opponentcolor;
            if (PawnState.GetType() == typeof(WhitePawn))
            {
                yourcolor = "Whites";
                opponentcolor = "Blacks";
            }
            else
            {
                yourcolor = "Blacks";
                opponentcolor = "Whites";
            }

            if (reason =="Q")
            {
                GameWindow.NotificationsTextBlock.Text = "Your opponent quite";
            }
            else if(reason == "time")
            {
                if (IsYourTurn)
                {
                    GameWindow.NotificationsTextBlock.Text = $"{yourcolor} lost due to the time limitation";
                }
                else
                {
                    GameWindow.NotificationsTextBlock.Text = $"{opponentcolor} lost due to the time limitation";
                }
            }
            else
            {

            }

        }
        public void Quite()
        {
            GameWindow.Close();
        }
        

        

        public void RollDice()
        {
            
        }

        public void GetMove(string move)
        {
            
        }

        public void GetRollDice(int yourNum, int oppNum)
        {
            
        }

        public void Move(StackPanel clickedPanel)
        {
            
        }
    }
}
