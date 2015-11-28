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
        public static Menu Menu, GeneralMenu, DrawMenu, ComboMenu, HarassMenu, FarmMenu;
        public static AIHeroClient Target = null;
        public static AIHeroClient Player { get { return ObjectManager.Player; } }

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

            GeneralMenu = Menu.AddSubMenu("General Option", "MenuGeneral");
            GeneralMenu.AddGroupLabel("General Tweak - Configure main options");
            {
                //GeneralMenu.Add("autouseE", new CheckBox("Spam E on Poisonned Target"));
                GeneralMenu.Add("useEifkillable", new CheckBox("Use E without poison if killable"));
            }

            DrawMenu = Menu.AddSubMenu("Draw", "MenuDraw");
            DrawMenu.AddGroupLabel("Draw - Show range circle arround you and other information");
            Menu.AddSeparator();
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

            ComboMenu = Menu.AddSubMenu("Combo", "MenuCombo");
            ComboMenu.AddGroupLabel("Combo");
            Menu.AddSeparator();
            {
                ComboMenu.Add("comboQ", new CheckBox("Use Q"));
                ComboMenu.Add("comboW", new CheckBox("Use W"));
                ComboMenu.Add("comboE", new CheckBox("Use E"));
            }
            
            /*
            HarassMenu = Menu.AddSubMenu("Harass", "MenuHarass");
            HarassMenu.AddGroupLabel("Harass - Harass is an Automatic Toggle");
            Menu.AddSeparator();
            {
                HarassMenu.Add("testa", new CheckBox("Test A"));
            } */

            /*
            FarmMenu = Menu.AddSubMenu("Farm", "MenuFarm");
            FarmMenu.AddGroupLabel("Farm");
            Menu.AddSeparator();
            {
                FarmMenu.Add("testd", new Slider("Slider", 10, 1, 20));
            } */

        }

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
                        if (Player.Spellbook.GetSpell(SpellSlot.E).Cooldown <= 500000)
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

        static void OnTick(EventArgs args)
        {
            if (Player.IsDead) return;
            Target = TargetSelector.GetTarget(1300, DamageType.Magical);
            if (Target != null && Target.IsValidTarget())
            {
                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
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
                    if (ComboMenu["comboW"].Cast<CheckBox>().CurrentValue && W.IsReady() && Target.IsValidTarget(W.Range) && (!IsPoisoned(Target) || Target.Distance(_Player) < W.Range - 150 || !Q.IsReady()))
                    {
                        if (predictionW.HitChancePercent >= 50)
                        {
                            W.Cast(predictionW.CastPosition);
                        }
                    }

                    if (ComboMenu["comboE"].Cast<CheckBox>().CurrentValue && E.IsReady() &&
                        Target.IsValidTarget(E.Range) && IsPoisoned(Target))
                    {
                        E.Cast(Target);
                    }
                }
            }

        }
        public static AIHeroClient _Player
        {
            get { return ObjectManager.Player; }
        }
        public static bool IsPoisoned(Obj_AI_Base target)
        {
            return target.Buffs.Where(o => o.IsValid()).Any(buff => buff.DisplayName.Contains("Cassiopeia"));
        }
        //=========== End
    }
}
