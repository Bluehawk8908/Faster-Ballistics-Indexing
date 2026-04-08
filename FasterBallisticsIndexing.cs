using GHPC.Weapons;
using MelonLoader;
using HarmonyLib;
using FasterBallisticsIndexing;

[assembly: MelonInfo(typeof(FasterBallistics), "Faster Ballistics Indexing", "1.0.0", "Bluehawk")]
[assembly: MelonGame("Radian Simulations LLC", "GHPC")]

namespace FasterBallisticsIndexing
{
    public class FasterBallistics : MelonMod
    {
        public override void OnInitializeMelon()
        {
            MelonLogger.Msg("Patching reload functions");
            
        }
    }

    [HarmonyPatch(typeof(GHPC.Weapons.AmmoFeed), "Reload")]
    public static class ReloadOnR
    {
        private static void Prefix(GHPC.Weapons.AmmoFeed __instance)
        {
            AmmoType queued = __instance.QueuedClipType.MinimalPattern[0].AmmoType;
            WeaponSystem wep = __instance.gameObject.GetComponent<WeaponSystem>();
            FireControlSystem fcs = wep.FCS;
            if (queued != null) { fcs.CurrentAmmoType = queued; }           
        }
    }

    [HarmonyPatch(typeof(GHPC.Weapons.AmmoFeed), "SetNextClipType")]
    public static class ReloadOnNum
    {
        private static void Postfix(GHPC.Weapons.AmmoFeed __instance)
        {
            AmmoType queued = __instance.QueuedClipType.MinimalPattern[0].AmmoType;
            WeaponSystem wep = __instance.gameObject.GetComponent<WeaponSystem>();
            FireControlSystem fcs = wep.FCS;
            if (queued != null && __instance.AmmoTypeInBreech == null) { fcs.CurrentAmmoType = queued; }
        }
    }   
}
