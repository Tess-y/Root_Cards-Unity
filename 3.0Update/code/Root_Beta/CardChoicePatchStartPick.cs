using HarmonyLib;
using RootCore;
using System;

namespace RootStandardCards.Patches {
    [Serializable]
    [HarmonyPatch(typeof(CardChoice), nameof(CardChoice.StartPick))]
    public class CardChoicePatchStartPick {
        public static void Prefix(CardChoice __instance, ref int picksToSet, int pickerIDToSet) {
            if(PlayerManager.instance.GetPlayerWithID(pickerIDToSet).HasCard("Omniscience"))
                picksToSet += 2;
            if(PlayerManager.instance.GetPlayerWithID(pickerIDToSet).HasCard("Leftovers")) picksToSet = -999;
        }
    }

}
