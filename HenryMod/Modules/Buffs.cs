using RoR2;
using System.Collections.Generic;
using UnityEngine;

namespace DuskWing.Modules
{
    public static class Buffs
    {
        // armor buff gained during roll
        internal static BuffDef lockOnBuff;

        internal static void RegisterBuffs()
        {
            Sprite hologramIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texBazookaOutIcon");
            lockOnBuff = AddNewBuff("DuskWingLockOnDebuff",
                hologramIcon, 
                Color.red, 
                false, 
                true);
        }

        // simple helper method
        internal static BuffDef AddNewBuff(string buffName, Sprite buffIcon, Color buffColor, bool canStack, bool isDebuff)
        {
            BuffDef buffDef = ScriptableObject.CreateInstance<BuffDef>();
            buffDef.name = buffName;
            buffDef.buffColor = buffColor;
            buffDef.canStack = canStack;
            buffDef.isDebuff = isDebuff;
            buffDef.eliteDef = null;
            buffDef.iconSprite = buffIcon;

            Modules.Content.AddBuffDef(buffDef);

            return buffDef;
        }
    }
}