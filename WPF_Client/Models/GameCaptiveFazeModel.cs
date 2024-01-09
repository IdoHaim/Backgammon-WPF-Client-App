using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using WPF_Client.Game.MVVM.Model.Interface;
using WPF_Client.Game;
using WPF_Client.Models.Interface;
using WPF_Client.Services.Interface;

namespace WPF_Client.Models
{
    public class GameCaptiveFazeModel : IGameStatusModel
    {
        public GameMainWindow GameWindow { get; set; }
        public IGameService GameService { get; set; }

        public IPawnState PawnState { get; set; }

        public bool IsYourTurn { get; set; }
        string MoveString = string.Empty;
        

        public void GetMove(string move)
        {
            
        }

        public void GetRollDice(int yourNum, int oppNum)
        {
            
        }

        public void Move(StackPanel clickedPanel)
        {
            
        }

        public void Quite()
        {
            GameService.Quite();
        }

        public void RollDice()
        {
            if (IsYourTurn)
                GameService.RollDice();
        }
        private void ChangeStatus(IGameStatusModel gameStatus)
        {
            gameStatus.IsYourTurn = IsYourTurn;
            gameStatus.PawnState = PawnState;
            gameStatus.GameWindow = GameWindow;
            gameStatus.GameService = GameService;
            GameWindow.GameStatus = gameStatus;
            GameService.GameStatus = gameStatus;
        }
    }
}
