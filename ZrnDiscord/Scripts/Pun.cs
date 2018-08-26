using System;
using System.IO;
using System.Linq;
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
            Console.WriteLine("There are {0} puns", puns.Length);
        }

        public bool Execute(SocketMessage msg, string cmd, string args)
        {
            Console.WriteLine("Executing pun script");
            Console.WriteLine("msg: {0}\tcmd: {1}\targs: {2}", msg.ToString(), cmd, args);
            if (Commands.Contains(cmd))
            {
                Console.WriteLine("Found command");
                if (args != null)
                {
                    Console.WriteLine("args are null");
                    int result;
                    if (Int32.TryParse(args, out result))
                    {
                        Console.WriteLine("Got a number: {0}", result);
                        if (!(result < 1) && !(result > puns.Length))
                        {
                            msg.Channel.SendMessageAsync(puns[result - 1]);
                            return true;
                        }
                    }
                    Console.WriteLine("Invalid number");
                    msg.Channel.SendMessageAsync("Invalid number! Number must be between 1 and " + puns.Length + ".\r\nGrabbing random pun");
                }

                Console.WriteLine("Outputting pun");
                msg.Channel.SendMessageAsync(puns[new Random().Next(puns.Length)]);
                return true;
            }

            return false;
        }

        public void ScriptShutdown() { }
    }
}
