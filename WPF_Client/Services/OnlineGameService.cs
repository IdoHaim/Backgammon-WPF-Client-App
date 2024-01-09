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
using WPF_Client.Services.Interface;

namespace WPF_Client.Services
{
    public class OnlineGameService : IGameService
    {
        
        public Guid GameId {  get; private set; }
        public UserModel UserToPlayWith {  get; private set; }

        public IGameStatusModel GameStatus { get; set; }
        public bool IsDiceRolled { get; set; }

        public OnlineGameService(Guid gameId ,UserModel userToPlayWith)
        {
            GameId = gameId;
            UserToPlayWith = userToPlayWith;
            IsDiceRolled = false;
        }

        // Get
        public void GetYourRollDice(int num)
        {


        }
        public void GetOpponentRollDice(int num)
        {


        }
        public void GetRollDice(int num1,int num2)
        {


        }
        public void GetMove(string move)
        {


        }


        // Set
        public void RollDice()
        {
            App.OnlineServiceControl.RollDice(GameId);
            
        }

        public void Move(string move)
        {
            App.OnlineServiceControl.Move(GameId, move);
        }

        public void Quite()
        {
            App.OnlineServiceControl.Quite(GameId);
        }

        public void FirstRollDice()
        {
            App.OnlineServiceControl.FirstRollDice(GameId);
        }
        public void OpponentQuite()
        {
            GameFinishedModel gameModel = new GameFinishedModel();
            gameModel.PawnState = gameModel.PawnState;
            gameModel.IsYourTurn = GameStatus.IsYourTurn;
            GameStatus.GameWindow.GameStatus = gameModel;
            GameStatus = gameModel;
            gameModel.GameEnded("Q");
        }

        public void TimesUp()
        {
            GameFinishedModel gameModel = new GameFinishedModel();
            gameModel.PawnState = gameModel.PawnState;
            gameModel.IsYourTurn = GameStatus.IsYourTurn;
            GameStatus.GameWindow.GameStatus = gameModel;
            GameStatus = gameModel;
            gameModel.GameEnded();
            
        }

       
    }
}
