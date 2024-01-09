using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using WPF_Client.Game;
using WPF_Client.Game.MVVM.Model;
using WPF_Client.Game.MVVM.Model.Interface;
using WPF_Client.Game.MVVM.View.Components;
using WPF_Client.Models.Interface;
using WPF_Client.Services;
using WPF_Client.Services.Interface;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WPF_Client.Models
{
    public class GameActiveFazeModel : IGameStatusModel
    {
        public bool IsYourTurn { get; set; }
        public GameMainWindow GameWindow { get; set; }
        public IGameService GameService { get; set; }

        public IPawnState PawnState { get; set; }

        // Active Game helpers
        //string MoveString = string.Empty;
        List<PawnPointer> PointersList = new List<PawnPointer>();
        List<string[]> blackList = new List<string[]>();           //arr len = 2 [location,target]
        List<string[]> mandatoryMoves = new List<string[]>();

        private int _moveslimit;
        private int MovesLimit
        {
            get { return _moveslimit; }
            set
            {
                _moveslimit = value;
                if (_moveslimit <= 0)
                {                   
                    blackList = new List<string[]>();
                    mandatoryMoves = new List<string[]>();
                    PointersList = new List<PawnPointer>();
                    GameService.Move("@");
                }
            }
        }

        public void GetMove(string move)
        {
            if(IsTurnFinished(move))
            {
                return;
            }
            string[] singleMove = move.Split(">");
            StackPanel panel = GameWindow.GameBoard.StackPanels[singleMove[0]];
            foreach (var pawn1 in panel.Children)
            {
                Pawn? pawn = pawn1 as Pawn;
                if (pawn != null)
                {
                    bool isEatten = false;
                    panel.Children.Remove(pawn);
                    foreach (var item in GameWindow.GameBoard.StackPanels[singleMove[1]].Children)
                    {
                        Pawn? pawn2 = item as Pawn;
                        if (pawn2 != null && pawn2.PawnState.GetType() == PawnState.GetType())
                        {
                            GameWindow.GameBoard.StackPanels[singleMove[1]].Children.Remove(pawn2);
                            GameWindow.GameBoard.E0.Children.Add(pawn2);
                        }
                    }
                    IsYourTurn = true;
                    if (!isEatten)
                    {
                        GameWindow.GameBoard.StackPanels[singleMove[1]].Children.Add(pawn);
                        if (IsReadyToFinalFaze())
                        {
                            GameFinalFazeModel gameFinalFazeModel = new GameFinalFazeModel();
                            ChangeStatus(gameFinalFazeModel);
                        }
                    }
                    else
                    {
                        GameCaptiveFazeModel gameCaptiveFazeModel = new GameCaptiveFazeModel();
                        ChangeStatus(gameCaptiveFazeModel);
                    }

                }
            }
        }

        public void GetRollDice(int num1, int num2)
        {
            GameWindow.Dice_1.SetNumber(num1);
            GameWindow.Dice_2.SetNumber(num2);
            if (IsYourTurn)
            {
                if (GameWindow.Dice_1.Value == GameWindow.Dice_2.Value)
                {
                    MovesLimit = 4;
                }
                else
                {
                    MovesLimit = 2;
                    CheckPossibleMoves();
                }
            }
        }
        public void Quite()
        {
            GameService.Quite();
        }

        public void RollDice()
        {
            if(IsYourTurn)
            GameService.RollDice();
        }

        public void Move(StackPanel clickedPanel)
        {
            if (IsYourTurn)
            {
                var forbidenmove = blackList.Where(x => x[0] == clickedPanel.Name).FirstOrDefault();
                if (forbidenmove == null)
                {
                    bool isConfirmed = false;
                    PawnPointer pointer = new PawnPointer();
                    foreach (var item in clickedPanel.Children)
                    {
                        if (item.GetType() == typeof(PawnPointer))
                        {
                            isConfirmed = true;
                            pointer = item as PawnPointer;
                            if (pointer != null)
                                break;
                        }
                    }
                    if (isConfirmed)
                    {

                        string moveString = pointer.OriginalLocation + ">" + clickedPanel.Name;


                        StackPanel panel = GameWindow.GameBoard.StackPanels[pointer.OriginalLocation];
                        panel.Children.Remove(pointer);
                        PointersList.Remove(pointer);

                        if (PointersList.Count != 0)
                        {
                            PawnPointer? secondpointer = PointersList.FirstOrDefault();
                            panel.Children.Remove(secondpointer);
                            PointersList.Remove(secondpointer);
                        }

                        GameService.Move(moveString);
                    }
                    else
                    {
                        CheckSpecificMove(clickedPanel);
                    }
                }
            }
        }

        private void CheckSpecificMove(StackPanel clickedPanel)
        {// dont forget to remove pointer after the functions
            int dice1 = GameWindow.Dice_1.Value;
            int dice2 = GameWindow.Dice_2.Value;
            int Pnumber = int.Parse(clickedPanel.Name.Remove(0, 1)); // A24 -> 24


            if (IsMovelligel(Pnumber, dice1))
            {
                PawnPointer pointer = new PawnPointer();
                pointer.OriginalLocation = clickedPanel.Name;
                StackPanel panel = GameWindow.GameBoard.StackPanels["A" + Pnumber];
                panel.Children.Add(pointer);
                PointersList.Add(pointer);
            }


            // repeate for dice2
            if (GameWindow.Dice_1.Value != GameWindow.Dice_2.Value && IsMovelligel(Pnumber, dice2))
            {
                PawnPointer pointer = new PawnPointer();
                pointer.OriginalLocation = clickedPanel.Name;
                StackPanel panel = GameWindow.GameBoard.StackPanels["A" + Pnumber];
                panel.Children.Add(pointer);
                PointersList.Add(pointer);
            }
        }

        private void CheckPossibleMoves()
        {
            Dictionary<string, StackPanel> _stackPanels = GameWindow.GameBoard.StackPanels;
            string first1 = string.Empty;
            string first2 = string.Empty;
            int counter1 = 0, counter2 = 0;
            int dice1 = GameWindow.Dice_1.Value;
            int dice2 = GameWindow.Dice_2.Value;

            // part 1
            foreach (var item in _stackPanels)
            {
                if (item.Value.Children != null)
                {
                    bool IsSameType;
                    foreach (var child1 in item.Value.Children)
                    {
                        Pawn? child = child1 as Pawn;
                        if (child != null)
                        {
                            IsSameType = child.PawnState.GetType() == PawnState.GetType();
                        }
                        break;
                    }
                    int Pnumber = int.Parse(item.Value.Name.Remove(0, 1));

                    if (IsMovelligel(Pnumber, dice1))
                    {
                        first1 = item.Value.Name;
                        counter1++;
                    }

                    if (IsMovelligel(Pnumber, dice2))
                    {
                        first2 = item.Value.Name;
                        counter2++;
                    }
                }
            }

            // part 2
            ///////////////////////(
            string MarkedLocation = string.Empty;
            int diceX = 0;
            int diceY = 0;

            List<Pawn> pawnslist;
            int counter = 0;
            bool isWhite = PawnState.GetType() == typeof(WhitePawn);
            if (isWhite)
            {
                pawnslist = GameWindow.GameBoard.WhitePawnsList;
            }
            else
            {
                pawnslist = GameWindow.GameBoard.BlackPawnsList;
            }

            foreach (Pawn p in pawnslist)
            {
                StackPanel? panel = p.Parent as StackPanel;
                if (panel != null)
                {
                    int currentplace = int.Parse(panel.Name.Remove(0, 1));

                    if (isWhite)
                    {
                        if (currentplace <= 5)
                        {
                            counter++;
                        }
                    }
                    else
                    {
                        if (currentplace >= 18)
                        {
                            counter++;
                        }
                    }
                }
            }
            int difference = pawnslist.Count - counter;
            ///////////////////////)

            // part 3
            //senario 1 
            if ((counter1 == 1 && counter2 >= 2) ||
               (counter2 == 1 && counter1 >= 2))
            {
                if (counter1 == 1)
                {
                    MarkedLocation = first1;
                    diceX = dice1;
                    diceY = dice2;
                }
                else
                {
                    MarkedLocation = first2;
                    diceX = dice2;
                    diceY = dice1;
                }
                int currentnum = int.Parse(MarkedLocation.Remove(0, 1));
                int Pnumber = currentnum + (diceY * PawnState.coefficient);
                if (!IsMovelligel(Pnumber, diceX))
                {///////////////////////(
                    bool isForbbiden = true;
                    if (difference == 1)
                    {
                        if (isWhite && Pnumber <= 5)
                        {
                            StackPanel panel = _stackPanels["A" + (diceX - 1).ToString()];
                            foreach (var i in panel.Children)
                            {
                                Pawn? p1 = i as Pawn;
                                if (p1 != null && p1.PawnState.GetType() == PawnState.GetType())
                                    isForbbiden = false;
                                break;
                            }
                        }
                        if (!isWhite && Pnumber >= 18)
                        {
                            StackPanel panel = _stackPanels["A" + (23 - diceX - 1).ToString()];
                            foreach (var i in panel.Children)
                            {
                                Pawn? p1 = i as Pawn;
                                if (p1 != null && p1.PawnState.GetType() == PawnState.GetType())
                                    isForbbiden = false;
                                break;
                            }
                        }
                    }
                    if (isForbbiden)
                    {///////////////////////)
                        string[] forbidenMove = { MarkedLocation, ("A" + Pnumber.ToString()) };
                        blackList.Add(forbidenMove);
                    }
                }
            }

            //senario 2
            else if ((counter1 == 0 && counter2 >= 2) ||
                     (counter2 == 0 && counter1 >= 2))
            {
                if (counter1 == 0)
                {
                    MarkedLocation = first1;
                    diceX = dice1;
                    diceY = dice2;
                }
                else
                {
                    MarkedLocation = first2;
                    diceX = dice2;
                    diceY = dice1;
                }
                foreach (var item in _stackPanels)
                {
                    if (item.Value.Children != null)
                    {
                        bool IsSameType;
                        foreach (var child1 in item.Value.Children)
                        {
                            Pawn? child = child1 as Pawn;
                            if (child != null)
                            {
                                IsSameType = child.PawnState.GetType() == PawnState.GetType();
                            }
                            break;
                        }
                        int Pnumber = int.Parse(item.Value.Name.Remove(0, 1));

                        if (IsMovelligel(Pnumber, diceY))
                        {
                            int newPnumber = Pnumber + (diceY * PawnState.coefficient);
                            if (IsMovelligel(newPnumber, diceX))
                            {
                                string[] mandatoryMove = { MarkedLocation, ("A" + newPnumber.ToString()) };
                                mandatoryMoves.Add(mandatoryMove);
                            }
                        }
                    }
                }
                if (mandatoryMoves.Count == 0)
                {
                    MovesLimit = 1;
                }
            }

            //senario 3
            else if (counter1 == 0 && counter2 == 0)
            {
                MovesLimit = 0; //there are no possible moves
            }

            //Empty the lists after your turn!
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

        private bool IsMovelligel(int Pnumber, int dice)
        {
            Dictionary<string, StackPanel> _stackPanels = GameWindow.GameBoard.StackPanels;
            bool IsSameType = false;
            foreach (var child1 in _stackPanels["A" + Pnumber].Children)
            {
                Pawn? child = child1 as Pawn;
                if (child != null)
                {
                    IsSameType = child.PawnState.GetType() == PawnState.GetType();
                }
                break;
            }

            return !(Pnumber + (dice * PawnState.coefficient) > 23 ||
                Pnumber + (dice * PawnState.coefficient) < 0 ||
                (IsSameType && _stackPanels["A" + Pnumber].Children.Count > 1));

        }
        private bool IsTurnFinished(string move)
        {
            if(move == "@")
            {
                IsYourTurn = !IsYourTurn;
                MovesLimit = 2;
                GameWindow.ResetTimer();
                return true;
            }
            return false;
        }

        private bool IsReadyToFinalFaze()
        {
            List<Pawn> pawnslist;
            int counter = 0;
            bool isWhite = PawnState.GetType() == typeof(WhitePawn);
            if (isWhite)
            {
                pawnslist = GameWindow.GameBoard.WhitePawnsList;
            }
            else
            {
                pawnslist = GameWindow.GameBoard.BlackPawnsList;
            }

            foreach (Pawn p in pawnslist)
            {
                StackPanel? panel = p.Parent as StackPanel;
                if (panel != null)
                {
                    int currentplace = int.Parse(panel.Name.Remove(0, 1));

                    if (isWhite)
                    {
                        if (currentplace <= 5)
                        {
                            counter++;
                        }
                    }
                    else
                    {
                        if (currentplace >= 18)
                        {
                            counter++;
                        }
                    }
                }
            }
            return (pawnslist.Count - counter) == 0;
        }
    }
}
