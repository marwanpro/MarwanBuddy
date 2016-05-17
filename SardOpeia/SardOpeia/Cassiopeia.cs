/*
 *  Project Name: SardOpeia
 *  Author: marwanpro ( marwan69120@live.fr )
 *  Version: 0.5-BETA (Revision 1)
 *  Last Update: 22/12/2015 19:18 @ GMT +1 
 *  License: WTFPL v2 + Details above
 *  
 *  Additional License: This Addon is for EloBuddy Community, you can port it to any other API.
 *  Just change the name and mention it. And please, send me a mail or a PM with your port link :D
 *
 *  Description: Cassiopeia Addon for EloBuddy with Combo, Range Drawer and more...
 *  
 *  Credits: Sardoche, Hellsing, WujuSan, Buddy, Ban, MrArticuno, WhoIAM, KeiZsT, MarioGK, HEI Cii
 *
 *  Thread Link: https://www.elobuddy.net/topic/8192-X
 *  Install Link: http://elobuddydb.com/link/269?github
 *  GitHub Link: https://github.com/marwanpro/MarwanBuddy/tree/master/SardOpeia
 *
 */

using System;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Text.RegularExpressions;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Rendering;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using SharpDX;
using Color = System.Drawing.Color;
namespace SardOpeia
{
    class Cassiopeia
    {
        public static Spell.Skillshot Q;
        public static Spell.Skillshot W;
        public static Spell.Targeted E;
        public static Spell.Skillshot R;
        public static Menu Menu, GeneralMenu, DrawMenu, ComboMenu, HarassMenu, FarmMenu, UltimateMenu;
        public static AIHeroClient Target = null;
        public static AIHeroClient Player = ObjectManager.Player;
        public static Vector2 Player2D = new Vector2();

        public static void Init()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 850, SkillShotType.Circular, 400, null, 75);
            W = new Spell.Skillshot(SpellSlot.W, 850, SkillShotType.Circular, 250, null, 125);
            E = new Spell.Targeted(SpellSlot.E, 700);
            R = new Spell.Skillshot(SpellSlot.R, 825, SkillShotType.Cone, 600, null,(int)((80*Math.PI)/180));

            ConfigInitialize();
            Game.OnTick += OnTick;
            Drawing.OnDraw += OnDraw;
            
        }

        public static void ConfigInitialize()
        {
            
            Menu = MainMenu.AddMenu("SardOpeia", "SardOpeia");
            Menu.AddGroupLabel("SardOpeia v" + Program.AddonVersion + " by Marwanpro");
            Menu.AddSeparator();
            Menu.AddLabel("If you have any problem/bug/suggestion, post in forum");
            Menu.AddLabel("Have a fun (And Free ELO) with this addon !");

            // --- General Menu Todo: Fix and add missings features
            GeneralMenu = Menu.AddSubMenu("General Option", "MenuGeneral");
            GeneralMenu.AddGroupLabel("General Tweak - Configure main options");
            GeneralMenu.AddSeparator();
            {
                GeneralMenu.Add("autouseE", new CheckBox("Spam E on Poisonned Target"));
                GeneralMenu.Add("useEifkillable", new CheckBox("Use E without poison if killable"));
            }

            // --- Draw Menu Todo: Check
            DrawMenu = Menu.AddSubMenu("Draw", "MenuDraw");
            DrawMenu.AddGroupLabel("Draw - Show range circle arround you and other information");
            DrawMenu.AddSeparator();
            {
                DrawMenu.Add("drawQW", new CheckBox("Draw Q/W"));
                DrawMenu.Add("drawE", new CheckBox("Draw E Range"));
                DrawMenu.Add("drawR", new CheckBox("Draw Ultimate"));
                DrawMenu.AddSeparator();
                DrawMenu.AddLabel("Green Circle: Spell Ready");
                DrawMenu.AddLabel("Red Circle: Spell in Cooldown");
                DrawMenu.AddLabel("Purple Circle (Q/W): Only Q or W ready");
                DrawMenu.AddLabel("Yellow Circle (E): 0.5 sec of Cooldown");
                DrawMenu.AddSeparator();
                DrawMenu.AddLabel("Don't Forget to disable AA circle in OrbWalker's settings");
            }

            // --- Combo Menu Todo : Add Delayer
            ComboMenu = Menu.AddSubMenu("Combo", "MenuCombo");
            ComboMenu.AddGroupLabel("Combo");
            ComboMenu.AddSeparator();
            {
                ComboMenu.Add("comboQ", new CheckBox("Use Q"));
                ComboMenu.Add("comboW", new CheckBox("Use W"));
                ComboMenu.Add("comboE", new CheckBox("Use E"));
            }
            
            // --- Harass Menu Todo: Make it
            HarassMenu = Menu.AddSubMenu("Harass", "MenuHarass");
            HarassMenu.AddGroupLabel("Harass - Auto Harass");
            HarassMenu.AddSeparator();
            {
                HarassMenu.Add("testa", new CheckBox("Test A"));
            } 

            // --- Farm Menu Todo: Improve it
            FarmMenu = Menu.AddSubMenu("Farm", "MenuFarm");
            FarmMenu.AddGroupLabel("Farm");
            FarmMenu.AddSeparator();
            {
                FarmMenu.Add("EtoFinishCreep", new CheckBox("E on Poisonned Creep (LastHit Mode)"));
            }

            // --- Ultimate Menu Todo: LOL
            UltimateMenu = Menu.AddSubMenu("Ultimate", "MenuUltimate");
            UltimateMenu.AddGroupLabel("Ultimate Logic");
            UltimateMenu.AddSeparator();
            {
                UltimateMenu.AddLabel("Enable this option to allow this script to use your Ultimate");
                UltimateMenu.Add("enableR", new CheckBox("Enable Ultimate Logic", false));
                UltimateMenu.AddSeparator();
                UltimateMenu.Add("RonGapcloser", new CheckBox("Use R on GapCloser", false));
                UltimateMenu.Add("RtoInterrupt", new CheckBox("Use R to Interrupt", false));
                UltimateMenu.Add("RonCombo", new CheckBox("Use R on Combo", false));
                UltimateMenu.Add("FaceToUlt", new Slider("Minimum number of facing ennemies to use Ultimate", 3, 1, 5));
            }
        }

        // === Range Drawer ===//
        static void OnDraw(EventArgs args)
        {
            if (!Player.IsDead)
            {
                if (DrawMenu["drawQW"].Cast<CheckBox>().CurrentValue)
                {
                    if (Q.IsReady() && W.IsReady())
                    {
                        Drawing.DrawCircle(Player.Position, Q.Range, Color.Green);
                    }
                    else
                    {
                        if (Q.IsReady() || W.IsReady())
                        {
                            Drawing.DrawCircle(Player.Position, Q.Range, Color.Purple);
                        }
                        else
                        {
                            Drawing.DrawCircle(Player.Position, Q.Range, Color.Red);
                        }
                    }
                }

                if (DrawMenu["drawE"].Cast<CheckBox>().CurrentValue && E.IsLearned)
                {
                    if (E.IsReady())
                    {
                        Drawing.DrawCircle(Player.Position, E.Range, Color.Green);
                    }
                    else
                    {
                        if (E.Handle.SData.CooldownTime <= 0.5) // Much kudos to MarioGK
                        {
                            Drawing.DrawCircle(Player.Position, E.Range, Color.Yellow);
                        }
                        else
                        {
                            Drawing.DrawCircle(Player.Position, E.Range, Color.Red);
                        }
                    }
                    
                }
                if (DrawMenu["drawR"].Cast<CheckBox>().CurrentValue == true && R.IsLearned)
                {
                    Drawing.DrawCircle(Player.Position, R.Range, R.IsReady() ? Color.Green : Color.Red);
                }
            }
            return;
        }

        // === Main Void Loop === //
        static void OnTick(EventArgs args)
        {
            if (Player.IsDead) return;
            Target = TargetSelector.GetTarget(1300, DamageType.Magical);
            Player2D = Player.ServerPosition.To2D();
            //Chat.Print(E.Handle.CooldownExpires);
            if (Target != null && Target.IsValidTarget() && Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
               Combo();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit))
            {
                LastHit();
            }
            /*if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
            {
                LaneClear();
            }*/
        }

        // === Combo === //
        static void Combo()
        {
            var predictionQ = Q.GetPrediction(Target);
            if (ComboMenu["comboQ"].Cast<CheckBox>().CurrentValue && Q.IsReady() &&
                Target.IsValidTarget(Q.Range))
            {
                if (predictionQ.HitChancePercent >= 50)
                {
                    Q.Cast(predictionQ.CastPosition);
                }
            }

            var predictionW = W.GetPrediction(Target);
            if (ComboMenu["comboW"].Cast<CheckBox>().CurrentValue && W.IsReady() && Target.IsValidTarget(W.Range) && (!IsPoisoned(Target) || Target.Distance(Player) < W.Range - 150 || !Q.IsReady()))
            {
                if (predictionW.HitChancePercent >= 50)
                {
                    W.Cast(predictionW.CastPosition);
                }
            }
            if ((ComboMenu["comboE"].Cast<CheckBox>().CurrentValue && E.IsReady() && Target.IsValidTarget(E.Range) && IsPoisoned(Target)))
            {
                E.Cast(Target);
            }
        }

        // === LastHit === //
        static void LastHit()
        {
            var Eminion =
                 EntityManager.MinionsAndMonsters.GetLaneMinions()
                     .OrderByDescending(m => m.Distance(Player))
                     .FirstOrDefault(
                         m =>
                             m.IsValidTarget(E.Range) && m.Health <= Player.GetSpellDamage(m, SpellSlot.E) &&
                             IsPoisoned(m));
            if (Eminion != null && FarmMenu["EtoFinishCreep"].Cast<CheckBox>().CurrentValue)
            {
                E.Cast(Eminion);
            }
        }

        // === LaneClear === //
        static void LaneClear()
        {
            
        }

        // === Define Player === //
        /* public static AIHeroClient _Player
        {
            get { return ObjectManager.Player; }
        } */

        // === Check if target is poisoned === //
        /*public static bool IsPoisoned(Obj_AI_Base target)
        {
            return target.Buffs.Where(o => o.IsValid()).Any(buff => buff.DisplayName.Contains("Cassiopeia"));
        }*/
        public static bool IsPoisoned(Obj_AI_Base target)
        {
            return target.HasBuffOfType(BuffType.Poison); //Fixed by Hellsing himself :jodus:
        }
        // === Spell Damage Calculator (Based on WujuSan Addons) === //
        static float SpellDamage(Obj_AI_Base target, SpellSlot slot)
        {
            switch (slot)
            {
                case SpellSlot.Q:
                    return Damage.CalculateDamageOnUnit(Player, target, DamageType.Magical, new float[] { 75, 115, 155, 195, 235 }[Q.Level - 1] + 0.45f * Player.TotalMagicalDamage);
                case SpellSlot.W:
                    return Damage.CalculateDamageOnUnit(Player, target, DamageType.Magical, new float[] { 10, 15, 20, 25, 30 }[W.Level - 1] + 0.45f * Player.TotalMagicalDamage);
                case SpellSlot.E:
                    return Damage.CalculateDamageOnUnit(Player, target, DamageType.Magical, new float[] { 55, 80, 105, 130, 155 }[E.Level - 1] + 0.55f * Player.TotalMagicalDamage, true, true);
                case SpellSlot.R:
                    return Damage.CalculateDamageOnUnit(Player, target, DamageType.Magical, new float[] { 150, 250, 350 }[R.Level - 1] + 0.5f * Player.TotalMagicalDamage);
                default:
                    return 0;
            }
        }

        // === END === //
    }
}
