using System.Diagnostics;
using System.Collections.Generic;
using EloBuddy;


namespace AutoQueue_Reloader
{
    internal class AQR
    {
        public static string BotPath = getBotPath();
        public static Dictionary<GameEventId, int> Events;

        public static void Init()
        {
            setupEvents();
            Chat.OnInput += OnCommand;
            Game.OnNotify += Game_OnGameNotifyEvent; //Thanks to reversesh3ll for GameEnd Event
        }

        public static void OnCommand(ChatInputEventArgs args)
        {
            if (args.Input.ToLower().Equals("/aqr") || args.Input.ToLower().StartsWith("/aqr help"))
            {
                Chat.Print("/aqr pid - Get PID");
                Chat.Print("/aqr kill bot - Kill the AutoQueuer");
                Chat.Print("/aqr kill lol - Kill LoL");
                Chat.Print("/aqr kill all - Kill the Bot and LoL");
                Chat.Print("/aqr start - Launch Bot");
                Chat.Print("/aqr reload - Kill all then relaunch the Bot");
                args.Process = false;
            }
            else if (args.Input.ToLower().StartsWith("/aqr pid"))
            {
                Chat.Print("League Of Legends PID: " + getLoLPID());
                Chat.Print("AutoQueuer PID: " + getBotPID());
                Chat.Print("Path: " + getBotPath());
                args.Process = false;
            }
            else if (args.Input.ToLower().StartsWith("/aqr kill bot"))
            {
                killBot();
                Chat.Print("Request sent");
                args.Process = false;
            }
            else if (args.Input.ToLower().StartsWith("/aqr kill lol"))
            {
                killLoL();
                Chat.Print("Request sent");
                args.Process = false;
            }
            else if (args.Input.ToLower().StartsWith("/aqr kill all"))
            {
                killBot();
                Chat.Print("Request sent");
                killLoL();
                args.Process = false;
            }
            else if (args.Input.ToLower().StartsWith("/aqr start"))
            {
                startBot();
                Chat.Print("Request sent");
                args.Process = false;
            }
            else if (args.Input.ToLower().StartsWith("/aqr reload"))
            {
                reload();
                Chat.Print("Request sent");
                args.Process = false;
            }

        }

        public static void Game_OnGameNotifyEvent(GameNotifyEventArgs args)
        {
            if (Events.ContainsKey(args.EventId))
            {
                if (args.EventId.ToString() == "OnHQDie" || args.EventId.ToString() == "OnHQKill")
                {
                    System.Threading.Thread.Sleep(15000);
                    reload();
                }
            }
        }

        static void setupEvents()
        {
            Events = new Dictionary<GameEventId, int>
            {
                { GameEventId.OnHQDie, 1 },
                { GameEventId.OnHQKill, 1 },
            };
        }

        private static void killLoL()
        {
            Process lol = Process.GetProcessById(getLoLPID());
            lol.Kill();
            Chat.Print("LoL Killed");
        }

        private static void killBot()
        {
            Process bot = Process.GetProcessById(getBotPID());
            bot.Kill();
            Chat.Print("Bot Killed");
        }

        private static void startBot()
        {
            Process sbot = Process.Start(BotPath);
        }

        private static void reload()
        {
            killBot();
            System.Threading.Thread.Sleep(150);
            startBot();
            System.Threading.Thread.Sleep(50);
            killLoL();
        }

        public static int getLoLPID()
        {
            Process[] lol = Process.GetProcessesByName("League Of Legends");
            Process plol = lol[0];
            int lolpid = plol.Id;
            return lolpid;
        }

        public static int getBotPID()
        {
            Process[] bot = Process.GetProcessesByName("Bot");
            Process pbot = bot[0];
            int botpid = pbot.Id;
            return botpid;
        }

        public static string getBotPath()
        {
            Process bot = Process.GetProcessById(getBotPID());
            string botpath = bot.MainModule.FileName;
            return botpath;
        }

    }
}
