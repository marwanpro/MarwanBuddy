using System;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace ExecutionerUrgot
{
    internal class SpellManager
    {
        public static Spell.Skillshot Q { get; set; }
        public static Spell.Targeted Q2 { get; set; }
        public static Spell.Active W { get; set; }
        public static Spell.Skillshot E { get; set; }
        public static Spell.Targeted R { get; set; }

        // Clone Character Object
        public static AIHeroClient Champion = Program.Champion;

        // Tear Timestamp
        public static float StackerStamp = 0;

        public static void Initialize()
        {
            // Initialize spells
            Q = new Spell.Skillshot(SpellSlot.Q, 1000, SkillShotType.Linear, 125, 1600, 60)
            {
                MinimumHitChance = HitChance.High,
                AllowedCollisionCount = 0
            };
            Q2 = new Spell.Targeted(SpellSlot.Q, 1200);
            W = new Spell.Active(SpellSlot.W);
            E = new Spell.Skillshot(SpellSlot.E, 900, SkillShotType.Circular, 250, 1500, 210)
            {
                MinimumHitChance = HitChance.High,
                AllowedCollisionCount = int.MaxValue
            };
            R = new Spell.Targeted(SpellSlot.R, (uint) (400 + 150*Champion.Spellbook.GetSpell(SpellSlot.R).Level));
        }

        public static void ConfigSpells(EventArgs args)
        {
            R = new Spell.Targeted(SpellSlot.R, (uint)(400 + 150 * Champion.Spellbook.GetSpell(SpellSlot.R).Level));
        }

        // Champion Specified Abilities
        public static float QDamage()
        {
            return new float[] { 0, 10, 40, 70, 100, 130 }[Q.Level] + (0.85f * Champion.FlatPhysicalDamageMod);
        }

        public static float EDamage()
        {
            return new float[] { 0, 75, 130, 185, 240, 295 }[E.Level] + (0.6f * Champion.FlatPhysicalDamageMod);
        }

        // Cast Methods
        public static void CastQ(Obj_AI_Base target)
        {
            if (target == null) return;
            if (!target.HasBuff("urgotcorrosivedebuff"))
                if (Q.IsReady())
                    Q.Cast(target);
            if (target.HasBuff("urgotcorrosivedebuff"))
                if (Q2.IsReady())
                    Q2.Cast(target.Position);
        }

        public static void CastW(Obj_AI_Base target)
        {
            if (target == null) return;
            if (W.IsReady())
                W.Cast();
        }

        public static void CastE(Obj_AI_Base target)
        {
            if (target == null) return;
            if (E.IsReady())
                E.Cast(target);
        }

        public static void CastR(Obj_AI_Base target)
        {
            if (target == null) return;
            if (R.IsReady())
                R.Cast(target);
        }
    }
}
