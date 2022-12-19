using UnityEngine;
using ModdingUtils.Utils;
using HarmonyLib;
using UnboundLib;
using System.Collections.Generic;

[HarmonyPatch(typeof(Cards),"PlayerIsAllowedCard")]
public class AllowedCardPatch{
    private static void Postfix(CardInfo card, ref bool __result){
        if(card is RootCardInfo cardInfo && cardInfo.PickPhaseOnly){
        if(!(bool)CardChoice.instance.GetFieldValue("isPlaying") || CardChoice.instance.picks < 1 || ((List<GameObject>)CardChoice.instance.GetFieldValue("spawnedCards")).Count >= CardChoice.instance.transform.childCount){
            __result = false;
        }
        }
    }
}