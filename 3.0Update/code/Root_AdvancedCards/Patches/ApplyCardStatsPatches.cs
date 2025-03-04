using HarmonyLib;
using RootCore;
using System.Collections.Generic;
using System.Linq;

namespace RootAdvancedCards.Patches {
    [HarmonyPatch(typeof(ApplyCardStats))]
	public class ApplyCardStatsPatches {

		[HarmonyPatch("RPCA_Pick")]
		[HarmonyPrefix]
		public static void RPCA_Pick(ref int[] actorIDs) {
			List<int> newIDs = actorIDs.ToList();
			for(int i = 0; i < actorIDs.Length; i++) {
				Player playerToUpgrade = PlayerManager.instance.GetPlayerWithActorID(actorIDs[i]);
				if(playerToUpgrade != null && playerToUpgrade.HasCard("Simulacrum")) {
					newIDs.Add(playerToUpgrade.data.view.ControllerActorNr);
				}
			}
			actorIDs = newIDs.ToArray();
		}

		[HarmonyPatch("OFFLINE_Pick")]
		[HarmonyPrefix]
		public static void OFFLINE_Pick(ref Player[] players) {
			List<Player> newIDs = players.ToList();
			for(int i = 0; i < players.Length; i++) {
				Player playerToUpgrade = players[i];
				if(playerToUpgrade != null && playerToUpgrade.HasCard("Simulacrum")) {
					newIDs.Add(playerToUpgrade);
				}
			}
			players = newIDs.ToArray();
		}

	}
}
