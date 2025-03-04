using HarmonyLib;
using RootCore;
using System;
using UnityEngine;

namespace RootStandardCards.Patches {
    [Serializable]
    [HarmonyPatch(typeof(HealthHandler), nameof(HealthHandler.DoDamage))]
    public class HealthHandlerPatchDoDamage {

        static bool Prefix(HealthHandler __instance, Vector2 damage, Vector2 position, Color blinkColor, GameObject damagingWeapon = null, Player damagingPlayer = null, bool healthRemoval = false, bool lethal = true, bool ignoreBlock = false) {
            if(!__instance.player.HasCard("Guardian") && PlayerManager.instance.players.Find(p=> !p.data.dead && p.teamID == __instance.player.teamID && p.playerID != __instance.player.playerID && p.HasCard("Guardian")) is Player player) {
                player.data.healthHandler.DoDamage(damage, position, blinkColor, damagingWeapon, damagingPlayer, healthRemoval, lethal, ignoreBlock);
                return false;
            }
            return true;
        }

    }
}
