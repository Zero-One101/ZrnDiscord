using Discord.WebSocket;

namespace ZrnDiscord
{
    public interface IScript
    {
        string[] Commands { get; }

        void ScriptInit();
        bool Execute(SocketMessage msg, string cmd, string args);
        void ScriptShutdown();
    }
}
