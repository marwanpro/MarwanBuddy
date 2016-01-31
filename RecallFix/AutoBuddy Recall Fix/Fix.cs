using System;
using EloBuddy;
using SharpDX;

namespace AutoBuddy_Recall_Fix
{
    class Fix
    {
        public static AIHeroClient Player = ObjectManager.Player;
        public static DateTime StartTime = DateTime.UtcNow;
        public static Int32 LastCheckTime;
        public static Vector3 PreviousPos = Player.Position;
        public static int CheckInterval = 15;

        public static void Init()
        {
            Game.OnTick += OnTick;
        }

        static void OnTick(EventArgs args)
        {
            if (LastCheckTime + CheckInterval < CurrentTime() && !Player.IsDead && CurrentTime() > 50)
            {
                LastCheckTime = CurrentTime();
                Vector3 currentPos = Player.Position;
                if (PreviousPos == currentPos)
                {
                    Chat.Print("Recall Requested");
                    ObjectManager.Player.Spellbook.CastSpell(SpellSlot.Recall);
                }
                PreviousPos = currentPos;
            }

        }

        public static int CurrentTime()
        {
            var timeSpan = (DateTime.UtcNow - StartTime);
            return (int)timeSpan.TotalSeconds;
        }
    }
}
