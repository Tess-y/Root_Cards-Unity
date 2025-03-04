using ItemShops.Extensions;
using System;
using System.Collections.Generic;

namespace RootCore {
    public class TabInfoRegesterer {
        static bool canRegester = false;
        public static string cat = "Root Stats";
        public static void RegesterInfo(string category, string name, Func<Player, bool> displayCondition, Func<Player, string> displayValue, int priority = 6) {
            if(!canRegester) return;
            InternalRegester(category, name, displayCondition, displayValue, priority);
        }
        internal static void InternalRegester(string category, string name, Func<Player, bool> displayCondition, Func<Player, string> displayValue, int priority) {
            TabinfoInterface.RegesterInfo(category, name, displayCondition, displayValue, priority);
        }

        public static void Setup() {
            canRegester = true;
            TabinfoInterface.SetUp();
            RegesterInfo(cat, "ROOT:", (p) => p.GetRootData().SteamID == "76561198060618523", (p) => "Player Is Root", 6);
            RegesterInfo(cat, "Eternal Card", (p) => p.GetRootData().lockedCard != null, (p) => $"{p.GetRootData().lockedCard.cardName}", 6);
            RegesterInfo(cat, "Wishes", (p) => p.GetAdditionalData().bankAccount.HasFunds(new Dictionary<string, int> { { "Wish", 1 } }), (p) => $"{p.GetAdditionalData().bankAccount.Money["Wish"]}", 6);
            RegesterInfo(cat, "Block Efectiveness", (p) => p.GetRootData().shieldEfectiveness != 1, (p) => $"{Math.Round(p.GetRootData().shieldEfectiveness * 100, 1)}", 6);
            RegesterInfo(cat, "HP Culling", (p) => p.GetRootData().hpCulling != 0, (p) => $"{Math.Round(p.GetRootData().hpCulling * 100, 1)}%", 6);
        }
    }
}
