using LiteDB;
using Microsoft.AspNetCore.SignalR.Client;

namespace MyChatApp
{
    public partial class MainPage : ContentPage
    {
        readonly HubConnection hubConnection;
        string? _name;
        string dbPath;

        public MainPage()
        {
            InitializeComponent();

            dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "mydatabase.db");

            InitDb();

            hubConnection = new HubConnectionBuilder()
                .WithUrl("https://mychatappserver.crizzhd.net/chatHub")
                .Build();

            hubConnection.On<string, string>("MessageReceived", (name, message) =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Label chatMessagess = new Label
                    {
                        Text = $"{Environment.NewLine}{name} says: {message}",
                        FontSize= 18
                    };

                    chatMessages.Add(chatMessagess);
                });
            });

            Task.Run( async() =>
            { 
                await hubConnection.StartAsync();  
            });
        }

        async void OnSend(object sender, EventArgs e)
        {
            try
            {
                await hubConnection.InvokeCoreAsync("SendMessage", [mychatMessage.Text, _name]);
                mychatMessage.Text = string.Empty;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        void ChangeName(object sender, EventArgs e)
        {
            using var db = new LiteDatabase(dbPath);

            var col = db.GetCollection<User>("users");

            var user = col.FindById(1);
            user.Name = nameEntry.Text;
            nameEntry.Text = string.Empty;
            NameDisplay.Text = user.Name;
            _name = user.Name;
            col.Update(user);
        }

        void InitDb()
        {
            using var db = new LiteDatabase(dbPath);

            var col = db.GetCollection<User>("users");

            if (col.Count() > 0)
            {
                var user = col.FindById(1);
                if (user != null)
                {
                    NameDisplay.Text = user.Name;
                    _name = user.Name;
                }
            }
            else
            {
                var user = new User { Id = 1, Name = "New User" };
                col.Insert(user);
                NameDisplay.Text = user.Name;
                _name = "New User";
            }
        }
    }

    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
