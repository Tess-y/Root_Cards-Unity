using UnityEngine;
using ModdingUtils.Utils;
using HarmonyLib;
using UnboundLib;
using System.Collections.Generic;
using Nullmanager;
using System.Linq;

[HarmonyPatch(typeof(Cards), "PlayerIsAllowedCard")]
public class AllowedCardPatch {
    private static void Postfix(Player player, CardInfo card, ref bool __result) {
        if(card is null)
            return;
        if(card is RootCardInfo cardInfo) {
            if(cardInfo.PickPhaseOnly) {
                if(!(bool)CardChoice.instance.GetFieldValue("isPlaying") || CardChoice.instance.picks < 1 || ((List<GameObject>)CardChoice.instance.GetFieldValue("spawnedCards")).Count >= CardChoice.instance.transform.childCount) {
                    __result = false;
                }
            }
            if(cardInfo.Key == "Cake_Toggle") {
                __result = false;
            }
            if(cardInfo.Restricted && cardInfo.Key.StartsWith("Cake_") && !UnboundLib.Utils.CardManager.IsCardActive(CardResgester.ModCards["Cake_Toggle"])) {
                __result = false;
            }
            if(cardInfo.Key.StartsWith("Cake_") && ((List<GameObject>)CardChoice.instance.GetFieldValue("spawnedCards")).Any(spawnedCard 
                => spawnedCard.GetComponent<RootCardInfo>() is RootCardInfo rootcard && rootcard.Key.StartsWith("Cake_"))) {
                __result = false;
            }
        }
        if(player != null&&player.data.stats.GetRootData().knowledge>0) {
            if(!card.IsNullable()||card.IsAntiCard())
                __result=false;
        }
    }
}