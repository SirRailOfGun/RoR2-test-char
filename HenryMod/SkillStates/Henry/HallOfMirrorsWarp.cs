using EntityStates;
using RoR2.Projectile;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using DuskWing.Modules;
using UnityEngine.Networking;
using System.Linq;

namespace DuskWing.SkillStates
{
    //public class HallOfMirrorsWarp : GenericProjectileBaseState
    //{
    //    public static float BaseDuration = 0.3f;

    //    public override void OnEnter()
    //    {
    //        base.projectilePrefab = Modules.Projectiles.hologramPrefab;
    //        //base.effectPrefab = Modules.Assets.SomeMuzzleEffect;
    //        //targetmuzzle = "muzzleThrow"

    //        base.attackSoundString = "DuskWingBombThrow";

    //        base.baseDuration = BaseDuration;

    //        base.damageCoefficient = StaticValues.hallOfMirrorsDamageCoefficient;
    //        //proc coefficient is set on the components of the projectile prefab

    //        //base.projectilePitchBonus = 0;
    //        //base.minSpread = 0;
    //        //base.maxSpread = 0;

    //        base.recoilAmplitude = 0.1f;
    //        base.bloom = 10;
    //        base.OnEnter();
    //    }

    //    public override void FixedUpdate()
    //    {
    //        base.FixedUpdate();
    //    }

    //    public override InterruptPriority GetMinimumInterruptPriority()
    //    {
    //        return InterruptPriority.Skill;
    //    }

    //    public override void PlayAnimation(float duration)
    //    {

    //        if (base.GetModelAnimator())
    //        {
    //            base.PlayAnimation("Gesture, Override", "ThrowBomb", "ThrowBomb.playbackRate", this.duration);
    //        }
    //    }
    //}

    internal class HallOfMirrorsWarp : BaseSkillState
    {
        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = HallOfMirrorsWarp.baseDuration / this.attackSpeedStat;
            EffectManager.SimpleImpactEffect(HallOfMirrorsWarp.enterEffectPrefab, base.characterBody.corePosition, Vector3.up, false);
            Util.PlaySound(HallOfMirrorsWarp.enterSoundString, base.gameObject);
            if (NetworkServer.active)
            {
                BullseyeSearch bullseyeSearch = new BullseyeSearch();
                bullseyeSearch.filterByDistinctEntity = true;
                bullseyeSearch.filterByLoS = false;
                bullseyeSearch.maxDistanceFilter = float.PositiveInfinity;
                bullseyeSearch.minDistanceFilter = 0f;
                bullseyeSearch.minAngleFilter = 0f;
                bullseyeSearch.maxAngleFilter = 180f;
                bullseyeSearch.sortMode = BullseyeSearch.SortMode.Distance;
                bullseyeSearch.teamMaskFilter = TeamMask.GetUnprotectedTeams(base.GetTeam());
                bullseyeSearch.searchOrigin = base.characterBody.corePosition;
                bullseyeSearch.viewer = null;
                bullseyeSearch.RefreshCandidates();
                bullseyeSearch.FilterOutGameObject(base.gameObject);
                IEnumerable<HurtBox> results = bullseyeSearch.GetResults();
                this.detonationTargets = results.ToArray<HurtBox>();
                HallOfMirrorsWarp.DetonationController detonationController = new HallOfMirrorsWarp.DetonationController();
                detonationController.characterBody = base.characterBody;
                detonationController.interval = HallOfMirrorsWarp.detonationInterval;
                detonationController.detonationTargets = this.detonationTargets;
                detonationController.damageStat = this.damageStat;
                detonationController.isCrit = base.RollCrit();
                detonationController.active = true;
            }
            base.PlayAnimation(this.animationLayerName, this.animationStateName, this.playbackRateParam, this.duration);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.isAuthority && base.fixedAge >= this.duration)
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }

        public static float baseDuration;

        public static float baseDamageCoefficient;

        public static float damageCoefficientPerStack;

        public static float procCoefficient;

        public static float detonationInterval;

        public static GameObject detonationEffectPrefab;

        public static GameObject orbEffectPrefab;

        public static GameObject enterEffectPrefab;

        public static string enterSoundString;

        [SerializeField]
        public string animationLayerName;

        [SerializeField]
        public string animationStateName;

        [SerializeField]
        public string playbackRateParam;

        private float duration;

        private HurtBox[] detonationTargets;

        private class DetonationController
        {
            public bool active
            {
                get
                {
                    return this._active;
                }
                set
                {
                    if (this._active == value)
                    {
                        return;
                    }
                    this._active = value;
                    if (this._active)
                    {
                        RoR2Application.onFixedUpdate += this.FixedUpdate;
                        return;
                    }
                    RoR2Application.onFixedUpdate -= this.FixedUpdate;
                }
            }

            private void FixedUpdate()
            {
                if (!this.characterBody || !this.characterBody.healthComponent || !this.characterBody.healthComponent.alive)
                {
                    this.active = false;
                    return;
                }
                this.timer -= Time.deltaTime;
                if (this.timer <= 0f)
                {
                    this.timer = this.interval;
                    while (this.i < this.detonationTargets.Length)
                    {
                        try
                        {
                            HurtBox targetHurtBox = null;
                            Util.Swap<HurtBox>(ref targetHurtBox, ref this.detonationTargets[this.i]);
                            if (this.DoDetonation(targetHurtBox))
                            {
                                break;
                            }
                        }
                        catch (Exception message)
                        {
                            Debug.LogError(message);
                        }
                        this.i++;
                    }
                    if (this.i >= this.detonationTargets.Length)
                    {
                        this.active = false;
                    }
                }
            }

            private bool DoDetonation(HurtBox targetHurtBox)
            {
                if (!targetHurtBox)
                {
                    return false;
                }
                HealthComponent healthComponent = targetHurtBox.healthComponent;
                if (!healthComponent)
                {
                    return false;
                }
                CharacterBody body = healthComponent.body;
                if (!body)
                {
                    return false;
                }
                if (body.GetBuffCount(RoR2Content.Buffs.LunarDetonationCharge) <= 0)
                {
                    return false;
                }
                RoR2.Orbs.LunarDetonatorOrb lunarDetonatorOrb = new RoR2.Orbs.LunarDetonatorOrb();
                lunarDetonatorOrb.origin = this.characterBody.corePosition;
                lunarDetonatorOrb.target = targetHurtBox;
                lunarDetonatorOrb.attacker = this.characterBody.gameObject;
                lunarDetonatorOrb.baseDamage = this.damageStat * HallOfMirrorsWarp.baseDamageCoefficient;
                lunarDetonatorOrb.damagePerStack = this.damageStat * HallOfMirrorsWarp.damageCoefficientPerStack;
                lunarDetonatorOrb.damageColorIndex = DamageColorIndex.Default;
                lunarDetonatorOrb.isCrit = this.isCrit;
                lunarDetonatorOrb.procChainMask = default(ProcChainMask);
                lunarDetonatorOrb.procCoefficient = 1f;
                lunarDetonatorOrb.detonationEffectPrefab = HallOfMirrorsWarp.detonationEffectPrefab;
                lunarDetonatorOrb.travelSpeed = 120f;
                lunarDetonatorOrb.orbEffectPrefab = HallOfMirrorsWarp.orbEffectPrefab;
                RoR2.Orbs.OrbManager.instance.AddOrb(lunarDetonatorOrb);
                return true;
            }

            public HurtBox[] detonationTargets;
            public CharacterBody characterBody;
            public float damageStat;
            public bool isCrit;
            public float interval;
            private int i;
            private float timer;
            private bool _active;
        }
    }
}
