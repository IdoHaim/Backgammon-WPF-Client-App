using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;
using WPF_Client.Chat;
using WPF_Client.Game;
using WPF_Client.Game.MVVM.Model;
using WPF_Client.Game.MVVM.Model.Interface;
using WPF_Client.Models;

namespace WPF_Client.Services
{
    public class OnlineService
    {
        public string UserName { get; private set; }

        //Connection
        HubConnection connection;

        //Lists
        List<ChatService> ChatsList;
        List<OnlineGameService> GamesList;
        public UsersListModel UsersLists { get; private set; }

        //Connection state
        public HubConnectionState ConnectionState { get; private set; }


        //Events
        public event EventHandler<HubConnectionState> StatusEvent;
        public event EventHandler<string> SignUpEvent;
        public event EventHandler<string> LogInEvent;
        public event EventHandler<MessageModel> ReceiveMessageEvent;
        public event EventHandler<UsersListModel> UsersListReceivedEvent;
        public event EventHandler<UserUpdateModel> UsersListUpdateEvent;
        public event EventHandler<InvitationModel> ReceiveInvitationEvent;
        //public event EventHandler<InvitationModel> ConfirmInvitationEvent;
        public event EventHandler<string> InvitationGotNoticedEvent;
        public event EventHandler<int> GotDiceResultEvent;
        public OnlineService()
        {
            UserName = "Guest";
            ConnectionBuilder();

            UsersLists = new UsersListModel(new List<UserModel>(), new List<UserModel>());
            ChatsList = new List<ChatService>();
            GamesList = new List<OnlineGameService>();

        }

        //Logging
        public async void SignUp(string name, string password)
        {
            try
            {
                await connection.InvokeAsync("SignUp", name, password);
            }
            catch { }

        }

        public async void LogIn(string name, string password)
        {
            if (connection == null)
            {
                OpenConnection();
            }

            try
            {
                connection.InvokeAsync("LogIn", name, password);
            }
            catch
            {
                return;
            }

            UserName = name;

        }



        // Game
        public async void FirstRollDice(Guid gameId)
        {
            try
            {
                await connection.InvokeAsync("FirstRollDice",gameId);
            }
            catch { }
        }
        public async void RollDice(Guid gameId)
        {
            try
            {
                await connection.InvokeAsync("RollDice",gameId);
            }
            catch { }
        }
        public async void Move(Guid gameId,string move)
        {
            try
            {
                await connection.InvokeAsync("Move", gameId);
            }
            catch { }
        }
        public async void Quite(Guid gameId)
        {
            try
            {
                await connection.InvokeAsync("Q", gameId);
            }
            catch { }
            GamesList.RemoveAll(g => g.GameId == gameId);
        }


        // Chat
        public async void SendMessage(ChatService sender, string message)
        {
            try
            {
                await connection.InvokeAsync("SendMessage", sender.UserToChatWith.Username, message);

            }
            catch { }

        }
        public void OpenNewChat(string targetname)
        {
            UserModel user = UsersLists.OnlineUsersList.Where(u => u.Username == targetname).FirstOrDefault();
            if (user != null)
            {
                ChatService newChat = new ChatService(ChatsList, user);
                Application.Current.Dispatcher.Invoke(() =>
                {
                    ChatWindow chatWindow = new ChatWindow(newChat);
                    chatWindow.Show();
                });
            }
        }

        // invitations
        public async void SendInvetation(string targetname)
        {
            UserModel user = UsersLists.OnlineUsersList.Where(u => u.Username == targetname).FirstOrDefault();
            if (user != null)
            {
                try
                {
                    await connection.InvokeAsync("SendInvetation", user.Username);
                }
                catch { }
            }
        }

        public async void ConfirmInvetation(InvitationModel answer)
        {
            string answerStr = string.Empty;
            if (answer.IsAccepted) answerStr = "Yes";
            else answerStr = "No";
            try
            {
                await connection.InvokeAsync("ConfirmInvetation", answer.TargetUser.Username, answerStr);
            }
            catch { }
        }

        // System functions
        public void InvitationNoticedAction()
        {
            InvitationGotNoticedEvent?.Invoke(this, "Noticed");
        }


        // Connection Building
        public async void OpenConnection()
        {
            // for messages
            connection.On<string, string>("ReceiveMessage", (sender, message) =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                    {
                        MessageModel messageModel = new MessageModel(
                                sender,
                                message
                            );


                        ChatService chat = ChatsList.Where(chat => chat.UserToChatWith.Username == sender).FirstOrDefault();

                        UserModel user = UsersLists.OnlineUsersList.Where(u => u.Username == sender).FirstOrDefault();

                        if (chat == null)
                        {
                            chat = new ChatService(ChatsList, user);
                            ChatWindow chatWindow = new ChatWindow(chat);
                            chatWindow.Show();
                        }

                        chat.MessageRecived(message);

                    });
            });

            // for notifications from the server
            connection.On("ReceiveNotificationSignUp", (Action<string>)((message) =>
            {
                Application.Current.Dispatcher.Invoke((Action)(() =>
                {
                    SignUpEvent?.Invoke((object)this, message);
                }));
            }));
            connection.On("ReceiveNotificationLogIn", (Func<string, Task>)(async (message) =>
            {

                LogInEvent?.Invoke((object)this, message);

                if (message == "Ok")
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        ConnectionState = connection.State;
                        StatusEvent?.Invoke(this, connection.State);
                    });

                    await connection.InvokeAsync("GetUsersLists");

                }

            }));
            connection.On<string, string>("ReceiveNotification", (user, message) =>
            {
                // this part will activate if unrecognized user will try to use
                // tasks that are not sign up or log in
                Application.Current.Dispatcher.Invoke(() =>
                {

                });

            });

            // for invitations
            connection.On<string>("ReceiveInvitation", (sender) =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    InvitationModel invitationModel = new InvitationModel(
                        new UserModel() { Username = sender }, false, DateTime.Now.ToString("HH:mm"));
                    ReceiveInvitationEvent?.Invoke(this, invitationModel);
                });
            });
            connection.On<string, string, Guid>("ConfirmInvitation", (sender, answer, gameId) =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    // The user that send the invitation is the white pawns
                    IPawnState pawnState;
                    if (answer == "Yes") pawnState = new WhitePawn();
                    else if (answer == "Yes2") pawnState = new BlackPawn();
                    else return;   

                    OnlineGameService ?gameService = GamesList.Find(game => game.UserToPlayWith.Username == sender);

                    UserModel ?user = UsersLists.OnlineUsersList.Where(u => u.Username == sender).FirstOrDefault();

                    if (gameService == null&& user != null)
                    {
                        gameService = new OnlineGameService(gameId,user);
                        GameStartModel gameStartModel = new GameStartModel(gameService,pawnState);
                        GameMainWindow gameMainWindow = new GameMainWindow(gameStartModel);
                        gameStartModel.SetViews();
                        GamesList.Add(gameService);
                        gameMainWindow.Show();
                    }
                });
            });



            // for the users lists
            connection.On<string, string>("GetLists", (onlineUsers, offlineUsers) =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {

                    List<UserModel> onlineuserslist;
                    List<UserModel> offlineuserslist;
                    try
                    {
                        onlineuserslist = JsonConvert.DeserializeObject<List<UserModel>>(onlineUsers);
                        offlineuserslist = JsonConvert.DeserializeObject<List<UserModel>>(offlineUsers);
                    }
                    catch (Exception ex)
                    {
                        onlineuserslist = new List<UserModel>();
                        offlineuserslist = new List<UserModel>();
                    }

                    UsersListModel usersListModel = new UsersListModel(
                        onlineuserslist,
                        offlineuserslist
                    );

                    UsersLists = usersListModel;
                    UsersListReceivedEvent?.Invoke(this, usersListModel);
                });

            });

            connection.On<string, string>("UpdateList", (user, state) =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {

                    bool BoolState = true;
                    if (state == "disconnected")
                    { BoolState = false; }
                    else if (state == "connected")
                    { BoolState = true; }

                    UserUpdateModel userUpdate = new UserUpdateModel(
                        user,
                        BoolState
                       );

                    UsersListUpdateEvent?.Invoke(this, userUpdate);
                });
            });

            // Game
            connection.On<Guid,string >("ReceiveGameNotification", (gameId, message) =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    OnlineGameService ?game = GamesList.Where(g => g.GameId == gameId).FirstOrDefault();
                    if(game != null)
                    {
                        if(message == "Q")
                        {
                            game.OpponentQuite();
                            GamesList.RemoveAll(g => g.GameId == gameId);
                        }
                        else if(message == "E")
                            GamesList.RemoveAll(g => g.GameId == gameId);

                    }
                });
            });
            connection.On<Guid,int>("GetYourDiceRoll", (gameId,result) =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    OnlineGameService? game = GamesList.Where(g => g.GameId == gameId).FirstOrDefault();
                    if (game != null)
                    {
                        game.GetYourRollDice(result);
                    }
                });
            });
            connection.On<Guid,int>("GetOpponentDiceRoll", (gameId, result) =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    OnlineGameService? game = GamesList.Where(g => g.GameId == gameId).FirstOrDefault();
                    if (game != null)
                    {
                        game.GetOpponentRollDice(result);
                    }
                });
            });
            connection.On<Guid, int,int>("GetDiceRoll", (gameId, num1,num2) =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    OnlineGameService? game = GamesList.Where(g => g.GameId == gameId).FirstOrDefault();
                    if (game != null)
                    {
                        game.GetRollDice(num1, num2);
                    }

                });
            });
            connection.On<Guid, string>("GetMove", (gameId,move) =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    OnlineGameService? game = GamesList.Where(g => g.GameId == gameId).FirstOrDefault();
                    if (game != null)
                    {
                        game.GetMove(move);
                    }
                });
            });




            //start the connection
            try
            {
                await connection.StartAsync();
            }
            catch { }

        }

        public async void CloseConnection()
        {
            await connection.StopAsync();
        }

        private void ConnectionBuilder()
        {
            connection = new HubConnectionBuilder()
           .WithUrl(ReadAdress() + "/hub")
           .WithAutomaticReconnect()
           .Build();


            connection.Reconnecting += (sender) =>
            {
                // "Attempting to reconnect...";
                ConnectionState = connection.State;
                StatusEvent?.Invoke(this, connection.State);

                return Task.CompletedTask;
            };

            connection.Reconnected += (sender) =>
            {
                //"Reconnected to the server";
                ConnectionState = connection.State;
                StatusEvent?.Invoke(this, connection.State);

                return Task.CompletedTask;
            };

            connection.Closed += (sender) =>
            {//TODO: empty lists and make shure that an invitation will be presented always
                // "Connection closed";
                ConnectionState = connection.State;
                StatusEvent?.Invoke(this, connection.State);
                UserName = "Guest";

                return Task.CompletedTask;
            };



        }

        private string ReadAdress()
        {
            string path = "../../../ServerAdress.txt";

            try
            {
                StreamReader reader = new StreamReader(path);
                return reader.ReadToEnd();
            }
            catch
            {
                return string.Empty;
            }
        }


    }
}
