using HarmonyLib;
using ModdingUtils.Utils;
using Nullmanager;
using RootCore;
using System.Linq;

namespace RootNulledCards.Patches {
    [HarmonyPatch(typeof(Cards), "PlayerIsAllowedCard")]
	public class AllowedCardPatch {
	    private static void Postfix(Player player, CardInfo card, ref bool __result, Cards __instance) {
            if(card is null || player is null)
                return;
            if(player.GetRootData().knowledge>0) {
	            if(!card.IsNullable()||card.IsAntiCard())
	                __result=false;
	        }
			if(card == CardList.GetCardInfo("Integraty") && !player.data.currentCards.Any(c => c is NullCardInfo)) {
				__result=false;
			}
	    }
	}
}
