using System.Linq;
using System.Threading.Tasks;
using Discord.WebSocket;

namespace ZrnDiscord.Scripts
{
    class Say : IScript
    {
        public string[] Commands => new[] { "say" };

        public void ScriptInit() { }

        public async Task<bool> Execute(SocketMessage msg, string cmd, string args)
        {
            if (Commands.Contains(cmd))
            {
                if (args != null)
                {
                    await msg.Channel.SendMessageAsync(args);
                    return true;
                }

                await msg.Channel.SendMessageAsync("You didn't say anything!");
                return true;
            }

            return false;
        }

        public void ScriptShutdown() { }
    }
}
