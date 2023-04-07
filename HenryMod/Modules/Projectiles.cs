using R2API;
using RoR2;
using RoR2.Projectile;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Networking;
using static R2API.DamageAPI;

namespace DuskWing.Modules
{
    internal static class Projectiles
    {
        internal static GameObject pipePrefab;
        internal static GameObject hologramPrefab;

        internal static void RegisterProjectiles()
        {
            CreateBomb();
            CreateHologram();

            AddProjectile(pipePrefab);
            AddProjectile(hologramPrefab);
        }

        internal static void AddProjectile(GameObject projectileToAdd)
        {
            Modules.Content.AddProjectilePrefab(projectileToAdd);
        }

        private static void CreateBomb()
        {
            pipePrefab = CloneProjectilePrefab("CommandoGrenadeProjectile", "DuskWingBombProjectile");

            ProjectileImpactExplosion bombImpactExplosion = pipePrefab.GetComponent<ProjectileImpactExplosion>();
            InitializeImpactExplosion(bombImpactExplosion);
            pipePrefab.GetComponent<ProjectileSimple>().desiredForwardSpeed = 200f;

            //bombImpactExplosion.impactEffect = Modules.Assets.bombExplosionEffect;
            //bombImpactExplosion.lifetimeExpiredSound = Modules.Assets.CreateNetworkSoundEventDef("DuskWingBombExplosion");

            bombImpactExplosion.blastRadius = 4f;
            bombImpactExplosion.destroyOnEnemy = true;
            bombImpactExplosion.lifetime = 12f;
            bombImpactExplosion.timerAfterImpact = true;
            bombImpactExplosion.lifetimeAfterImpact = 0f;
            bombImpactExplosion.GetComponent<ProjectileDamage>().damageType = DamageType.Generic;

            ProjectileController bombController = pipePrefab.GetComponent<ProjectileController>();
            if (Modules.Assets.mainAssetBundle.LoadAsset<GameObject>("DWgrenadeGhost") != null) bombController.ghostPrefab = CreateGhostPrefab("DWgrenadeGhost");
            bombController.startSound = "";
        }

        private static void CreateHologram()
        {
            hologramPrefab = CloneProjectilePrefab("LoaderPylon", "DuskWingHologramProjectile");
            hologramPrefab.AddComponent<ProjectileExplosion>();
            ProjectileExplosion hologramDetonation = hologramPrefab.GetComponent<ProjectileExplosion>();
            InitializeTimedExplosion(hologramDetonation);

            //ProjectileImpactExplosion bombImpactExplosion = pipePrefab.GetComponent<ProjectileImpactExplosion>();
            //InitializeImpactExplosion(bombImpactExplosion);
            hologramPrefab.GetComponent<ProjectileSimple>().desiredForwardSpeed = 0f;
            hologramPrefab.GetComponent<ProjectileSimple>().lifetime = 30f;

            hologramPrefab.GetComponent<ProjectileProximityBeamController>().attackFireCount = 1;
            hologramPrefab.GetComponent<ProjectileProximityBeamController>().attackInterval = 2f;
            hologramPrefab.GetComponent<ProjectileProximityBeamController>().damageCoefficient = 0f;
            hologramPrefab.GetComponent<ProjectileProximityBeamController>().procCoefficient = 0f;
            hologramPrefab.GetComponent<ProjectileProximityBeamController>().inheritDamageType = true;

            hologramPrefab.AddComponent<ModdedDamageTypeHolderComponent>();
            hologramPrefab.GetComponent<ModdedDamageTypeHolderComponent>().Add(DuskWing.LockOnDamageType);
            hologramDetonation.blastRadius = 7f;
            hologramDetonation.blastDamageCoefficient = 6f;

            ProjectileController hologramController = hologramPrefab.GetComponent<ProjectileController>();
            if (Modules.Assets.mainAssetBundle.LoadAsset<GameObject>("DWhologramGhost") != null) hologramController.ghostPrefab = CreateGhostPrefab("DWhologramGhost");
            hologramController.startSound = "";
        }

        private static void InitializeImpactExplosion(ProjectileImpactExplosion projectileImpactExplosion)
        {
            projectileImpactExplosion.blastDamageCoefficient = 1f;
            projectileImpactExplosion.blastProcCoefficient = 1f;
            projectileImpactExplosion.blastRadius = 1f;
            projectileImpactExplosion.bonusBlastForce = Vector3.zero;
            projectileImpactExplosion.childrenCount = 0;
            projectileImpactExplosion.childrenDamageCoefficient = 0f;
            projectileImpactExplosion.childrenProjectilePrefab = null;
            projectileImpactExplosion.destroyOnEnemy = false;
            projectileImpactExplosion.destroyOnWorld = false;
            projectileImpactExplosion.falloffModel = RoR2.BlastAttack.FalloffModel.None;
            projectileImpactExplosion.fireChildren = false;
            projectileImpactExplosion.impactEffect = null;
            projectileImpactExplosion.lifetime = 0f;
            projectileImpactExplosion.lifetimeAfterImpact = 0f;
            projectileImpactExplosion.lifetimeRandomOffset = 0f;
            projectileImpactExplosion.offsetForLifetimeExpiredSound = 0f;
            projectileImpactExplosion.timerAfterImpact = false;

            projectileImpactExplosion.GetComponent<ProjectileDamage>().damageType = DamageType.Generic;
        }

        private static void InitializeTimedExplosion(ProjectileExplosion projectileExplosion)
        {
            projectileExplosion.blastDamageCoefficient = 1f;
            projectileExplosion.blastProcCoefficient = 1f;
            projectileExplosion.blastRadius = 1f;
            projectileExplosion.bonusBlastForce = Vector3.zero;
            projectileExplosion.childrenCount = 0;
            projectileExplosion.childrenDamageCoefficient = 0f;
            projectileExplosion.childrenProjectilePrefab = null;
            projectileExplosion.falloffModel = RoR2.BlastAttack.FalloffModel.None;
            projectileExplosion.fireChildren = false;

            projectileExplosion.GetComponent<ProjectileDamage>().damageType = DamageType.Generic;
        }

        private static GameObject CreateGhostPrefab(string ghostName)
        {
            GameObject ghostPrefab = Modules.Assets.mainAssetBundle.LoadAsset<GameObject>(ghostName);
            if (!ghostPrefab.GetComponent<NetworkIdentity>()) ghostPrefab.AddComponent<NetworkIdentity>();
            if (!ghostPrefab.GetComponent<ProjectileGhostController>()) ghostPrefab.AddComponent<ProjectileGhostController>();

            Modules.Assets.ConvertAllRenderersToHopooShader(ghostPrefab);

            return ghostPrefab;
        }

        private static GameObject CloneProjectilePrefab(string prefabName, string newPrefabName)
        {
            GameObject newPrefab = PrefabAPI.InstantiateClone(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/" + prefabName), newPrefabName);
            return newPrefab;
        }
    }
}