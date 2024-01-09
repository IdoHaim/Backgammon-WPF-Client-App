using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Client.Game.MVVM.Model.Interface;
using WPF_Client.Game.MVVM.View.Components;
using WPF_Client.Game.MVVM.View;
using WPF_Client.Models.Interface;
using WPF_Client.Game.MVVM.Model;
using WPF_Client.Services.Interface;
using WPF_Client.Game;
using System.Threading;
using System.Windows.Media;
using System.Windows.Controls;

namespace WPF_Client.Models
{
    public class GameStartModel : IGameStatusModel
    {    
        public GameMainWindow GameWindow { get; set; }
        public IGameService GameService { get; set; }
        public bool IsYourTurn {  get; set; }

        public IPawnState PawnState { get ; set; }

        public GameStartModel(IGameService gameService,IPawnState pawnState)
        {
            GameService = gameService;
            PawnState = pawnState;

            if (PawnState.GetType() == typeof(WhitePawn)) //Remove
            {
                IsYourTurn = true;
            }
            else
            {
                IsYourTurn = false;               
            }

            GameService.GameStatus = this;
            
        }

        public void Move(StackPanel s)
        {
            // disabled
        }

        public void Quite()
        {
            GameService.Quite();            
        }

        public void RollDice()
        { 
             GameService.FirstRollDice();   
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

        public void GetMove(string move)
        {
            // disabled
        }

        public void GetRollDice(int your,int opp)
        {
                GameWindow.Dice_1.SetNumber(your);
                GameWindow.Dice_2.SetNumber(opp);
            
                GameActiveFazeModel gameModel = new GameActiveFazeModel();
            if(your > opp)
            {
                GameWindow.NotificationsTextBlock.Text = "You are starting the game";
                IsYourTurn = true;
                ChangeStatus(gameModel);
            }
            else if(opp > your)
            {
                GameWindow.NotificationsTextBlock.Text = "Your opponent is starting the game";
                IsYourTurn = false;
                ChangeStatus(gameModel);
            }
            else
            {
                RollDice();
            }
        }

        public void SetViews()
        {
            GameWindow.OpponentNameBlock.Text = $"Game With\n{GameService.UserToPlayWith.Username}";
            if(!IsYourTurn) 
            {
                RotateTransform rotateTransform = new RotateTransform(180);
                rotateTransform.CenterX = 0.5;
                rotateTransform.CenterY = 0.5;
                GameWindow.GameBoard.RenderTransform = rotateTransform;
            }
        }
    }
}
