using UnityEngine;
using ModdingUtils.Utils;
using HarmonyLib;
using UnboundLib;
using System.Collections.Generic;
using Nullmanager;

[HarmonyPatch(typeof(Cards), "PlayerIsAllowedCard")]
public class AllowedCardPatch {
    private static void Postfix(Player player, CardInfo card, ref bool __result) {
        if(card is null)
            return;
        if(card is RootCardInfo cardInfo&&cardInfo.PickPhaseOnly) {
            if(!(bool)CardChoice.instance.GetFieldValue("isPlaying")||CardChoice.instance.picks<1||((List<GameObject>)CardChoice.instance.GetFieldValue("spawnedCards")).Count>=CardChoice.instance.transform.childCount) {
                __result=false;
            }
        }
        if(player != null&&player.data.stats.GetRootData().knowledge>0) {
            if(!card.IsNullable()||card.IsAntiCard())
                __result=false;
        }
    }
}