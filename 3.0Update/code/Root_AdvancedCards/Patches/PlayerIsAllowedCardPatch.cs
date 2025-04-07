using HarmonyLib;
using ModdingUtils.Utils;
using RootCore;
using UnityEngine;

namespace RootAdvancedCards.Patches {
    [HarmonyPatch(typeof(Cards), "PlayerIsAllowedCard")]
	public class AllowedCardPatch {
	    private static bool CakeCheck = false;
	    private static void Postfix(Player player, CardInfo card, ref bool __result, Cards __instance) {
	        if(card is null || player is null || CakeCheck)
	            return;
	        if(card is RootCardInfo cardInfo) {
	            if(cardInfo.Key == "Cake_Toggle") {
	                __result = false;
	            }
	            if(cardInfo.Key.StartsWith("Cake_")) {
	                if(cardInfo.Restricted && !UnboundLib.Utils.CardManager.IsCardActive(CardList.GetCardInfo("Cake_Toggle"))) {
	                    __result = false;
	                } else {
						if(CardChoice.instance.spawnedCards != null){
							foreach(GameObject go in CardChoice.instance.spawnedCards) {
                                if(go.GetComponent<CardInfo>() is RootCardInfo rc && rc.Key.StartsWith("Cake_")) {
                                    __result = false;
                                    return;
                                }
							}
						}
	
	                    CakeCheck = true;
	                    __result = __result && CakeRestrictions.CanHaveCake(player,cardInfo) && __instance.PlayerIsAllowedCard(player, CardList.GetCardInfo("Cake_Toggle"));
	                    CakeCheck = false;
	                }
	            }
	        }
	    }
	}
}
