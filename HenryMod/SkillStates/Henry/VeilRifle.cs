using EntityStates;
using RoR2;
using UnityEngine;

namespace DuskWing.SkillStates
{
    public class VeilRifle : BaseSkillState
    {
        public float baseDuration = 0.35f;
        private float duration;


        //OnEnter() runs once at the start of the skill
        //All we do here is create a BulletAttack and fire it
        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = this.baseDuration / base.attackSpeedStat;
            Ray aimRay = base.GetAimRay();
            base.StartAimMode(aimRay, 2f, true);
            base.PlayAnimation("Gesture Additive, Right", "FirePistol, Right");
            base.AddRecoil(-0.6f, 0.6f, -0.6f, 0.6f);


            if (base.isAuthority)
            {
                new BulletAttack
                {
                    owner = base.gameObject,
                    weapon = base.gameObject,
                    origin = aimRay.origin,
                    aimVector = aimRay.direction,
                    minSpread = 0f,
                    maxSpread = 0f,
                    bulletCount = 1U,
                    procCoefficient = 1f,
                    damage = base.characterBody.damage * 1f,
                    force = 3,
                    falloffModel = BulletAttack.FalloffModel.None,
                    muzzleName = "MuzzleRight",
                    //hitEffectPrefab = this.hitEffectPrefab,
                    isCrit = base.RollCrit(),
                    HitEffectNormal = false,
                    stopperMask = LayerIndex.noCollision.mask,
                    //smartCollision = true,
                    maxDistance = 100f
                }.Fire();
            }
        }

        //This method runs once at the end
        //Here, we are doing nothing
        public override void OnExit()
        {
            base.OnExit();
        }

        //FixedUpdate() runs almost every frame of the skill
        //Here, we end the skill once it exceeds its intended duration
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.fixedAge >= this.duration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }

        //GetMinimumInterruptPriority() returns the InterruptPriority required to interrupt this skill
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}