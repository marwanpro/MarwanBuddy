using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace ExecutionerUrgot
{
    internal class MenuManager
    {
        // Create Main Segments
        public static Menu ExecutionerUrgotMenu,
            ComboMenu,
            HarassMenu,
            JungleMenu,
            LaneClearMenu,
            LastHitMenu,
            KillStealMenu,
            DrawingMenu,
            SettingMenu;

        public static void Initialize()
        {
            // Addon Menu
            ExecutionerUrgotMenu = MainMenu.AddMenu("ExecutionerUrgot", "ExecutionerUrgot");
            ExecutionerUrgotMenu.AddGroupLabel("Executioner Urgot");

            // Combo Menu
            ComboMenu = ExecutionerUrgotMenu.AddSubMenu("Combo Features", "ComboFeatures");
            ComboMenu.AddGroupLabel("Combo Features");
            ComboMenu.AddLabel("Independent boxes for Spells:");
            ComboMenu.Add("Qcombo", new CheckBox("Use Q"));
            ComboMenu.Add("Wcombo", new CheckBox("Use W for slow"));
            ComboMenu.Add("Ecombo", new CheckBox("Use E"));
            ComboMenu.AddLabel("Urgot will cast R when Slider # equals # of Enemies.");
            ComboMenu.Add("Rcombo", new Slider("Use R - 0 is off", 3, 0, 5));

            // Harass Menu
            HarassMenu = ExecutionerUrgotMenu.AddSubMenu("Harass Features", "HarassFeatures");
            HarassMenu.AddGroupLabel("Harass Features");
            HarassMenu.AddLabel("Independent boxes for Spells:");
            HarassMenu.Add("Qharass", new CheckBox("Use Q"));
            HarassMenu.Add("Eharass", new CheckBox("Use E"));
            HarassMenu.AddSeparator(1);
            HarassMenu.Add("Mharass", new Slider("Mana Limiter at Mana %", 25));

            // Jungle Menu
            JungleMenu = ExecutionerUrgotMenu.AddSubMenu("Jungle Features", "JungleFeatures");
            JungleMenu.AddGroupLabel("Jungle Features");
            JungleMenu.AddLabel("Independent boxes for Spells:");
            JungleMenu.Add("Qjungle", new CheckBox("Use Q"));
            JungleMenu.Add("Ejungle", new CheckBox("Use E"));
            JungleMenu.AddSeparator(1);
            JungleMenu.Add("Mjungle", new Slider("Mana Limiter at Mana %", 25));

            // LaneClear Menu
            LaneClearMenu = ExecutionerUrgotMenu.AddSubMenu("Lane Clear Features", "LaneClearFeatures");
            LaneClearMenu.AddGroupLabel("Lane Clear Features");
            LaneClearMenu.AddLabel("Independent boxes for Spells:");
            LaneClearMenu.Add("Qlanec", new CheckBox("Use Q"));
            LaneClearMenu.Add("Elanec", new CheckBox("Use E", false));
            LaneClearMenu.AddSeparator(1);
            LaneClearMenu.Add("Mlanec", new Slider("Mana Limiter at Mana %", 25));

            // LastHit Menu
            LastHitMenu = ExecutionerUrgotMenu.AddSubMenu("Last Hit Features", "LastHitFeatures");
            LastHitMenu.AddGroupLabel("Last Hit Features");
            LastHitMenu.AddLabel("Independent boxes for Spells:");
            LastHitMenu.Add("Qlasthit", new CheckBox("Use Q"));
            LastHitMenu.AddSeparator(1);
            LastHitMenu.Add("Mlasthit", new Slider("Mana Limiter at Mana %", 25));

            // Kill Steal Menu
            KillStealMenu = ExecutionerUrgotMenu.AddSubMenu("KS Features", "KSFeatures");
            KillStealMenu.AddGroupLabel("Kill Steal Features");
            KillStealMenu.Add("Uks", new CheckBox("KS Mode"));
            KillStealMenu.AddSeparator(1);
            KillStealMenu.AddLabel("Independent boxes for Spells:");
            KillStealMenu.Add("Qks", new CheckBox("Use Q in KS"));
            KillStealMenu.Add("Eks", new CheckBox("Use E in KS", false));

            // Drawing Menu
            DrawingMenu = ExecutionerUrgotMenu.AddSubMenu("Drawing Features", "DrawingFeatures");
            DrawingMenu.AddGroupLabel("Drawing Features");
            DrawingMenu.Add("Udrawer", new CheckBox("Use Drawer"));
            DrawingMenu.AddSeparator(1);
            DrawingMenu.AddLabel("Independent boxes for Spells:");
            DrawingMenu.Add("Qdraw", new CheckBox("Draw Q"));
            DrawingMenu.Add("Edraw", new CheckBox("Draw E"));
            DrawingMenu.Add("Rdraw", new CheckBox("Draw R"));
            DrawingMenu.AddSeparator(1);
            DrawingMenu.AddLabel("Skin Designer");
            DrawingMenu.Add("Udesigner", new CheckBox("Use Designer"));
            DrawingMenu.Add("Sdesign", new Slider("Skin Designer: ", 2, 0, 3));

            // Setting Menu
            SettingMenu = ExecutionerUrgotMenu.AddSubMenu("Settings", "Settings");
            SettingMenu.AddGroupLabel("Settings");
            SettingMenu.AddLabel("Automatic Leveler");
            SettingMenu.Add("Uleveler", new CheckBox("Use Leveler"));
            SettingMenu.AddSeparator(1);
            SettingMenu.AddLabel("Automatic Tear Stacker");
            SettingMenu.Add("Ustacker", new CheckBox("Use Stacker"));
            SettingMenu.AddSeparator(1);
            SettingMenu.AddLabel("Auto R - While under ally turret, grab enemy");
            SettingMenu.Add("Ugrabber", new CheckBox("Use Grabber"));
            SettingMenu.AddSeparator(1);
            SettingMenu.AddLabel("Interrupter");
            SettingMenu.Add("Uinterrupt", new CheckBox("Use Interrupter", false));
            SettingMenu.Add("Rinterrupt", new CheckBox("Use R to interrupt", false));
            SettingMenu.AddLabel("Gap Closer");
            SettingMenu.Add("Ugapc", new CheckBox("Use Gapcloser", false));
            SettingMenu.Add("Rgapc", new CheckBox("Use R to gapclose", false));
        }

        // Assign Global Checks+
        public static bool ComboUseQ { get { return ComboMenu["Qcombo"].Cast<CheckBox>().CurrentValue; } }
        public static bool ComboUseW { get { return ComboMenu["Wcombo"].Cast<CheckBox>().CurrentValue; } }
        public static bool ComboUseE { get { return ComboMenu["Ecombo"].Cast<CheckBox>().CurrentValue; } }
        public static int ComboUseR { get { return ComboMenu["Rcombo"].Cast<Slider>().CurrentValue; } }

        public static bool HarassUseQ { get { return HarassMenu["Qharass"].Cast<CheckBox>().CurrentValue; } }
        public static bool HarassUseE { get { return HarassMenu["Eharass"].Cast<CheckBox>().CurrentValue; } }
        public static int HarassMana { get { return HarassMenu["Mharass"].Cast<Slider>().CurrentValue; } }

        public static bool JungleUseQ { get { return JungleMenu["Qjungle"].Cast<CheckBox>().CurrentValue; } }
        public static bool JungleUseE { get { return JungleMenu["Ejungle"].Cast<CheckBox>().CurrentValue; } }
        public static int JungleMana { get { return JungleMenu["Mjungle"].Cast<Slider>().CurrentValue; } }

        public static bool LaneClearUseQ { get { return LaneClearMenu["Qlanec"].Cast<CheckBox>().CurrentValue; } }
        public static bool LaneClearUseE { get { return LaneClearMenu["Elanec"].Cast<CheckBox>().CurrentValue; } }
        public static int LaneClearMana { get { return LaneClearMenu["Mlanec"].Cast<Slider>().CurrentValue; } }

        public static bool LastHitUseQ { get { return LastHitMenu["Qlasthit"].Cast<CheckBox>().CurrentValue; } }
        public static int LastHitMana { get { return LastHitMenu["Mlasthit"].Cast<Slider>().CurrentValue; } }

        public static bool KsMode { get { return KillStealMenu["Uks"].Cast<CheckBox>().CurrentValue; } }
        public static bool KsUseQ { get { return KillStealMenu["Qks"].Cast<CheckBox>().CurrentValue; } }
        public static bool KsUseE { get { return KillStealMenu["Eks"].Cast<CheckBox>().CurrentValue; } }

        public static bool DrawerMode { get { return DrawingMenu["Udrawer"].Cast<CheckBox>().CurrentValue; } }
        public static bool DrawQ { get { return DrawingMenu["Qdraw"].Cast<CheckBox>().CurrentValue; } }
        public static bool DrawE { get { return DrawingMenu["Edraw"].Cast<CheckBox>().CurrentValue; } }
        public static bool DrawR { get { return DrawingMenu["Rdraw"].Cast<CheckBox>().CurrentValue; } }
        public static bool DesignerMode { get { return DrawingMenu["Udesigner"].Cast<CheckBox>().CurrentValue; } }
        public static int DesignerSkin { get { return DrawingMenu["Sdesign"].Cast<Slider>().CurrentValue; } }

        public static bool LevelerMode { get { return SettingMenu["Uleveler"].Cast<CheckBox>().CurrentValue; } }
        public static bool StackerMode { get { return SettingMenu["Ustacker"].Cast<CheckBox>().CurrentValue; } }
        public static bool GrabberMode { get { return SettingMenu["Ugrabber"].Cast<CheckBox>().CurrentValue; } }

        public static bool InterrupterMode { get { return SettingMenu["Uinterrupt"].Cast<CheckBox>().CurrentValue; } }
        public static bool InterrupterUseR { get { return SettingMenu["Rinterrupt"].Cast<CheckBox>().CurrentValue; } }
        public static bool GapCloserMode { get { return SettingMenu["Ugapc"].Cast<CheckBox>().CurrentValue; } }
        public static bool GapCloserUseR { get { return SettingMenu["Rgapc"].Cast<CheckBox>().CurrentValue; } }
    }
}
