using System.Linq;
using Discord.WebSocket;

namespace ZrnDiscord.Scripts
{
    class Ping : IScript
    {
        public string[] Commands => new[] { "ping" };

        public void ScriptInit() { }

        public bool Execute(SocketMessage msg, string cmd, string args)
        {
            if (Commands.Contains(cmd))
            {
                msg.Channel.SendMessageAsync("Pong!");
                return true;
            }

            return false;
        }

        public void ScriptShutdown() { }
    }
}
