/*
 *  Project Name: Recall Fix (AutoBuddy)
 *  Author: marwanpro ( marwan69120@live.fr )
 *  Version: 1.2 (FINAL)
 *  Last Update: 31/01/2016 23:21 @ GMT +1 
 *  License: WTFPL v2 + Details above
 *  
 *  Additional License: This Addon is for EloBuddy Community, you can port it to any other API.
 *  Just change the name and mention it. And please, send me a mail or a PM with your port link :D
 *
 *  Description: Fix for AutoBuddy (When the bot doesn't recall)
 *  
 *  Credits: Christian Brutal Sniper, MarioGK
 *
 *  Thread Link: https://www.elobuddy.net/topic/13973-fix-autobuddy-recall-fix-by-marwanpro/ 
 *  Install Link: N/A
 *  GitHub Link: https://github.com/marwanpro/MarwanBuddy/tree/master/RecallFix
 *
 */

using System;
using System.Reflection;
using EloBuddy;
using EloBuddy.SDK.Events;
using Version = System.Version;

namespace AutoBuddy_Recall_Fix
{
    class Program
    {
        public static Version AddonVersion;

        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoadingComplete;
        }

        private static void OnLoadingComplete(EventArgs args)
        {
            AddonVersion = Assembly.GetExecutingAssembly().GetName().Version;
            Chat.Print("AutoBuddy Recall Fix injected (Marwanpro)");
            Fix.Init();
        }
    }
}