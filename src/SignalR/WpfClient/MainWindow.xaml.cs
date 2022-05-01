using System.Windows;
using Microsoft.AspNetCore.SignalR.Client;
using System.Threading.Tasks;
using System;

namespace WpfClient
{
    public partial class MainWindow : Window
    {
        HubConnection hubConnection;
        public MainWindow()
        {
            hubConnection = new HubConnectionBuilder().WithUrl("https://localhost:7177/chathub").WithAutomaticReconnect().Build();

            hubConnection.Reconnecting += (sender) =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    var message = "Attempting to Recconect";
                    Messages.Items.Add(message);
                });
                return Task.CompletedTask;
            };

            hubConnection.Reconnected += (sender) =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    var message = "Recconected to Server";
                    Messages.Items.Clear();
                    Messages.Items.Add(message);
                });
                return Task.CompletedTask;
            };

            hubConnection.Closed += (sender) =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    var message = "Connection Closed";
                    Messages.Items.Add(message);
                    Connected.IsEnabled=true;
                   SendMessage.IsEnabled = false;
                });
                return Task.CompletedTask;
            };
        }

        private async void Connected_Click(object sender, RoutedEventArgs e)
        {
            hubConnection.On<string, string>("ReciveMessage", (user, message) =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    var formattedmessage = $"{user} : {message}";
                    Messages.Items.Add(formattedmessage);
                });
            });

            try
            {
                await hubConnection.StartAsync();
                Messages.Items.Add("connection started");
                Connected.IsEnabled=false;
                SendMessage.IsEnabled=true;
            }
            catch (Exception ex)
            {

                Messages.Items.Add(ex.Message);
            }
        }

        private void SendMessage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                hubConnection.InvokeAsync("SendMessage", "WPF Client", MessageInput.Text);
            }
            catch (Exception ex)
            {

                Messages.Items.Add(ex.Message);
            }
        }
    }
}
