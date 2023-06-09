﻿using DuskWing.Modules;
using EntityStates;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace DuskWing.SkillStates
{
    public class BurstLauncher : GenericProjectileBaseState
    {

        public static float BaseDuration = 0.3f;
        //delay here for example and to match animation
        //ordinarily I recommend not having a delay before projectiles. makes the move feel sluggish
        //public static float BaseDelayDuration = 0.01f * BaseDuration;

        public override void OnEnter()
        {
            base.projectilePrefab = Modules.Projectiles.pipePrefab;
            this.duration = this.baseDuration / base.attackSpeedStat;
            //base.effectPrefab = Modules.Assets.SomeMuzzleEffect;
            //targetmuzzle = "muzzleThrow"

            base.attackSoundString = "DuskWingBombThrow";
            
            base.baseDuration = BaseDuration;

            base.damageCoefficient = StaticValues.BurstLauncherDamageCoefficient;
            //proc coefficient is set on the components of the projectile prefab
            base.force = 500f;

            //base.projectilePitchBonus = 0;
            //base.minSpread = 0;
            //base.maxSpread = 0;

            base.recoilAmplitude = 0.1f;
            base.bloom = 10;
            base.OnEnter();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }

        public override void PlayAnimation(float duration)
        {

            if (base.GetModelAnimator())
            {
                base.PlayAnimation("Gesture, Override", "ThrowBomb", "ThrowBomb.playbackRate", this.duration);
            }
        }
    }
}