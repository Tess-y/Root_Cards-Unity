using HarmonyLib;
using RootCore;
using System;
using UnityEngine;

namespace RootStandardCards.Patches {
    [Serializable]
	[HarmonyPatch(typeof(Block), nameof(Block.blocked))]
	internal class BlockPatchblocked {
		private static void Prefix(Block __instance, UnityEngine.GameObject projectile, Vector3 forward, Vector3 hitPos) {
			bool destroy = false;
			ProjectileHit proj = projectile.GetComponent<ProjectileHit>();
			HealthHandler healthHandler = (HealthHandler)Traverse.Create(__instance).Field("health").GetValue();
			Player player = (Player)Traverse.Create(healthHandler).Field("player").GetValue();
			if(player.GetRootData().shieldEfectiveness < 1) {
				Vector2 damage = ((proj.bulletCanDealDeamage ? proj.damage : 1f) - ((proj.bulletCanDealDeamage ? proj.damage : 1f) * player.GetRootData().shieldEfectiveness)) * forward.normalized;
				healthHandler.TakeDamage(damage, hitPos, proj.projectileColor, proj.ownWeapon, proj.ownPlayer, true, true);

				destroy = true;
			}
			if(destroy) {
				// destroy the bullet
				UnityEngine.GameObject.Destroy(projectile);
			}
		}
	}
}
