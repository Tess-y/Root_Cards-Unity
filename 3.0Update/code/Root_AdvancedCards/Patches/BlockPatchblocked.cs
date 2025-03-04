using HarmonyLib;
using RootCore;
using System;
using UnboundLib;
using UnityEngine;

namespace RootAdvancedCards.Patches {
    [Serializable]
	[HarmonyPatch(typeof(Block), "blocked")]
	internal class BlockPatchblocked {
	    private static void Prefix(Block __instance, UnityEngine.GameObject projectile, Vector3 forward, Vector3 hitPos) {
	        ProjectileHit proj = projectile.GetComponent<ProjectileHit>();
	        HealthHandler healthHandler = (HealthHandler)Traverse.Create(__instance).Field("health").GetValue();
	        Player player = (Player)Traverse.Create(healthHandler).Field("player").GetValue();
	        if(player.GetRootData().witchTimeDuration>0&&proj.ownPlayer.teamID!=player.teamID) {
	            player.gameObject.GetOrAddComponent<WitchTime>().time_remaning=player.GetRootData().witchTimeDuration;
	        }
	    }
	}
}
