/*
 *  Project Name: Auto Queue Reloader (AQR)
 *  Author: marwanpro ( marwan69120@live.fr )
 *  Version: 1.3 (FINAL)
 *  Last Update: 06/02/2016 17:00 @ GMT +1 
 *  License: WTFPL v2 + Details above
 *  
 *  Additional License: This Addon is for EloBuddy Community, you can port it to any other API.
 *  Just change the name and mention it. And please, send me a mail or a PM with your port link :D
 *
 *  Description: Fix when Auto Queuer (VoliBot, PazBot, ...) doesn't close LoL and launch a new game
 *  
 *  Credits: reversesh3ll, Christian Brutal Sniper
 *
 *  Thread Link: https://www.elobuddy.net/topic/14768-
 *  Install Link: N/A
 *  GitHub Link: https://github.com/marwanpro/MarwanBuddy/tree/master/AutoQueueReloader
 *
 */

using System;
using System.Reflection;
using EloBuddy;
using EloBuddy.SDK.Events;
using Version = System.Version;

namespace AutoQueue_Reloader
{
    internal class Program
    {
        public static Version AddonVersion;

        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoadingComplete;
        }

        private static void OnLoadingComplete(EventArgs args)
        {
            AddonVersion = Assembly.GetExecutingAssembly().GetName().Version;
            Chat.Print("AutoQueue Bot Reloader Injected (Marwanpro)");
            AQR.Init();
        }
    }
}
