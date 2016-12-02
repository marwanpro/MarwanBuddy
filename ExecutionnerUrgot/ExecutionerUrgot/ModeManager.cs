using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;

namespace ExecutionerUrgot
{
    internal class ModeManager
    {
        // Clone Character Object
        public static AIHeroClient Champion = Program.Champion;

        public static void ComboMode()
        {
            if (MenuManager.ComboUseE)
            {
                var target = TargetManager.GetChampionTarget(SpellManager.E.Range, DamageType.Physical);
                if (target != null)
                    SpellManager.CastE(target);
            }
            if (MenuManager.ComboUseW)
            {
                var target = TargetManager.GetChampionTarget(750, DamageType.Magical);
                if (target != null && !target.IsFacing(Champion) && target.Health <= target.MaxHealth / 2)
                    SpellManager.CastW(target);
            }
            if (MenuManager.ComboUseQ)
            {
                var target = TargetManager.GetChampionTarget(SpellManager.Q.Range, DamageType.Physical, false, true);
                if (target != null)
                    SpellManager.CastQ(target);
            }
            if (MenuManager.ComboUseR != 0 &&
                Champion.CountEnemiesInRange(SpellManager.R.Range) == MenuManager.ComboUseR)
            {
                var target = TargetManager.GetChampionTarget(SpellManager.R.Range, DamageType.Magical);
                if (target != null)
                    SpellManager.CastR(target);
            }
        }

        public static void HarassMode()
        {
            if (Champion.ManaPercent <= MenuManager.HarassMana) return;
            if (MenuManager.HarassUseE)
            {
                var target = TargetManager.GetChampionTarget(SpellManager.E.Range, DamageType.Physical);
                if (target != null)
                    SpellManager.CastE(target);
            }
            if (MenuManager.HarassUseQ)
            {
                var target = TargetManager.GetChampionTarget(SpellManager.Q.Range, DamageType.Physical, false, true);
                if (target != null)
                    SpellManager.CastQ(target);
            }
        }

        public static void JungleMode()
        {
            if (Champion.ManaPercent <= MenuManager.JungleMana) return;
            if (MenuManager.JungleUseQ)
            {
                var target = TargetManager.GetMinionTarget(SpellManager.Q.Range, DamageType.Physical, false, true, true);
                if (target != null)
                    SpellManager.CastQ(target);
            }
            if (MenuManager.JungleUseE)
            {
                var target = TargetManager.GetMinionTarget(SpellManager.E.Range, DamageType.Physical, false, true, false);
                if (target != null)
                    SpellManager.CastE(target);
            }
        }

        public static void LaneClearMode()
        {
            if (Champion.ManaPercent <= MenuManager.LaneClearMana) return;
            if (MenuManager.LaneClearUseQ)
            {
                var target = TargetManager.GetMinionTarget(SpellManager.Q.Range, DamageType.Physical, false, false, true);
                if (target != null)
                    SpellManager.CastQ(target);
            }
            if (MenuManager.LaneClearUseE)
            {
                var target = TargetManager.GetMinionTarget(SpellManager.E.Range, DamageType.Physical);
                if (target != null)
                    SpellManager.CastE(target);
            }
        }

        public static void LastHitMode()
        {
            if (Champion.ManaPercent <= MenuManager.LastHitMana) return;
            if (Orbwalker.CanAutoAttack && Orbwalker.IsAutoAttacking) return;
            if (MenuManager.LastHitUseQ)
            {
                var target = TargetManager.GetMinionTarget(SpellManager.Q.Range, DamageType.Physical, false, false, true, SpellManager.QDamage());
                if (target != null)
                    SpellManager.CastQ(target);
                var jtarget = TargetManager.GetMinionTarget(SpellManager.Q.Range, DamageType.Physical, false, true, true, SpellManager.QDamage());
                if (jtarget != null)
                    SpellManager.CastQ(jtarget);
            }
        }

        public static void KsMode()
        {
            if (MenuManager.KsUseQ)
            {
                var target = TargetManager.GetChampionTarget(SpellManager.Q.Range, DamageType.Physical, false, true, SpellManager.QDamage());
                if (target != null)
                    SpellManager.CastQ(target);
            }
            if (MenuManager.KsUseE)
            {
                var target = TargetManager.GetChampionTarget(SpellManager.E.Range, DamageType.Physical, false, false, SpellManager.QDamage());
                if (target != null)
                    SpellManager.CastE(target);
            }
        }

        public static void GrabMode()
        {
            if (!Champion.IsUnderHisturret()) return;
            var target = TargetManager.GetChampionTarget(SpellManager.R.Range, DamageType.Magical);
            if (target != null)
            {
                var turret = TargetManager.GetTurretTarget(600, true);
                if (turret != null && !turret.Spellbook.IsAutoAttacking && !turret.Spellbook.IsCastingSpell)
                    SpellManager.CastR(target);
            }
        }

        public static void StackMode()
        {
            foreach (var item in Champion.InventoryItems)
            {
                if ((item.Id == ItemId.Tear_of_the_Goddess || item.Id == ItemId.Archangels_Staff ||
                     item.Id == ItemId.Manamune) && Champion.IsInShopRange())
                {
                    if ((int)(Game.Time - SpellManager.StackerStamp) >= 2)
                    {
                        SpellManager.CastQ(Champion);
                        SpellManager.StackerStamp = Game.Time;
                    }
                }
            }
        }

        public static void InterruptMode(Obj_AI_Base sender, Interrupter.InterruptableSpellEventArgs args)
        {
            if (!MenuManager.InterrupterMode) return;
            if (sender != null && MenuManager.InterrupterUseR)
            {
                var target = TargetManager.GetChampionTarget(SpellManager.R.Range, DamageType.Magical);
                if (target != null)
                    SpellManager.CastR(target);
            }
        }

        public static void GapCloserMode(Obj_AI_Base sender, Gapcloser.GapcloserEventArgs args)
        {
            if (!MenuManager.GapCloserMode) return;
            if (sender != null && MenuManager.GapCloserUseR)
            {
                var target = TargetManager.GetChampionTarget(SpellManager.R.Range, DamageType.Magical);
                if (target != null)
                    SpellManager.CastR(target);
            }
        }
    }
}
