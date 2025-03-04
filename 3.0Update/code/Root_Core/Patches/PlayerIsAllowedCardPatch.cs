using HarmonyLib;
using ModdingUtils.Utils;
using System.Collections.Generic;
using UnboundLib;
using UnityEngine;

namespace RootCore.Patches {
    [HarmonyPatch(typeof(Cards), "PlayerIsAllowedCard")]
	public class AllowedCardPatch {
	    private static void Postfix(Player player, CardInfo card, ref bool __result, Cards __instance) {
            if(card is null || player is null)
                return;
            if(player.GetRootData().simple) {
	            __result = __result && card.rarity == CardInfo.Rarity.Common;
	        }

			if(card is RootCardInfo cardInfo) {
				if(cardInfo.PickPhaseOnly) {
					if(!CardChoice.instance.isPlaying || CardChoice.instance.picks < 1 || CardChoice.instance.spawnedCards.Count >= CardChoice.instance.transform.childCount) {
						__result = false;
					}
				}
			}
	    }
	}
}
