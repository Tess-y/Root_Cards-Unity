using HarmonyLib;
using RootCore;
using System.Collections.Generic;
using System.Linq;
using UnboundLib.GameModes;

namespace RootAdvancedCards.Patches {
    [HarmonyPatch(typeof(ApplyCardStats))]
	public class ApplyCardStatsPatches {

		[HarmonyPatch(nameof(ApplyCardStats.RPCA_Pick))]
		[HarmonyPrefix]
		public static void RPCA_Pick(ref int[] actorIDs, ApplyCardStats __instance) {
			List<int> newIDs = actorIDs.ToList();
			foreach(Player player in PlayerManager.instance.players) {
				if(player.HasCard("Manifest_Destiny") && !newIDs.Contains(player.data.view.ControllerActorNr) && GameModeManager.CurrentHandler.GetRoundWinners().Contains(player.teamID) && player.IsAllowedCard(__instance.GetComponent<CardInfo>())) {
					newIDs.Add(player.data.view.ControllerActorNr);
				}
			}
			actorIDs = newIDs.ToArray();
		}

		[HarmonyPatch(nameof(ApplyCardStats.OFFLINE_Pick))]
		[HarmonyPrefix]
		public static void OFFLINE_Pick(ref Player[] players, ApplyCardStats __instance) {
			List<Player> newIDs = players.ToList();
            foreach(Player player in PlayerManager.instance.players) {
                if(player.HasCard("Manifest_Destiny") && !newIDs.Contains(player) && GameModeManager.CurrentHandler.GetRoundWinners().Contains(player.teamID) && player.IsAllowedCard(__instance.GetComponent<CardInfo>())) {
                    newIDs.Add(player);
                }
            }
			players = newIDs.ToArray();
		}

	}
}
