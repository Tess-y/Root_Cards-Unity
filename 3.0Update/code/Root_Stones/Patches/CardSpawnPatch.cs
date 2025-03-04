using CardChoiceSpawnUniqueCardPatch.CustomCategories;
using HarmonyLib;
using ModdingUtils.Utils;
using Photon.Pun;
using RootCore;
using System.Collections.Generic;
using System.Linq;
using UnboundLib;
using UnityEngine;

namespace RootStones.Patches {

    [HarmonyPatch(typeof(CardChoice), "SpawnUniqueCard")]
    internal class CardSpawnPatch {
	
		private static void Postfix(List<GameObject> ___spawnedCards, int ___pickrID, ref GameObject __result) {
	
			var player = GetPlayerWithID(___pickrID);
			if(player == null || !player.HasCard("Mind_Stone")) return;

			int startingIndex = player.GetRootData().perpetualCard == null ? 0 : 1;

            List<CardInfo> cards = new List<CardInfo>();
            List<CardInfo> PlayerCards = player.data.currentCards.Where(c => Cards.instance.PlayerIsAllowedCard(player, c) &&
                !c.categories.Intersect(new CardCategory[] {
                         CustomCardCategories.instance.CardCategory("GearUp_Card-Shuffle"),
                         CustomCardCategories.instance.CardCategory("NoPreGamePick") //Shuffle has this, as far as i know it is the only card that does, and is the easyst way to blacklist shuffle.
                    }).Any() &&
                !c.IsHiddenCard()).ToList();
            if(___spawnedCards.Count == startingIndex) {
                cards = PlayerCards.Distinct().ToList();
                cards.Shuffle();
                cards.Add((CardInfo)typeof(CardChoiceSpawnUniqueCardPatch.CardChoiceSpawnUniqueCardPatch).GetField("NullCard", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic).GetValue(null));
                ReplaceCard(ref __result, cards[0]);
            }
            
			if(player.HasCard("Gauntlet")){
				if(___spawnedCards.Count == startingIndex + 1) {
					var raity = PlayerCards.ToList().Select(c => c.rarity).Min(r => RarityLib.Utils.RarityUtils.GetRarityData(r).relativeRarity);
					cards = PlayerCards.ToList().Where(card => RarityLib.Utils.RarityUtils.GetRarityData(card.rarity).relativeRarity == raity).Distinct().ToList();
					cards.Shuffle();
                    cards.Add((CardInfo)typeof(CardChoiceSpawnUniqueCardPatch.CardChoiceSpawnUniqueCardPatch).GetField("NullCard", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic).GetValue(null));
                    ReplaceCard(ref __result, cards[0]);
				} else if(___spawnedCards.Count == startingIndex + 2) {
					cards = PlayerCards.ToList().Where(card => PlayerCards.Count(card1 => card1 == card) == PlayerCards.Max(c => PlayerCards.Count(c1 => c1 == c))).Distinct().ToList();
					cards.Shuffle();
                    cards.Add((CardInfo)typeof(CardChoiceSpawnUniqueCardPatch.CardChoiceSpawnUniqueCardPatch).GetField("NullCard", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic).GetValue(null));
                    ReplaceCard(ref __result, cards[0]);
				}
            }
		}

		internal static void ReplaceCard(ref GameObject __result, CardInfo card) {
            GameObject old = __result;
            Core.instance.ExecuteAfterFrames(3, () => PhotonNetwork.Destroy(old));
            __result = PhotonNetwork.Instantiate(card.name, __result.transform.position, __result.transform.rotation);
        }
        internal static Player GetPlayerWithID(int playerID) {
            for(int i = 0; i < PlayerManager.instance.players.Count; i++) {
                if(PlayerManager.instance.players[i].playerID == playerID) {
                    return PlayerManager.instance.players[i];
                }
            }
            return null;
        }
    }
	
}
