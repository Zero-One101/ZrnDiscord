using System.Linq;
using System.Threading.Tasks;
using Discord.WebSocket;

namespace ZrnDiscord.Scripts
{
    class Ping : IScript
    {
        public string[] Commands => new[] { "ping" };

        public void ScriptInit() { }

        public async Task<bool> Execute(SocketMessage msg, string cmd, string args)
        {
            if (Commands.Contains(cmd))
            {
                await msg.Channel.SendMessageAsync("Pong!");
                return true;
            }

            return false;
        }

        public void ScriptShutdown() { }
    }
}
