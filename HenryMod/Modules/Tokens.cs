using R2API;
using System;

namespace DuskWing.Modules
{
    internal static class Tokens
    {
        internal static void AddTokens()
        {
            #region DuskWing
            string prefix = DuskWing.DEVELOPER_PREFIX + "_DUSK_WING_BODY_";

            string desc = StaticValues.descriptionText;
            string outro = "..You think I escaped? That this is all real?";
            string outroFailure = "Many things happened the moment I think I died.";

            LanguageAPI.Add(prefix + "NAME", "Dusk Wing");
            LanguageAPI.Add(prefix + "DESCRIPTION", desc);
            LanguageAPI.Add(prefix + "SUBTITLE", "Esoteric EVA");
            LanguageAPI.Add(prefix + "LORE", "“Belief in what we could see, what we could touch – in what our comp/cons assured us was there, in our own subjectivity and memory. Belief in reality became a weapon. We approached the metavault knowing that we would face an unknown enemy, but we approached with the advantage of numbers and machine strength." +
                "\n“DHIYED taught us as it killed us: through garbled comms chatter, through the screams of the dying, through the cackling of our mirror-selves as they killed us. Every spoofed signature, every temporal skip, every memetic, every non-Euclid – these were lessons." +
                "\n“Do you understand?" +
                "\n“DHIYED the Teacher. DHIYED the Monster. As we killed it, DHIYED taught us what to fear, and how to face it." +
                "\n“What do I fear now? That's a good question. What does the pilot fear who cracked open DHIYED’s casket?" +
                "\n“I don’t think we killed it. I think it wants us to believe we killed it – and I cannot imagine what it has done while we think ourselves safe.”");
            LanguageAPI.Add(prefix + "OUTRO_FLAVOR", outro);
            LanguageAPI.Add(prefix + "OUTRO_FAILURE", outroFailure);

            #region Skins
            LanguageAPI.Add(prefix + "DEFAULT_SKIN_NAME", "Dusk Wing");
            LanguageAPI.Add(prefix + "MASTERY_SKIN_NAME", "Smith-Shimano");
            LanguageAPI.Add(prefix + "GRANDMASTERY_SKIN_NAME", "HORUS");
            #endregion

            #region Passive
            LanguageAPI.Add(prefix + "PASSIVE_NAME", "DuskWing passive");
            LanguageAPI.Add(prefix + "PASSIVE_DESCRIPTION", "Sample text.");
            #endregion

            #region Primary
            LanguageAPI.Add(prefix + "PRIMARY_VEILRIFLE_NAME", "Veil Rifle");
            LanguageAPI.Add(prefix + "PRIMARY_VEILRIFLE_DESCRIPTION", Helpers.agilePrefix + $"Fire a <style=cIsUtility>piercing</style> beam for <style=cIsDamage>{100f * StaticValues.VeilRifleDamageCoefficient}% damage</style>.");
            #endregion

            #region Secondary
            //LanguageAPI.Add(prefix + "SECONDARY_GUN_NAME", "Handgun");
            //LanguageAPI.Add(prefix + "SECONDARY_GUN_DESCRIPTION", Helpers.agilePrefix + $"Fire a handgun for <style=cIsDamage>{100f * StaticValues.gunDamageCoefficient}% damage</style>.");
            LanguageAPI.Add(prefix + "SECONDARY_BURST_LAUNCHER_NAME", "Burst Launcher");
            LanguageAPI.Add(prefix + "SECONDARY_BURST_LAUNCHER_DESCRIPTION", $"Fire a grenade for <style=cIsDamage>{100f * StaticValues.BurstLauncherDamageCoefficient}% damage</style>.");
            #endregion

            #region Utility
            //LanguageAPI.Add(prefix + "UTILITY_ROLL_NAME", "Roll");
            //LanguageAPI.Add(prefix + "UTILITY_ROLL_DESCRIPTION", "Roll a short distance, gaining <style=cIsUtility>300 armor</style>. <style=cIsUtility>You cannot be hit during the roll.</style>");
            LanguageAPI.Add(prefix + "UTILITY_STUN_CROWN_NAME", "Stun Crown");
            LanguageAPI.Add(prefix + "UTILITY_STUN_CROWN_DESCRIPTION", Helpers.agilePrefix + $"Emit a blinding flash, dealing <style=cIsDamage>{100f * StaticValues.StunCrownDamageCoefficient}% damage</style> and <style=cIsUtility>stunning</style> foes for 5 seconds.");
            #endregion

            #region Special
            //LanguageAPI.Add(prefix + "SPECIAL_BOMB_NAME", "Bomb");
            //LanguageAPI.Add(prefix + "SPECIAL_BOMB_DESCRIPTION", $"Throw a bomb for <style=cIsDamage>{100f * StaticValues.bombDamageCoefficient}% damage</style>.");
            //LanguageAPI.Add(prefix + "SPECIAL_HALL_OF_MIRRORS_NAME", "Hall Of Mirrors");
            //LanguageAPI.Add(prefix + "SPECIAL_HALL_OF_MIRRORS_DESCRIPTION", $"Every <style=cIsUtility>{StaticValues.hallOfMirrorsHologramDelay}</style> seconds, place a Hologram that you can teleport back to dealing <style=cIsDamage>{100f * StaticValues.hallOfMirrorsDamageCoefficient}% damage</style>.");

            LanguageAPI.Add(prefix + "SPECIAL_SPOTTER_DRONE_NAME", "Lotus Projector");
            LanguageAPI.Add(prefix + "SPECIAL_SPOTTER_DRONE_DESCRIPTION", $"Deploy a drone which <style=cIsUtility>Locks On</style> to the nearest foe every 2 seconds. The next attack against a Locked On foe is a <style=cIsDamage>crtitical</style>");

            LanguageAPI.Add(prefix + "SPECIAL_SPOTTER_DRONE_SCEPTER_NAME", "The Lesson of the Held Image");
            LanguageAPI.Add(prefix + "SPECIAL_SPOTTER_DRONE_SCEPTER_DESCRIPTION", $"Deploy a drone which <style=cIsUtility>Locks On</style> to the nearest 2 foes every 1.5 seconds. The next attack against a Locked On foe is a <style=cIsDamage>crtitical</style>");
            #endregion

            #region Achievements
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_ACHIEVEMENT_NAME", "Dusk Wing: Mastery");
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_ACHIEVEMENT_DESC", "As Dusk Wing, beat the game or obliterate on Monsoon.");
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_UNLOCKABLE_NAME", "Dusk Wing: Mastery");
            #endregion
            #endregion
        }
    }
}