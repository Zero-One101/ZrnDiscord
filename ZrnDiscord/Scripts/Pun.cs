using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Discord.WebSocket;

namespace ZrnDiscord.Scripts
{
    class Pun : IScript
    {
        public string[] Commands => new[] { "pun" };
        private string[] puns;

        public void ScriptInit()
        {
            puns = File.ReadAllLines(@"Puns.txt");
        }

        public async Task<bool> Execute(SocketMessage msg, string cmd, string args)
        {
            if (Commands.Contains(cmd))
            {
                if (args != null)
                {
                    Console.WriteLine("args are null");
                    int result;
                    if (Int32.TryParse(args, out result))
                    {
                        if (!(result < 1) && !(result > puns.Length))
                        {
                            await msg.Channel.SendMessageAsync(puns[result - 1]);
                            return true;
                        }
                    }
                    await msg.Channel.SendMessageAsync("Invalid number! Number must be between 1 and " + puns.Length + ".\r\nGrabbing random pun");
                }
                
                await msg.Channel.SendMessageAsync(puns[new Random().Next(puns.Length)]);
                return true;
            }

            return false;
        }

        public void ScriptShutdown() { }
    }
}
