using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using WPF_Client.Game.MVVM.Model.Interface;
using WPF_Client.Game.MVVM.View.Components;
using WPF_Client.Models;
using WPF_Client.Models.Interface;
using WPF_Client.Services.Interface;

namespace WPF_Client.Game
{
    /// <summary>
    /// Interaction logic for GameMainWindow.xaml
    /// </summary>
    public partial class GameMainWindow : Window
    {
        public IGameStatusModel GameStatus {get;set;}
        public DispatcherTimer GameTimer { get; set;}
        private TimeSpan totalTime = TimeSpan.FromMinutes(2); 
        private TimeSpan remainingTime;
        public GameMainWindow(GameStartModel gameStatus)
        {
            InitializeComponent();
            GameStatus = gameStatus;
            GameStatus.GameWindow = this;

            remainingTime = totalTime;
            GameTimer = new DispatcherTimer();
            GameTimer.Tick += Timer_Tick;
            GameTimer.Interval = TimeSpan.FromSeconds(1);


        }

        public void ResetTimer()
        {
            GameTimer.Stop(); 
            remainingTime = totalTime; 
            GameTimer.Start(); 
        }
        private void Timer_Tick(object? sender, EventArgs e)
        {
            remainingTime = remainingTime.Subtract(TimeSpan.FromSeconds(1));

            if (remainingTime <= TimeSpan.Zero)
            {
                GameTimer.Stop();
                GameStatus.GameService.TimesUp();
            }
            else
            {
                string formattedTime = string.Format("{0:D2}:{1:D2}",
                    remainingTime.Minutes, remainingTime.Seconds);

                TimerTextBlock.Text = formattedTime;
            }
        }

        

        private void QuiteBTN_Click(object sender, RoutedEventArgs e)
        {
            PopOutMessage popOutMessage = new PopOutMessage(this);
            popOutMessage.Show();
        }
        public void QuiteConfirmed()
        {
            GameStatus.Quite();
            GameTimer.Stop();
            this.Close();
        }

        private void RollDiceBTN_Click(object sender, RoutedEventArgs e)
        {
            GameStatus.RollDice();
        }
    }
}
