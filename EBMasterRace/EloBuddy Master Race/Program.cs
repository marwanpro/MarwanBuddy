using System;
using LeagueSharp;
using LeagueSharp.Common;

namespace EloBuddy_Master_Race
{
    internal class Program
    {
        public static Obj_AI_Base Player = ObjectManager.Player;
        public static DateTime StartTime = DateTime.UtcNow;
        public static int LastCheckTime;
        public static int SpamInterval = 35;

        private static void Main(string[] args)
        {
            Game.OnUpdate += Game_OnGameUpdate;
        }

        private static void Game_OnGameUpdate(EventArgs args)
        {
            if (LastCheckTime + SpamInterval < CurrentTime())
            {
                LastCheckTime = CurrentTime();
                Game.Say("/all Download your best script at EloBuddy.NET");
                Game.Say("/all Dodge every skillshot, carry your ranked !");
                Game.Say("/all No virus, 100% Safe, Undetectable");
                Game.Say("http://elobuddy.net");
            }
        }

        public static int CurrentTime()
        {
            var timeSpan = (DateTime.UtcNow - StartTime);
            return (int)timeSpan.TotalSeconds;
        }
    }
}
