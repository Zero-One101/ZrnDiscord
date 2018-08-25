using Discord;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;

namespace ZrnDiscord
{
    class Program
    {
        private DiscordSocketClient client;

        static void Main(string[] args) => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            client = new DiscordSocketClient();
            client.Log += Log;
            client.MessageReceived += ReceiveMessage;
            Config.ReadConfig();

            var token = Config.AuthToken;
            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();

            await Task.Delay(-1);
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString(padSource: msg.Source.Length));
            return Task.CompletedTask;
        }

        private void LogInfo(String source, String msg)
        {
            var lMsg = new LogMessage(LogSeverity.Info, source, msg);
            Log(lMsg);
        }

        private void LogMessage(SocketMessage message)
        {
            var source = string.Format("#{0} <{1}>:", message.Channel, message.Author);
            LogInfo(source, message.Content);
        }

        private async Task ReceiveMessage(SocketMessage message)
        {
            LogMessage(message);
            if (message.Content.StartsWith(Config.ControlChar.ToString()) && message.Author.Id != client.CurrentUser.Id)
            {
                await HandleCommand(message);
            }
        }

        private async Task HandleCommand(SocketMessage message)
        {
            if (message.Content == Config.ControlChar + "ping")
            {
                await message.Channel.SendMessageAsync("%ping");
            }
        }
    }
}
