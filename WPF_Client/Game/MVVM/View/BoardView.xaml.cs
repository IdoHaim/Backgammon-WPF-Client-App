using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPF_Client.Game.MVVM.Model;
using WPF_Client.Game.MVVM.Model.Interface;
using WPF_Client.Game.MVVM.View.Components;

namespace WPF_Client.Game.MVVM.View
{
    /// <summary>
    /// Interaction logic for Board.xaml
    /// </summary>
    public partial class BoardView : UserControl
    {
        /// <summary>
        /// 
        /// Whites path:
        /// A -> B -> C -> D
        /// 0 => 23
        /// 
        /// Blacks path:
        /// D -> C -> B -> A
        /// 23 => 0
        /// 
        /// </summary>
        
        public List<Pawn> WhitePawnsList { get; private set; }
        public List<Pawn> BlackPawnsList { get; private set; }
        public Dictionary<string,StackPanel> StackPanels { get; private set; }
        

        public BoardView()
        {
            InitializeComponent();

            WhitePawnsList = new List<Pawn>();
            BlackPawnsList = new List<Pawn>();

            StackPanels = new Dictionary<string, StackPanel>();
            

            ArrangePawnsForMatch();
            ArrangeStacksInDataStructers();
            testChildren();
        }

        private void ArrangePawnsForMatch()
        {
            //StackPanel item = GD.FindName("A2") as StackPanel;
            //Pawn pawn = new Pawn(new WhitePawn());
            //item.Children.Add(pawn);


            for (int i = 0; i < 15; i++)
            {
                Pawn whitePawn = new Pawn(new WhitePawn());
                Pawn blackPawn = new Pawn(new BlackPawn());


                if (i < 5)
                {
                    A5.Children.Add(whitePawn);
                    A18.Children.Add(blackPawn);
                }
                else if(i >= 5 && i < 8)
                {
                    A7.Children.Add(whitePawn);
                    A16.Children.Add(blackPawn);
                }
                else if (i >= 8 && i < 13)
                {
                    A12.Children.Add(whitePawn);
                    A11.Children.Add(blackPawn);
                }
                else
                {
                    A23.Children.Add(whitePawn);
                    A0.Children.Add(blackPawn);
                }

                WhitePawnsList.Add(whitePawn);
                BlackPawnsList.Add(blackPawn);

            }
        }

        private void ArrangeStacksInDataStructers()
        {
            int index = 0;
            foreach (var item in GA.Children)
            {
                StackPanel ?panel = item as StackPanel;
                if(panel != null)
                {
                    StackPanels.TryAdd(panel.Name,panel);
                    
                }
            }

            foreach (var item in GB.Children)
            {
                StackPanel? panel = item as StackPanel;
                if (panel != null)
                {
                    StackPanels.TryAdd(panel.Name, panel);
                    
                }
            }

            foreach (var item in GC.Children)
            {
                StackPanel? panel = item as StackPanel;
                if (panel != null)
                {
                    StackPanels.TryAdd(panel.Name, panel);
                    
                }
            }

            foreach (var item in GD.Children)
            {
                StackPanel? panel = item as StackPanel;
                if (panel != null)
                {
                    StackPanels.TryAdd(panel.Name, panel);
                    
                }
            }

            //StackPanels.TryAdd(E.Name, E);
        }

        private void MouseDown(object sender, MouseButtonEventArgs e)
        {
            StackPanel ClickedStack = sender as StackPanel;

            if(ClickedStack != null)
            {
                var a = ClickedStack.Children.Count;
            }

        }

        private void testChildren()
        {

            foreach(var item in WhitePawnsList)
            {
                StackPanel a = item.Parent as StackPanel;
                var b = a.Name;
            }


        }

    }
}
