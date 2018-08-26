using System.Linq;
using Discord.WebSocket;

namespace ZrnDiscord.Scripts
{
    class Say : IScript
    {
        public string[] Commands => new[] { "say" };

        public void ScriptInit() { }

        public bool Execute(SocketMessage msg, string cmd, string args)
        {
            if (Commands.Contains(cmd))
            {
                if (args != null)
                {
                    msg.Channel.SendMessageAsync(args);
                    return true;
                }

                msg.Channel.SendMessageAsync("You didn't say anything!");
                return true;
            }

            return false;
        }

        public void ScriptShutdown() { }
    }
}
