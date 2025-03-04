using HarmonyLib;
using RootCore;
using System;

namespace RootStandardCards.Patches {
    [Serializable]
    [HarmonyPatch(typeof(HealthHandler), "Heal")]
    public class HealthHandlerPatchHeal {

        [HarmonyPriority(Priority.First)]
        static void Prefix(HealthHandler __instance, ref float healAmount) {
            if(__instance.GetComponent<Player>().HasCard("Rejuvenation")) {
                healAmount *= 2;
            }
        }

    }
}
