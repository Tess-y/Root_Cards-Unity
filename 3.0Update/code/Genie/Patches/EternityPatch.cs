using HarmonyLib;
using ModdingUtils.Utils;
using RootCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Genie.Patches {
    [HarmonyPatch]
	public class EternityPatch {
	
	    [HarmonyPatch(typeof(ApplyCardStats), "OFFLINE_Pick")]
	    [HarmonyPrefix]
	    public static bool PickCardOffline(ApplyCardStats __instance, Player[] players) {
	        List<Player> array = players.ToList();
	        if(array.Any(p => p.GetRootData().lockedCard!=null)) {
	            array.ForEach(player => {
	                UnityEngine.Debug.Log(player.GetRootData().lockedCard);
	                if(player.GetRootData().lockedCard!=null&&player.GetRootData().lockedCard!=__instance.GetComponent<CardInfo>().sourceCard) {
	                    Cards.instance.AddCardToPlayer(player, player.GetRootData().lockedCard, addToCardBar: true);
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
	        List<Player> array = actorIDs.Select(PlayerManager.instance.GetPlayerWithActorID).ToList();
	        if(array.Any(p => p.GetRootData().lockedCard!=null)) {
	            array.ForEach(player => {
	                UnityEngine.Debug.Log(player.GetRootData().lockedCard);
	                if(player.GetRootData().lockedCard!=null&&player.GetRootData().lockedCard!=__instance.GetComponent<CardInfo>().sourceCard) {
	                    Cards.instance.AddCardToPlayer(player, player.GetRootData().lockedCard, addToCardBar: true);
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
	        if(player.GetRootData().lockedCard!=null)
	            card=player.GetRootData().lockedCard;
	    }
	}
}
