using HarmonyLib;
using RootCore;
using System;

namespace RootAdvancedCards.Patches {
    [Serializable]
    [HarmonyPatch(typeof(HealthHandler))]
    internal class HealthHandlerPatchDie {


        [HarmonyPrefix]
        [HarmonyPatch(nameof(HealthHandler.RPCA_Die))]
        [HarmonyPatch(nameof(HealthHandler.RPCA_Die_Phoenix))]
        private static void RPCA_Die(HealthHandler __instance, CharacterData ___data, Player ___player) {
            if(__instance != null && !__instance.isRespawning && !___data.dead
                && ___player.HasCard("Ouroboros")) {
                int count = ___player.CardCount("Ouroboros");
                Oroboros.oroboros += count;
                PlayerManager.instance.players.ForEach(player => {
                    int boost = player.CardCount("Ouroboros") * count * 10;
                    player.GetRootData().flatProjectileDamage += boost;
                    player.GetRootData().flatHPboost += boost;
                    if(GameManager.instance.battleOngoing) {
                        player.GetRootData().tempflatHPboost += boost;
                        player.data.maxHealth += boost;
                    }
                });
            }
        }

    }
}
	
