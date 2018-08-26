using Discord.WebSocket;
using System.Threading.Tasks;

namespace ZrnDiscord
{
    public interface IScript
    {
        string[] Commands { get; }

        void ScriptInit();
        Task<bool> Execute(SocketMessage msg, string cmd, string args);
        void ScriptShutdown();
    }
}
