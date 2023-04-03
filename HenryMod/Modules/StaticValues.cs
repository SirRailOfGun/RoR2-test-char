using System;

namespace DuskWing.Modules
{
    internal static class StaticValues
    {
        internal static string descriptionText = "The SSC Dusk Wing frame is a mobile hard suit developed with DHIYED-derived technologies.<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine 
            + "< ! > The Veil Rifle can pierce through foes and terrain alike." + Environment.NewLine + Environment.NewLine
            + "< ! > The Burst Launcher will stun foes on crit. Use it agianst foes that have been Locked On." + Environment.NewLine + Environment.NewLine
            + "< ! > The Stun Crown's cooldown is quite long. Use it with purpose." + Environment.NewLine + Environment.NewLine
            + "< ! > Hall Of Mirrors is a passive in disguise. Use it to empower your attacks, or to reposition yourself quickly." + Environment.NewLine + Environment.NewLine;


        internal const float swordDamageCoefficient = 2.8f;

        internal const float VeilRifleDamageCoefficient = 1.0f;

        internal const float gunDamageCoefficient = 4.2f;

        internal const float BurstLauncherDamageCoefficient = 3.5f;

        internal const float StunCrownDamageCoefficient = 1.0f;
        internal const float StunCrownBlastSize = 40.0f;
        internal const float StunCrownStun = 5f;

        internal const float bombDamageCoefficient = 16f;
    }
}