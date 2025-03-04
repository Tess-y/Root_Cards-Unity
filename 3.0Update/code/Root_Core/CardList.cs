using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RootCore {
    public class CardList:MonoBehaviour {
        public RootCardInfo[] CardsToRegester;
        internal static Dictionary<string, RootCardInfo> ModCards = new Dictionary<string, RootCardInfo>();
        public static RootCardInfo GetCardInfo(string cardName) {
            return ModCards == null || !ModCards.ContainsKey(cardName) ? null : ModCards[cardName];
        }

        public static List<RootCardInfo> GetCardsWithCondition(Func<RootCardInfo, bool> condition) {
            return ModCards.Values.Where(condition).ToList();
        }
    }
}
