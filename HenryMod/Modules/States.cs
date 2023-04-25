using DuskWing.SkillStates;

namespace DuskWing.Modules
{
    public static class States
    {
        internal static void RegisterStates()
        {
            Modules.Content.AddEntityState(typeof(VeilRifle));
            Modules.Content.AddEntityState(typeof(BurstLauncher));
            Modules.Content.AddEntityState(typeof(StunCrown));
            Modules.Content.AddEntityState(typeof(SpotterDrone));
        }
    }
}