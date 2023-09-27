using UnityEngine;
using ModdingUtils.Utils;
using HarmonyLib;
using UnboundLib;
using System.Collections.Generic;
using Nullmanager;
using System.Linq;

[HarmonyPatch(typeof(Cards), "PlayerIsAllowedCard")]
public class AllowedCardPatch {
    private static bool CakeCheck = false;
    private static void Postfix(Player player, CardInfo card, ref bool __result, Cards __instance) {
        if(card is null || player is null || CakeCheck)
            return;
        if(player.data.stats.GetRootData().simple) {
            __result = __result && card.rarity == CardInfo.Rarity.Common;
        }

        if(card is RootCardInfo cardInfo) {
            if(cardInfo.PickPhaseOnly) {
                if(!(bool)CardChoice.instance.GetFieldValue("isPlaying") || CardChoice.instance.picks < 1 || ((List<GameObject>)CardChoice.instance.GetFieldValue("spawnedCards")).Count >= CardChoice.instance.transform.childCount) {
                    __result = false;
                }
            }
            if(cardInfo.Key == "Cake_Toggle") {
                __result = false;
            }
            if(cardInfo.Key.StartsWith("Cake_")) {
                if(cardInfo.Restricted && !UnboundLib.Utils.CardManager.IsCardActive(CardResgester.ModCards["Cake_Toggle"])) {
                    __result = false;
                } else if(!((List<GameObject>)CardChoice.instance.GetFieldValue("spawnedCards")).Any(spawnedCard
                    => spawnedCard is null) && ((List<GameObject>)CardChoice.instance.GetFieldValue("spawnedCards")).Any(spawnedCard
                    => spawnedCard.GetComponent<RootCardInfo>() is RootCardInfo rootcard && rootcard.Key.StartsWith("Cake_"))) {
                    __result = false;
                } else {

                    CakeCheck = true;
                    __result = __result && __instance.PlayerIsAllowedCard(player, CardResgester.ModCards["Cake_Toggle"]);
                    CakeCheck = false;
                }
            }
        }
        if(player.data.stats.GetRootData().knowledge>0) {
            if(!card.IsNullable()||card.IsAntiCard())
                __result=false;
        }
    }
}