using CSScriptLibrary;
using Discord;
using Discord.WebSocket;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ZrnDiscord
{
    class Program
    {
        private DiscordSocketClient client;
        private string[] scripts;

        static void Main(string[] args) => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            scripts = Directory.GetFiles("Scripts");

            foreach (var file in scripts)
            {
                var x = CSScript.Evaluator.LoadFile<IScript>(file);
                x.ScriptInit();
            }

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
                var msg = message.ToString().Substring(1);
                var cmd = msg.Split(' ')[0];
                string args;
                try
                {
                    args = msg.Split(new[] { ' ' }, 2)[1];
                }
                catch (IndexOutOfRangeException)
                {
                    args = null;
                }

                await HandleCommand(message, cmd, args);
            }
        }

        private async Task HandleCommand(SocketMessage message, String cmd, String args)
        {
            foreach (var script in scripts)
            {
                var x = CSScript.Evaluator.LoadFile<IScript>(script);
                if (x.Execute(message, cmd, args))
                {
                    break;
                }
            }
        }
    }
}
