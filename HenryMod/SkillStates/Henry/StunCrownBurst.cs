using DuskWing.Modules;
using EntityStates;
using R2API;
using RoR2;
using RoR2.Projectile;
using UnityEngine;
using static R2API.DamageAPI;

namespace DuskWing.SkillStates
{
    public class StunCrownBurst : BaseState
    {
        public override void OnEnter()
        {
            base.OnEnter();
            this.animator = base.GetModelAnimator();
            //this.animator;
            if (base.characterBody)
            {
                base.characterBody.onSkillActivatedAuthority += this.OnSkillActivatedAuthority;
            }
            this.FireSmokebomb();
            Util.PlaySound(StunCrownBurst.enterStealthSound, base.gameObject);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.fixedAge > StunCrownBurst.duration)
            {
                this.outer.SetNextStateToMain();
            }
        }

        public override void OnExit()
        {
            if (!this.outer.destroying)
            {
                //this.FireSmokebomb();
            }
            Util.PlaySound(StunCrownBurst.exitStealthSound, base.gameObject);
            if (base.characterBody)
            {
                base.characterBody.onSkillActivatedAuthority -= this.OnSkillActivatedAuthority;
            }
            if (this.animator)
            {
                this.animator.SetLayerWeight(this.animator.GetLayerIndex("Body, StealthWeapon"), 0f);
            }
            base.OnExit();
        }

        private void OnSkillActivatedAuthority(GenericSkill skill)
        {
            if (skill.skillDef.isCombatSkill)
            {
                this.outer.SetNextStateToMain();
            }
        }

        private void FireSmokebomb()
        {
            if (base.isAuthority)
            {
                BlastAttack blastAttack = new BlastAttack();
                blastAttack.radius = StaticValues.StunCrownBlastSize;
                blastAttack.procCoefficient = 1f;
                blastAttack.position = base.transform.position;
                blastAttack.attacker = base.gameObject;
                blastAttack.crit = Util.CheckRoll(base.characterBody.crit, base.characterBody.master);
                blastAttack.baseDamage = base.characterBody.damage * StaticValues.StunCrownDamageCoefficient;
                blastAttack.falloffModel = BlastAttack.FalloffModel.None;
                blastAttack.AddModdedDamageType(DuskWing.StunCrownDamageType);
                blastAttack.baseForce = 1f;
                blastAttack.teamIndex = TeamComponent.GetObjectTeam(blastAttack.attacker);
                blastAttack.attackerFiltering = AttackerFiltering.NeverHitSelf;
                blastAttack.Fire();
            }
            if (StunCrownBurst.smokeBombEffectPrefab)
            {
                EffectManager.SimpleMuzzleFlash(StunCrownBurst.smokeBombEffectPrefab, base.gameObject, StunCrownBurst.smokeBombMuzzleString, false);
            }
            //if (base.characterMotor)
            //{
            //    base.characterMotor.velocity = new Vector3(base.characterMotor.velocity.x, StunCrownBurst.shortHopVelocity, base.characterMotor.velocity.z);
            //}
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }

        public static float duration;

        public static string enterStealthSound;

        public static string exitStealthSound;

        public static float blastAttackRadius;

        public static float blastAttackDamageCoefficient;

        public static float blastAttackProcCoefficient;

        public static float blastAttackForce;

        public static GameObject smokeBombEffectPrefab;

        public static string smokeBombMuzzleString;

        public static float shortHopVelocity;

        private Animator animator;
    }
}