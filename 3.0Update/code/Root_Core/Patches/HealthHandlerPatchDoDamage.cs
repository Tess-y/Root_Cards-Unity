using HarmonyLib;
using System;
using UnboundLib;
using UnityEngine;

namespace RootCore.Patches {
    [Serializable]
	[HarmonyPatch(typeof(HealthHandler), "DoDamage")]
	internal class HealthHandlerPatchDoDamage {
		[HarmonyPriority(0)]
		private static void Prefix(HealthHandler __instance, ref Vector2 damage) {

			if(__instance.stats.GetRootData().damageCap > 0) {
				damage = clampMagnatued(damage,
					(__instance.data.maxHealth * __instance.stats.GetRootData().damageCap) - __instance.stats.GetRootData().damageCapFilled);
			}
		}

		private static void Postfix(HealthHandler __instance, Vector2 damage) {
			if(__instance.stats.GetRootData().damageCapWindow > 0){
				__instance.stats.GetRootData().damageCapFilled += damage.magnitude;
				Core.instance.ExecuteAfterSeconds(__instance.stats.GetRootData().damageCapWindow, () =>
				__instance.stats.GetRootData().damageCapFilled -= damage.magnitude);
			}
		}

		private static Vector2 clampMagnatued(Vector2 vector, float max) {
			float oldMag = vector.magnitude;
			if(oldMag <= max) return vector;
			return vector.normalized * max;
		}
	}
}

