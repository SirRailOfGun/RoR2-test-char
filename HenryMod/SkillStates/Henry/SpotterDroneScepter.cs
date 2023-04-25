using EntityStates;
using System;
using System.Collections.Generic;
using System.Text;

using RoR2.Projectile;
using RoR2;
using UnityEngine;
using DuskWing.Modules;
using UnityEngine.Networking;
using System.Linq;

namespace DuskWing.SkillStates
{
    internal class SpotterDroneScepter : GenericProjectileBaseState
    {
        public static float BaseDuration = 0.3f;

        public override void OnEnter()
        {
            base.projectilePrefab = Modules.Projectiles.heldImagePrefab;
            //base.effectPrefab = Modules.Assets.SomeMuzzleEffect;
            //targetmuzzle = "muzzleThrow"

            base.attackSoundString = "DuskWingBombThrow";

            base.baseDuration = BaseDuration;

            base.damageCoefficient = StaticValues.hallOfMirrorsDamageCoefficient;
            //proc coefficient is set on the components of the projectile prefab

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
