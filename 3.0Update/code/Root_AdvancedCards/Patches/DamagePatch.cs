using HarmonyLib;
using RootCore;
using System;
using UnityEngine;

namespace RootAdvancedCards.Patches {
    [Serializable]
    [HarmonyPatch(typeof(HealthHandler))]
    internal class DamagePatch {
        [HarmonyPatch(methodName: "CallTakeDamage")]
        [HarmonyPatch(methodName: "TakeDamage", argumentTypes: new Type[] { typeof(Vector2), typeof(Vector2), typeof(GameObject), typeof(Player), typeof(bool), typeof(bool) })]
        [HarmonyPatch(methodName: "TakeDamage", argumentTypes: new Type[] { typeof(Vector2), typeof(Vector2), typeof(Color), typeof(GameObject), typeof(Player), typeof(bool), typeof(bool) })]
        [HarmonyPatch(methodName: "DoDamage")]
        [HarmonyPatch(methodName: "TakeDamageOverTime")]
        [HarmonyPriority(int.MaxValue)]
        private static void Prefix(ref Player damagingPlayer) {
			if(damagingPlayer != null && damagingPlayer.HasCard("Anonymity"))
				damagingPlayer = null;
	    }
	}
}
