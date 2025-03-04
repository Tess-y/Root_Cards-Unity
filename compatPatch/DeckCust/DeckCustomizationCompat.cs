using BepInEx;
using HarmonyLib;
using RootCore;
using System;
using System.Collections.Generic;

[BepInDependency("pykess.rounds.plugins.deckcustomization", BepInDependency.DependencyFlags.HardDependency)]
[BepInPlugin("root.DeckCustomizationCompat", "DeckCustomizationCompat", "1.0.1")]
[BepInProcess("Rounds.exe")]
[Serializable]
[HarmonyPatch(typeof(DeckCustomization.DeckCustomization))]
[HarmonyPatch("allCards", MethodType.Getter)]
internal class DeckCustomizationCompat:BaseUnityPlugin {
    public void Awake() {
        new Harmony("root.DeckCustomizationCompat").PatchAll();
    }
    public static void Postfix(ref List<CardInfo> __result) {
        __result.RemoveAll(card => card is RootCardInfo rootCard && rootCard.Restricted);
    }
}

