using System;
using System.Linq;
using System.Threading.Tasks;
using Discord.WebSocket;

namespace ZrnDiscord.Scripts
{
    class Choose : IScript
    {
        public string[] Commands => new[] { "choose" };

        public async Task<bool> Execute(SocketMessage msg, string cmd, string args)
        {
            if (Commands.Contains(cmd))
            {
                if (args != null)
                {
                    var options = args.Split('|');
                    if (options.Length > 1)
                    {
                        await msg.Channel.SendMessageAsync(options[new Random().Next(options.Length)]);
                        return true;
                    }
                    else
                    {
                        await msg.Channel.SendMessageAsync("Only found 1 option. Split options with '|'");
                        return true;
                    }
                }
                else
                {
                    await msg.Channel.SendMessageAsync("Think you forgot something. Like *choices.*");
                    return true;
                }
            }

            return false;
        }

        public void ScriptInit() { }
        public void ScriptShutdown() { }
    }
}
