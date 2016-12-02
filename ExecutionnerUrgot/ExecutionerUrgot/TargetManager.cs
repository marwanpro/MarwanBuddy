using System.Linq;
using EloBuddy;
using EloBuddy.SDK;

namespace ExecutionerUrgot
{
    internal class TargetManager
    {
        // Clone Character Object
        public static AIHeroClient Champion = Program.Champion;

        // Target Selectors
        public static AIHeroClient GetChampionTarget(float range, DamageType damagetype, bool isAlly = false, bool collision = false, float ksdamage = -1f)
        {
            var herotype = EntityManager.Heroes.AllHeroes;
            var target = herotype
                .OrderBy(a => a.HealthPercent).ThenBy(a => a.HasBuff("urgotcorrosivedebuff")) 
                .Where(a => WithinRange(a, range) && IsTargetValid(a)
                                && IsFriendOrFoe(a, isAlly)
                                && IsColliding(a, collision, range) && CalculateKs(a, damagetype, ksdamage));
            return TargetSelector.GetTarget(target, damagetype);
        }

        public static Obj_AI_Minion GetMinionTarget(float range, DamageType damagetype, bool isAlly = false, bool isMonster = false, bool collision = false, float ksdamage = -1)
        {
            var teamtype = EntityManager.UnitTeam.Enemy;
            if (isAlly)
                teamtype = EntityManager.UnitTeam.Ally;
            var miniontype = EntityManager.MinionsAndMonsters.GetLaneMinions(teamtype, Champion.ServerPosition, range).ToArray();
            if (isMonster)
                miniontype = EntityManager.MinionsAndMonsters.GetJungleMonsters(Champion.ServerPosition, range).ToArray();

            // Check list objects
            if (miniontype.Length == 0) return null;

            var target = miniontype
                .OrderBy(a => a.HealthPercent).ThenBy(a => a.HasBuff("urgotcorrosivedebuff"))
                .FirstOrDefault(a => WithinRange(a, range) && IsTargetValid(a)
                                && IsFriendOrFoe(a, isAlly) && IsMonsterOrMinion(a, isMonster)
                                && IsColliding(a, collision, range) && CalculateKs(a, damagetype, ksdamage));
            return target;
        }

        public static Obj_AI_Turret GetTurretTarget(float range, bool isAlly = false)
        {
            var turrettype = EntityManager.Turrets.AllTurrets;
            var target = turrettype
                .OrderBy(a => a.HealthPercent)
                .FirstOrDefault(a => WithinRange(a, range) && IsTargetValid(a) && IsFriendOrFoe(a, isAlly));
            return target;
        }


        // Secondary Checks
        public static bool BuffStatus(Obj_AI_Base target)
        {
            return !target.Buffs.Any(a => a.IsValid()
                                          && a.DisplayName == "Chrono Shift"
                                          && a.DisplayName == "FioraW"
                                          && a.Type == BuffType.SpellShield);
        }

        public static bool IsTargetValid(Obj_AI_Base target)
        {
            return !target.IsDead && !target.IsZombie && !Champion.IsRecalling() && BuffStatus(target);
        }

        public static bool IsFriendOrFoe(Obj_AI_Base target, bool check)
        {
            return (check && target.IsAlly && !target.IsMe) || (!check && target.IsEnemy);
        }

        public static bool IsMonsterOrMinion(Obj_AI_Base target, bool check)
        {
            return ((check && target.IsMonster) || (!check && !target.IsMonster));
        }

        public static bool WithinRange(Obj_AI_Base target, float range)
        {
            return ((range == SpellManager.Q.Range && !target.HasBuff("urgotcorrosivedebuff") && target.IsValidTarget(range))
                || (range == SpellManager.Q.Range && target.HasBuff("urgotcorrosivedebuff") && target.IsValidTarget(SpellManager.Q2.Range))
                || (range != SpellManager.Q.Range && target.IsValidTarget(range)))
                && ((range == SpellManager.Q.Range && !target.HasBuff("urgotcorrosivedebuff") && target.IsInRange(Champion, range))
                || (range == SpellManager.Q.Range && target.HasBuff("urgotcorrosivedebuff") && target.IsInRange(Champion, SpellManager.Q2.Range))
                || (range != SpellManager.Q.Range && target.IsInRange(Champion, range)));
        }

        public static bool CollisionCheck(Obj_AI_Base target, float range)
        {
            return target != null && Prediction.Position.Collision.LinearMissileCollision(target, Champion.Position.To2D(), target.Position.To2D().Extend(target, range), 1600, 60, 125);
        }

        public static bool IsColliding(Obj_AI_Base target, bool check, float range)
        {
            return ((check && !target.HasBuff("urgotcorrosivedebuff") && CollisionCheck(target, range))
                || (check && target.HasBuff("urgotcorrosivedebuff"))
                || !check);
        }

        public static bool CalculateKs(Obj_AI_Base target, DamageType damagetype, float damage)
        {
            return (damage > -1f && target.Health <= Champion.CalculateDamageOnUnit(target, damagetype, damage)) || damage == -1;
        }
    }
}