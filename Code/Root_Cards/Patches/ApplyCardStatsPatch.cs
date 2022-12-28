using System.Linq;
using HarmonyLib;
using Nullmanager;
using UnityEngine;

[HarmonyPatch(typeof(ApplyCardStats), "ApplyStats")]
internal class ApplyCardStatsPatch {
    private static void Postfix(ApplyCardStats __instance, Player ___playerToUpgrade) {

        if(___playerToUpgrade.data.stats.GetRootData().knowledge>0) {
            if(__instance.GetComponent<CardInfo>().sourceCard is NullCardInfo||__instance.GetComponent<CardInfo>() is NullCardInfo) {
                ___playerToUpgrade.data.stats.GetRootData().knowledge--;
            } else {
                ___playerToUpgrade.data.stats.AjustNulls((int)Mathf.Ceil(NullManager.instance.GetNullValue(__instance.GetComponent<CardInfo>().rarity)*1.5f));
            }
        }
        var aad = __instance.GetComponent<RootStatModifiers>();
        if(aad!=null) {
            aad.Apply(___playerToUpgrade);
        }
        __instance.GetComponents<OnAddEffect>().ToList().ForEach(effect => { effect.Run(___playerToUpgrade); });
    }
}
