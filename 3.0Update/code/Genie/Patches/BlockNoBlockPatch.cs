using HarmonyLib;
using UnityEngine;

namespace Genie.Patches {
    [HarmonyPatch(typeof(Block))]
	public class BlockNoBlockPatch : MonoBehaviour
	{
		[HarmonyPrefix]
		[HarmonyPatch(nameof(Block.RPCA_DoBlock))]
		public static bool norpca(Block __instance) {
			return __instance.GetComponentInChildren<NoBlock>() == null;
		}
		[HarmonyPrefix]
		[HarmonyPatch(nameof(Block.TryBlock))]
		public static bool notry(Block __instance) {
			return __instance.GetComponentInChildren<NoBlock>() == null;
		}
		[HarmonyPrefix]
		[HarmonyPatch(nameof(Block.DoBlock))]
		public static bool nodo(Block __instance) {
			return __instance.GetComponentInChildren<NoBlock>() == null;
		}
		[HarmonyPostfix]
		[HarmonyPatch(nameof(Block.IsBlocking))]
		public static void IsBlocking(Block __instance, ref bool __result) {
            __result = __result && __instance.GetComponentInChildren<NoBlock>() == null;
		}
		
	}
}
