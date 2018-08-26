using CSScriptLibrary;
using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ZrnDiscord
{
    class Program
    {
        private DiscordSocketClient client;
        private List<IScript> scripts;
        private bool quit = false;

        static void Main(string[] args) => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            LoadScripts();

            client = new DiscordSocketClient();
            client.Log += Log;
            client.MessageReceived += ReceiveMessage;
            Config.ReadConfig();

            var token = Config.AuthToken;
            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();

            while (!quit)
            {
                await Task.Delay(-1);
            }

            UnloadScripts();
        }

        private void ReloadScripts()
        {
            UnloadScripts();
            LoadScripts();
        }

        private void LoadScripts()
        {
            LogInfo("Core", "Loading scripts");
            scripts = new List<IScript>();
            var files = Directory.GetFiles("Scripts");

            foreach (var file in files)
            {
                var script = CSScript.Evaluator.LoadFile<IScript>(file);
                script.ScriptInit();
                scripts.Add(script);
            }
        }

        private void UnloadScripts()
        {
            LogInfo("Core", "Unloading scripts");
            foreach (var script in scripts)
            {
                script.ScriptShutdown();
            }
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
            if (cmd == "reload")
            {
                ReloadScripts();
                return;
            }

            try
            {
                foreach (var script in scripts)
                {
                    if (await script.Execute(message, cmd, args))
                    {
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                await message.Channel.SendMessageAsync("Script execution failed! See console for details.");
            }
        }
    }
}
