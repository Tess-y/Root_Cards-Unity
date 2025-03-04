using HarmonyLib;
using ModdingUtils.Utils;
using RootCore;
using System;
using System.Linq;

namespace RootAdvancedCards.Patches {
    [Serializable]
    [HarmonyPatch(typeof(Cards))]
    public class PerminentCardPatch {

        [HarmonyPatch(nameof(Cards.AddCardToPlayer), typeof(Player), typeof(CardInfo), typeof(bool), typeof(string), typeof(float), typeof(float), typeof(bool))]
        [HarmonyPrefix]
        public static bool AddPrefix(Player player, CardInfo card) {
            return !(card is RootCardInfo cardInfo && cardInfo.Perminent && player.HasCard(card));
        }
        [HarmonyPatch(nameof(Cards.RPCA_AssignCard), typeof(string), typeof(int), typeof(bool), typeof(string), typeof(float), typeof(float), typeof(bool))]
        [HarmonyPrefix]
        public static bool RPCAddPrefix(string cardObjectName, int playerID) {
            CardInfo card = Cards.instance.GetCardWithObjectName(cardObjectName);
            Player player = PlayerManager.instance.GetPlayerWithID(playerID);
            return !(card is RootCardInfo cardInfo && cardInfo.Perminent && player.HasCard(card));
        }

        [HarmonyPatch(nameof(Cards.RemoveAllCardsFromPlayer))]
        public static void Prefix(Player player, out CardInfo[] __state) {
            __state = player.data.currentCards.Where(card => card is RootCardInfo cardInfo && cardInfo.Perminent).ToArray();
        }
        [HarmonyPatch(nameof(Cards.RemoveAllCardsFromPlayer))]
        public static void Postfix(Player player, ref CardInfo[] __state) {
            Cards.instance.AddCardsToPlayer(player, __state, false);
        }
    }
}
