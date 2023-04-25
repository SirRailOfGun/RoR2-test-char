using BepInEx;
using DuskWing.Modules;
using DuskWing.Modules.Survivors;
using R2API;
using R2API.Utils;
using RoR2;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security;
using System.Security.Permissions;
using UnityEngine;
using UnityEngine.Networking;
using static R2API.DamageAPI;

[module: UnverifiableCode]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]

//rename this namespace
namespace DuskWing
{
    [BepInDependency("com.bepis.r2api", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("com.DestroyedClone.AncientScepter", BepInDependency.DependencyFlags.SoftDependency)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
    [BepInPlugin(MODUID, MODNAME, MODVERSION)]
    [R2APISubmoduleDependency(new string[]
    {
        "PrefabAPI",
        "LanguageAPI",
        "SoundAPI",
        "UnlockableAPI"
    })]

    public class DuskWing : BaseUnityPlugin
    {
        public static DamageAPI.ModdedDamageType StunCrownDamageType = DamageAPI.ReserveDamageType();
        public static DamageAPI.ModdedDamageType ImpairDamageType = DamageAPI.ReserveDamageType();
        public static DamageAPI.ModdedDamageType LockOnDamageType = DamageAPI.ReserveDamageType();

        // if you don't change these you're giving permission to deprecate the mod-
        //  please change the names to your own stuff, thanks
        //   this shouldn't even have to be said
        public const string MODUID = "com.SirRailOfGun.DuskWing";
        public const string MODNAME = "DuskWing";
        public const string MODVERSION = "0.1.0";

        // a prefix for name tokens to prevent conflicts- please capitalize all name tokens for convention
        public const string DEVELOPER_PREFIX = "SROG";

        public static DuskWing instance;

        public static bool ancientScepterInstalled = false;

        public static PluginInfo PInfo { get; private set; }
        private void Awake()
        {
            instance = this;
            PInfo = Info;

            Log.Init(Logger);
            Modules.Assets.Initialize(); // load assets and read config
            Modules.Config.ReadConfig();
            Modules.States.RegisterStates(); // register states for networking
            Modules.Buffs.RegisterBuffs(); // add and register custom buffs/debuffs
            Modules.Projectiles.RegisterProjectiles(); // add and register custom projectiles
            Modules.Tokens.AddTokens(); // register name tokens
            Modules.ItemDisplays.PopulateDisplays(); // collect item display prefabs for use in our display rules

            // survivor initialization
            new MyCharacter().Initialize();

            // now make a content pack and add it- this part will change with the next update
            new Modules.ContentPacks().Initialize();

            On.RoR2.HealthComponent.TakeDamage += StunCrownDamageHandler;
            Hook();
        }
        private void SetupModCompat()
        {
            //scepter stuff- dll won't compile without a reference to TILER2 and ClassicItems
            if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.DestroyedClone.AncientScepter"))
            {
                ancientScepterInstalled = true;
            }

            //FixItemDisplays();
        }

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private void ScepterSetup()
        {
            //AncientScepter.AncientScepterItem.instance.RegisterScepterSkill(scepterSpecialSkillDef, "MinerBody", SkillSlot.Special, 1);
        }

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private void ScepterClassicSetup()
        {
            //ThinkInvisible.ClassicItems.Scepter.instance.RegisterScepterSkill(scepterSpecialSkillDef, "MinerBody", SkillSlot.Special, specialSkillDef);
            //ThinkInvisible.ClassicItems.Scepter.instance.RegisterScepterSkill(scepterSpecialClassicSkillDef, "MinerBody", SkillSlot.Special, specialClassicSkillDef);
        }

        private void Hook()
        {
            // run hooks here, disabling one is as simple as commenting out the line
            //On.RoR2.CharacterBody.RecalculateStats += CharacterBody_RecalculateStats;
        }

        private void CharacterBody_RecalculateStats(On.RoR2.CharacterBody.orig_RecalculateStats orig, CharacterBody self)
        {
            orig(self);

            // a simple stat hook, adds armor after stats are recalculated
            if (self)
            {
                if (self.HasBuff(Modules.Buffs.lockOnBuff))
                {
                }
            }
        }

        private static void StunCrownDamageHandler(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo info)
        {
            if (info.HasModdedDamageType(ImpairDamageType))
            {
                var onHurt = self.body.GetComponent<SetStateOnHurt>();
                if (onHurt)
                {
                    onHurt.SetStun(StaticValues.ImpairStun);
                }
            }
            if (info.HasModdedDamageType(StunCrownDamageType))
            {
                var onHurt = self.body.GetComponent<SetStateOnHurt>();
                if (onHurt)
                {
                    onHurt.SetStun(StaticValues.StunCrownStun);
                }
            }
            if (self.body.HasBuff(Modules.Buffs.lockOnBuff))
            {
                info.crit = true;
                self.body.ClearTimedBuffs(Modules.Buffs.lockOnBuff);
            }
            if (info.HasModdedDamageType(LockOnDamageType))
            {
                if (self.body)
                {
                    self.body.AddTimedBuff(Modules.Buffs.lockOnBuff, 60f);
                }
            }
            orig(self, info);
        }
    }
}