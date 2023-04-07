﻿using BepInEx.Configuration;
using DuskWing.Modules.Characters;
using DuskWing.SkillStates;
using RoR2;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DuskWing.Modules.Survivors
{
    internal class MyCharacter : SurvivorBase
    {
        //used when building your character using the prefabs you set up in unity
        //don't upload to thunderstore without changing this
        public override string prefabBodyName => "DuskWing";

        public const string DUSKWING_PREFIX = DuskWing.DEVELOPER_PREFIX + "_DUSK_WING_BODY_";

        //used when registering your survivor's language tokens
        public override string survivorTokenPrefix => DUSKWING_PREFIX;

        public override BodyInfo bodyInfo { get; set; } = new BodyInfo
        {
            bodyName = "HenryTutorialBody",
            bodyNameToken = DUSKWING_PREFIX + "NAME",
            subtitleNameToken = DUSKWING_PREFIX + "SUBTITLE",

            characterPortrait = Assets.mainAssetBundle.LoadAsset<Texture>("texHenryIcon"),
            bodyColor = Color.white,

            crosshair = Modules.Assets.LoadCrosshair("Standard"),
            podPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/NetworkedObjects/SurvivorPod"),

            maxHealth = 110f,
            healthRegen = 1.5f,
            armor = 0f,

            jumpCount = 1,
        };

        public override CustomRendererInfo[] customRendererInfos { get; set; } = new CustomRendererInfo[] 
        {
                new CustomRendererInfo
                {
                    childName = "SwordModel",
                    material = Materials.CreateHopooMaterial("matHenry"),
                },
                new CustomRendererInfo
                {
                    childName = "GunModel",
                },
                new CustomRendererInfo
                {
                    childName = "Model",
                }
        };

        public override UnlockableDef characterUnlockableDef => null;

        public override Type characterMainState => typeof(EntityStates.GenericCharacterMain);

        public override ItemDisplaysBase itemDisplays => new DuskWingItemDisplays();

                                                                          //if you have more than one character, easily create a config to enable/disable them like this
        public override ConfigEntry<bool> characterEnabledConfig => null; //Modules.Config.CharacterEnableConfig(bodyName);

        //private static UnlockableDef masterySkinUnlockableDef;

        public override void InitializeCharacter()
        {
            base.InitializeCharacter();
        }

        public override void InitializeUnlockables()
        {
            //uncomment this when you have a mastery skin. when you do, make sure you have an icon too
            //masterySkinUnlockableDef = Modules.Unlockables.AddUnlockable<Modules.Achievements.MasteryAchievement>();
        }

        public override void InitializeHitboxes()
        {
            ChildLocator childLocator = bodyPrefab.GetComponentInChildren<ChildLocator>();

            //example of how to create a hitbox
            //Transform hitboxTransform = childLocator.FindChild("SwordHitbox");
            //Modules.Prefabs.SetupHitbox(prefabCharacterModel.gameObject, hitboxTransform, "Sword");
        }

        public override void InitializeSkills()
        {
            Modules.Skills.CreateSkillFamilies(bodyPrefab);
            string prefix = DuskWing.DEVELOPER_PREFIX;

            #region Primary
            //Creates a skilldef for a typical primary 
            SkillDef primarySkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo(prefix + "_DUSK_WING_BODY_PRIMARY_VEILRIFLE_NAME",
                                                                                      prefix + "_DUSK_WING_BODY_PRIMARY_VEILRIFLE_DESCRIPTION",
                                                                                      Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texSSCLineAttackIcon"),
                                                                                      new EntityStates.SerializableEntityStateType(typeof(SkillStates.VeilRifle)),
                                                                                      "Weapon",
                                                                                      true));


            Modules.Skills.AddPrimarySkills(bodyPrefab, primarySkillDef);
            #endregion

            #region Secondary
            SkillDef secondarySkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "_DUSK_WING_BODY_SECONDARY_BURST_LAUNCHER_NAME",
                skillNameToken = prefix + "_DUSK_WING_BODY_SECONDARY_BURST_LAUNCHER_NAME",
                skillDescriptionToken = prefix + "_DUSK_WING_BODY_SECONDARY_BURST_LAUNCHER_DESCRIPTION",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texSSCRangedAttackIcon"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.BurstLauncher)),
                activationStateMachineName = "Slide",
                baseMaxStock = 1,
                baseRechargeInterval = 4f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = false,
                cancelSprintingOnActivation = true,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1
            });

            Modules.Skills.AddSecondarySkills(bodyPrefab, secondarySkillDef);
            #endregion

            #region Utility
            SkillDef utilitySkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "_DUSK_WING_BODY_UTILITY_STUN_CROWN_NAME",
                skillNameToken = prefix + "_DUSK_WING_BODY_UTILITY_STUN_CROWN_NAME",
                skillDescriptionToken = prefix + "_DUSK_WING_BODY_UTILITY_STUN_CROWN_DESCRIPTION",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texSSCBurstAttackIcon"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.StunCrown)),
                activationStateMachineName = "Slide",
                baseMaxStock = 1,
                baseRechargeInterval = 10f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.PrioritySkill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = false,
                mustKeyPress = false,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1
            });

            Modules.Skills.AddUtilitySkills(bodyPrefab, utilitySkillDef);
            #endregion

            #region Special
            SkillDef specialSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "_DUSK_WING_BODY_SPECIAL_HALL_OF_MIRRORS_NAME",
                skillNameToken = prefix + "_DUSK_WING_BODY_SPECIAL_HALL_OF_MIRRORS_NAME",
                skillDescriptionToken = prefix + "_DUSK_WING_BODY_SPECIAL_HALL_OF_MIRRORS_DESCRIPTION",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texSSCLogoIcon"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.HallOfMirrorsWarp)),
                activationStateMachineName = "Slide",
                baseMaxStock = 1,
                baseRechargeInterval = 10f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = false,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1
            });

            Modules.Skills.AddSpecialSkills(bodyPrefab, specialSkillDef);
            #endregion

            #region Special Passive
            bodyPrefab.AddComponent<HallOfMirrorsPassiveAttatchment>();
            //SkillDef passivePart = Modules.Skills.CreateSkillDef(new SkillDefInfo
            //{
            //    //skillName = prefix + "_DUSK_WING_BODY_SPECIAL_HALL_OF_MIRRORS_NAME",
            //    //skillNameToken = prefix + "_DUSK_WING_BODY_SPECIAL_HALL_OF_MIRRORS_NAME",
            //    //skillDescriptionToken = prefix + "_DUSK_WING_BODY_SPECIAL_HALL_OF_MIRRORS_DESCRIPTION",
            //    //skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texSSCLogoIcon"),
            //    activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.HallOfMirrors)),
            //    activationStateMachineName = "Slide",
            //    baseMaxStock = 1,
            //    baseRechargeInterval = 10f,
            //    beginSkillCooldownOnSkillEnd = false,
            //    canceledFromSprinting = false,
            //    forceSprintDuringState = false,
            //    fullRestockOnAssign = true,
            //    interruptPriority = EntityStates.InterruptPriority.Skill,
            //    resetCooldownTimerOnUse = false,
            //    //isCombatSkill = true,
            //    mustKeyPress = false,
            //    cancelSprintingOnActivation = false,
            //    rechargeStock = 1,
            //    requiredStock = 1,
            //    stockToConsume = 1
            //});

            //Modules.Skills.AddSpecialSkills(bodyPrefab, passivePart);
            #endregion
        }

        public override void InitializeSkins()
        {
            ModelSkinController skinController = prefabCharacterModel.gameObject.AddComponent<ModelSkinController>();
            ChildLocator childLocator = prefabCharacterModel.GetComponent<ChildLocator>();

            CharacterModel.RendererInfo[] defaultRendererinfos = prefabCharacterModel.baseRendererInfos;

            List<SkinDef> skins = new List<SkinDef>();

            #region DefaultSkin
            //this creates a SkinDef with all default fields
            SkinDef defaultSkin = Modules.Skins.CreateSkinDef(DUSKWING_PREFIX + "DEFAULT_SKIN_NAME",
                Assets.mainAssetBundle.LoadAsset<Sprite>("texMainSkin"),
                defaultRendererinfos,
                prefabCharacterModel.gameObject);

            //these are your Mesh Replacements. The order here is based on your CustomRendererInfos from earlier
            //pass in meshes as they are named in your assetbundle
            //defaultSkin.meshReplacements = Modules.Skins.getMeshReplacements(defaultRendererinfos,
            //    "meshDuskWingSword",
            //    "meshDuskWingGun",
            //    "meshDuskWing");

            //add new skindef to our list of skindefs. this is what we'll be passing to the SkinController
            skins.Add(defaultSkin);
            #endregion
            
            //uncomment this when you have a mastery skin
            #region MasterySkin
            /*
            //creating a new skindef as we did before
            SkinDef masterySkin = Modules.Skins.CreateSkinDef(DuskWingPlugin.DEVELOPER_PREFIX + "_DUSK_WING_BODY_MASTERY_SKIN_NAME",
                Assets.mainAssetBundle.LoadAsset<Sprite>("texMasteryAchievement"),
                defaultRendererinfos,
                prefabCharacterModel.gameObject,
                masterySkinUnlockableDef);

            //adding the mesh replacements as above. 
            //if you don't want to replace the mesh (for example, you only want to replace the material), pass in null so the order is preserved
            masterySkin.meshReplacements = Modules.Skins.getMeshReplacements(defaultRendererinfos,
                "meshDuskWingSwordAlt",
                null,//no gun mesh replacement. use same gun mesh
                "meshDuskWingAlt");

            //masterySkin has a new set of RendererInfos (based on default rendererinfos)
            //you can simply access the RendererInfos defaultMaterials and set them to the new materials for your skin.
            masterySkin.rendererInfos[0].defaultMaterial = Modules.Materials.CreateHopooMaterial("matDuskWingAlt");
            masterySkin.rendererInfos[1].defaultMaterial = Modules.Materials.CreateHopooMaterial("matDuskWingAlt");
            masterySkin.rendererInfos[2].defaultMaterial = Modules.Materials.CreateHopooMaterial("matDuskWingAlt");

            //here's a barebones example of using gameobjectactivations that could probably be streamlined or rewritten entirely, truthfully, but it works
            masterySkin.gameObjectActivations = new SkinDef.GameObjectActivation[]
            {
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("GunModel"),
                    shouldActivate = false,
                }
            };
            //simply find an object on your child locator you want to activate/deactivate and set if you want to activate/deacitvate it with this skin

            skins.Add(masterySkin);
            */
            #endregion

            skinController.skins = skins.ToArray();
        }
    }
}