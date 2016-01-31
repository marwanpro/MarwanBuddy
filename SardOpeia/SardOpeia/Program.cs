using System;
using System.Reflection;
using EloBuddy;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Rendering;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using SharpDX;
using Version = System.Version;

namespace SardOpeia
{
    class Program
    {
        public static Version AddonVersion;
        public const string ChampName = "Cassiopeia";


        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoadingComplete;
        }

        private static void OnLoadingComplete(EventArgs args)
        {
            AddonVersion = Assembly.GetExecutingAssembly().GetName().Version;
            if (Player.Instance.ChampionName != ChampName)
            {
                Chat.Print("SardOpeia can't inject");
                Chat.Print("Error: Wrong Champion");
                return;
            }
            else
            {
                Chat.Print("SardOpeia Injected - Version "+ AddonVersion +" by Marwanpro");
                Chat.Print("Enjoy your free LP !");
                Cassiopeia.Init();
            }

        }
    }
}
