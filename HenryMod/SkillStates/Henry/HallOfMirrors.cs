using EntityStates;
using RoR2.Projectile;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace DuskWing.SkillStates.Henry
{
    internal class HallOfMirrors : BaseState
    {
        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = HallOfMirrors.baseDuration / this.attackSpeedStat;
            if (base.isAuthority)
            {
                Ray aimRay = base.GetAimRay();
                FireProjectileInfo fireProjectileInfo = new FireProjectileInfo
                {
                    crit = base.RollCrit(),
                    damage = this.damageStat * HallOfMirrors.damageCoefficient,
                    damageColorIndex = DamageColorIndex.Default,
                    force = 0f,
                    owner = base.gameObject,
                    position = aimRay.origin,
                    procChainMask = default(ProcChainMask),
                    projectilePrefab = HallOfMirrors.projectilePrefab,
                    rotation = Quaternion.LookRotation(aimRay.direction),
                    target = null
                };
                ProjectileManager.instance.FireProjectile(fireProjectileInfo);
            }
            EffectManager.SimpleMuzzleFlash(HallOfMirrors.muzzleflashObject, base.gameObject, HallOfMirrors.muzzleString, false);
            Util.PlaySound(HallOfMirrors.soundString, base.gameObject);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.isAuthority && this.duration <= base.age)
            {
                this.outer.SetNextStateToMain();
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }

        public static GameObject projectilePrefab;

        public static float baseDuration;

        public static float damageCoefficient;

        public static string muzzleString;

        public static GameObject muzzleflashObject;

        public static string soundString;

        private float duration;
    }
}
