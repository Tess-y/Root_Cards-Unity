using HarmonyLib;
using UnityEngine;

namespace RootCore.Patches {
    [HarmonyPatch(typeof(Gun), nameof(Gun.ApplyProjectileStats))]
    public class GunPatchApplyProjectileStats {

        public static void Postfix(Gun __instance, GameObject obj) {
            obj.GetComponent<ProjectileHit>().damage += __instance.player.GetRootData().flatProjectileDamage;
        }

    }
}
