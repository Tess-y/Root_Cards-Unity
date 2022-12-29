using HarmonyLib;
using System.Linq;
using UnityEngine;
using UnboundLib;
using System.Collections.Generic;
using ModdingUtils.Utils;
using System;

[HarmonyPatch]
public class EternityPatch {

    [HarmonyPatch(typeof(ApplyCardStats), "Pick")]
    [HarmonyPrefix]
    public static bool PickCard(ApplyCardStats __instance, int pickerID, bool forcePick, PickerType pickerType) {
        UnityEngine.Debug.Log("EternityPatch");
        UnityEngine.Debug.Log(__instance.GetComponent<CardInfo>().sourceCard);
        if(pickerType==PickerType.Player) {
            Player player = PlayerManager.instance.players.Find(p => p.playerID==pickerID);
            if(player.data.stats.GetRootData().lockedCard!=null&&player.data.stats.GetRootData().lockedCard!=__instance.GetComponent<CardInfo>().sourceCard) {
                player.data.stats.GetRootData().lockedCard.GetComponent<ApplyCardStats>().Pick(player.playerID, true, PickerType.Player);
                UnityEngine.Debug.Log("false");
                return false;
            }
            UnityEngine.Debug.Log("true");
            return true;
        }
        List<Player> array = PlayerManager.instance.GetPlayersInTeam(pickerID).ToList();
        if(array.Any(p => p.data.stats.GetRootData().lockedCard!=null)) {
            array.ForEach(player => {
                UnityEngine.Debug.Log(player.data.stats.GetRootData().lockedCard);
                if(player.data.stats.GetRootData().lockedCard!=null&&player.data.stats.GetRootData().lockedCard!=__instance.GetComponent<CardInfo>().sourceCard) {
                    player.data.stats.GetRootData().lockedCard.GetComponent<ApplyCardStats>().Pick(player.playerID, true, PickerType.Player);
                } else {
                    __instance.Pick(player.playerID, forcePick, PickerType.Player);
                }
            });
            return false;
        }
        return true;
    }

    [HarmonyPatch(typeof(Cards), "AddCardToPlayer", new Type[] { typeof(Player), typeof(CardInfo), typeof(bool), typeof(string), typeof(float), typeof(float), typeof(bool) })]
    [HarmonyPrefix]
    public static void Add(Player player, ref CardInfo card) {
        if(player.data.stats.GetRootData().lockedCard!=null)
            card=player.data.stats.GetRootData().lockedCard;
    }
}