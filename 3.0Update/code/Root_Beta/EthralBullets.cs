using HarmonyLib;
using RootCore;

namespace RootStandardCards.Patches {

    [HarmonyPriority(Priority.First)]
    [HarmonyPatch(typeof(ProjectileHit), nameof(ProjectileHit.Hit))]
    public class EthralBullets {
        public static bool Prefix(HitInfo hit, ProjectileHit __instance) {
            if((bool)hit.transform && __instance.ownPlayer.HasCard("Ethral")) {
                HealthHandler healthHandler = hit.transform.GetComponent<HealthHandler>();
                if(healthHandler == null) return false;
                return healthHandler.player.teamID != __instance.ownPlayer.teamID;
            }
            return true;
        }

    }
}
