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
        internal static GameObject spotterPrefab;
        internal static GameObject heldImagePrefab;

        internal static void RegisterProjectiles()
        {
            CreateBomb();
            CreateHologram();
            CreateScepterDrone();

            AddProjectile(pipePrefab);
            AddProjectile(spotterPrefab);
            AddProjectile(heldImagePrefab);
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

            bombImpactExplosion.blastRadius = 5f;
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
            spotterPrefab = CloneProjectilePrefab("LoaderPylon", "DuskWingHologramProjectile");

            //ProjectileImpactExplosion bombImpactExplosion = pipePrefab.GetComponent<ProjectileImpactExplosion>();
            //InitializeImpactExplosion(bombImpactExplosion);
            spotterPrefab.GetComponent<ProjectileSimple>().desiredForwardSpeed = 80f;
            spotterPrefab.GetComponent<ProjectileSimple>().lifetime = 30f;
            spotterPrefab.GetComponent<BeginRapidlyActivatingAndDeactivating>().delayBeforeBeginningBlinking = 27f;

            spotterPrefab.GetComponent<ProjectileProximityBeamController>().attackFireCount = 1;
            spotterPrefab.GetComponent<ProjectileProximityBeamController>().attackInterval = 2f;
            spotterPrefab.GetComponent<ProjectileProximityBeamController>().damageCoefficient = 0f;
            spotterPrefab.GetComponent<ProjectileProximityBeamController>().procCoefficient = 0f;
            spotterPrefab.GetComponent<ProjectileProximityBeamController>().inheritDamageType = true;

            spotterPrefab.AddComponent<ModdedDamageTypeHolderComponent>();
            spotterPrefab.GetComponent<ModdedDamageTypeHolderComponent>().Add(DuskWing.LockOnDamageType);

            ProjectileController hologramController = spotterPrefab.GetComponent<ProjectileController>();
            if (Modules.Assets.mainAssetBundle.LoadAsset<GameObject>("DWhologramGhost") != null) hologramController.ghostPrefab = CreateGhostPrefab("DWhologramGhost");
            hologramController.startSound = "";
            spotterPrefab.GetComponent<BeginRapidlyActivatingAndDeactivating>().blinkingRootObject = hologramController.ghostPrefab;
        }

        private static void CreateScepterDrone()
        {
            heldImagePrefab = CloneProjectilePrefab("LoaderPylon", "DuskWingHologramProjectile");

            //ProjectileImpactExplosion bombImpactExplosion = pipePrefab.GetComponent<ProjectileImpactExplosion>();
            //InitializeImpactExplosion(bombImpactExplosion);
            heldImagePrefab.GetComponent<ProjectileSimple>().desiredForwardSpeed = 0f;
            heldImagePrefab.GetComponent<ProjectileSimple>().lifetime = 30f;

            heldImagePrefab.GetComponent<ProjectileProximityBeamController>().attackFireCount = 2;
            heldImagePrefab.GetComponent<ProjectileProximityBeamController>().attackInterval = 1.5f;
            heldImagePrefab.GetComponent<ProjectileProximityBeamController>().damageCoefficient = 0f;
            heldImagePrefab.GetComponent<ProjectileProximityBeamController>().procCoefficient = 0f;
            heldImagePrefab.GetComponent<ProjectileProximityBeamController>().inheritDamageType = true;

            heldImagePrefab.AddComponent<ModdedDamageTypeHolderComponent>();
            heldImagePrefab.GetComponent<ModdedDamageTypeHolderComponent>().Add(DuskWing.LockOnDamageType);

            ProjectileController hologramController2 = heldImagePrefab.GetComponent<ProjectileController>();
            if (Modules.Assets.mainAssetBundle.LoadAsset<GameObject>("DWhologramGhost") != null) hologramController2.ghostPrefab = CreateGhostPrefab("DWhologramGhost");
            hologramController2.startSound = "";
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