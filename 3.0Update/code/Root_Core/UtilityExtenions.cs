using ModdingUtils.Utils;
using System;
using System.Linq;
using UnboundLib;
using UnboundLib.Utils;
using UnityEngine;

namespace RootCore {
    public static class UtilityExtenions {

        public static bool HasCard(this Player player, string cardKey) {
            return CardList.ModCards.ContainsKey(cardKey) && player.HasCard(CardList.GetCardInfo(cardKey));
        }
        public static bool HasCard(this Player player, CardInfo card) {
            return player.data.currentCards.Contains(card);
        }
        public static int CardCount(this Player player, string cardKey) {
            return CardList.ModCards.ContainsKey(cardKey)? player.CardCount(CardList.GetCardInfo(cardKey)):0;
        }
        public static int CardCount(this Player player, CardInfo card) {
            return player.data.currentCards.Count(c=> c==card);
        }

        public static bool OpponentHasCard(this Player player, string cardKey) {
            return CardList.ModCards.ContainsKey(cardKey) && player.OpponentHasCard(CardList.GetCardInfo(cardKey));
        }
        public static bool OpponentHasCard(this Player player, CardInfo card) {
            return PlayerManager.instance.players.Any(p => p.teamID != player.teamID && p.HasCard(card));
        }

        public static bool TeammateHasCard(this Player player, string cardKey) {
            return CardList.ModCards.ContainsKey(cardKey) && player.TeammateHasCard(CardList.GetCardInfo(cardKey));
        }
        public static bool TeammateHasCard(this Player player, CardInfo card) {
            return PlayerManager.instance.players.Any(p => p.teamID == player.teamID && p.playerID != player.playerID && p.HasCard(card));
        }

        public static void GiveCard(this Player player, string cardKey) {
            player.GiveCard(CardList.GetCardInfo(cardKey));
        }
        public static void GiveCard(this Player player, CardInfo card) {
            Cards.instance.AddCardToPlayer(player, card);
            CardBarUtils.instance.ShowAtEndOfPhase(player, card);
        }
        public static bool IsAllowedCard(this Player player, CardInfo card) {
            return Cards.instance.PlayerIsAllowedCard(player, card);
        }
        public static bool IsHiddenCard(this CardInfo card) {

            return !CardManager.cards.Values.Any(c => c.cardInfo == card);
        }


        public static void RPC_Others(this MonoBehaviour behaviour, string method, params object[] data) {
                NetworkingManager.RPC_Others(behaviour.GetType(), method, data);
        }
        public static void RPC(this MonoBehaviour behaviour, string method, params object[] data) {
                NetworkingManager.RPC(behaviour.GetType(), method, data);
        }

        public static void RemoveCardFromCardBar(this Player player, CardInfo card) {
            CardBar bar = CardBarUtils.instance.PlayersCardBar(player);
            var Buttons = bar.GetComponentsInChildren<CardBarButton>().Where(b => b.card == card);
            foreach ( var button in Buttons ) {
                GameObject.Destroy(button.gameObject);
            }
        }

        public static void Show(this CardInfo card, Player player = null) {
            if(player == null) {
                player = PlayerManager.instance.players[0];
            }
            Core.instance.StartCoroutine(CardBarUtils.instance.ShowImmediate(player,card));
        }


        public static bool ContainsOR(this string str, params string[] checks) {
            if(checks.Length == 0) return true;
            foreach(var check in checks) {
                if(str.Contains(check)) return true;
            }
            return false;
        }

        public static bool ContainsAND(this string str, params string[] checks) {
            if(checks.Length == 0) return true;
            foreach(var check in checks) {
                if(!str.Contains(check)) return false;
            }
            return true;
        }

        public static bool ContainsXOR(this string str, params string[] checks) {
            if(checks.Length < 2) return false;
            bool found = false;
            foreach(var check in checks) {
                if(str.Contains(check)) {
                    if(found) return false;
                    found = true;
                }
            }
            return found;
        }

        public static int IndexOf(this Array array, object value) {
            return Array.IndexOf(array, value);
        }

    }
}
