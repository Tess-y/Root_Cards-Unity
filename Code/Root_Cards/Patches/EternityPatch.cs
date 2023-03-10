using HarmonyLib;
using System.Linq;
using UnityEngine;
using UnboundLib;
using System.Collections.Generic;
using ModdingUtils.Utils;
using System;

[HarmonyPatch]
public class EternityPatch {

    [HarmonyPatch(typeof(ApplyCardStats), "OFFLINE_Pick")]
    [HarmonyPrefix]
    public static bool PickCardOffline(ApplyCardStats __instance, Player[] players) {
        List<Player> array = players.ToList();
        if(array.Any(p => p.data.stats.GetRootData().lockedCard!=null)) {
            array.ForEach(player => {
                UnityEngine.Debug.Log(player.data.stats.GetRootData().lockedCard);
                if(player.data.stats.GetRootData().lockedCard!=null&&player.data.stats.GetRootData().lockedCard!=__instance.GetComponent<CardInfo>().sourceCard) {
                    Cards.instance.AddCardToPlayer(player, player.data.stats.GetRootData().lockedCard, addToCardBar: true);
                } else {
                    Cards.instance.AddCardToPlayer(player, __instance.GetComponent<CardInfo>().sourceCard, addToCardBar: true);
                }
            });
            return false;
        }
        return true;
    }
    [HarmonyPatch(typeof(ApplyCardStats), "RPCA_Pick")]
    [HarmonyPrefix]
    public static bool PickCard(ApplyCardStats __instance, int[] actorIDs) {
        List<Player> array = actorIDs.Select(id=> (Player)PlayerManager.instance.InvokeMethod("GetPlayerWithActorID",id)).ToList();
        if(array.Any(p => p.data.stats.GetRootData().lockedCard!=null)) {
            array.ForEach(player => {
                UnityEngine.Debug.Log(player.data.stats.GetRootData().lockedCard);
                if(player.data.stats.GetRootData().lockedCard!=null&&player.data.stats.GetRootData().lockedCard!=__instance.GetComponent<CardInfo>().sourceCard) {
                    Cards.instance.AddCardToPlayer(player, player.data.stats.GetRootData().lockedCard, addToCardBar: true);
                } else {
                    Cards.instance.AddCardToPlayer(player, __instance.GetComponent<CardInfo>().sourceCard, addToCardBar: true);
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