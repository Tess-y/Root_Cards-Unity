using HarmonyLib;
using Nullmanager;
using RootCore;
using UnityEngine;

namespace RootNulledCards.Patches {
    [HarmonyPatch(typeof(ApplyCardStats), "ApplyStats")]
	internal class ApplyCardStatsPatch {
	    private static void Postfix(ApplyCardStats __instance, Player ___playerToUpgrade) {
	
	        if(___playerToUpgrade.GetRootData().knowledge>0) {
	            if(__instance.GetComponent<CardInfo>().sourceCard is NullCardInfo||__instance.GetComponent<CardInfo>() is NullCardInfo) {
	                ___playerToUpgrade.GetRootData().knowledge--;
	            } else {
	                ___playerToUpgrade.data.stats.AjustNulls((int)Mathf.Ceil(0.5f+NullManager.instance.GetNullValue(__instance.GetComponent<CardInfo>().rarity)*1.5f));
	            }
	        }
	    }
	}
}
