using HarmonyLib;
using System;
using UnityEngine;
using UnboundLib;

[Serializable]
[HarmonyPatch(typeof(Block), "blocked")]
internal class BlockPatchblocked {
    private static void Prefix(Block __instance, UnityEngine.GameObject projectile, Vector3 forward, Vector3 hitPos) {
        bool destroy = false;
        ProjectileHit proj = projectile.GetComponent<ProjectileHit>();
        HealthHandler healthHandler = (HealthHandler)Traverse.Create(__instance).Field("health").GetValue();
        Player player = (Player)Traverse.Create(healthHandler).Field("player").GetValue();
        if(player.data.stats.GetRootData().witchTimeDuration>0&&proj.ownPlayer.teamID!=player.teamID) {
            player.gameObject.GetOrAddComponent<WitchTime>().time_remaning=player.data.stats.GetRootData().witchTimeDuration;
        }
        if(player.data.stats.GetRootData().shieldEfectiveness<1) {
            Vector2 damage = ((proj.bulletCanDealDeamage ? proj.damage : 1f)-((proj.bulletCanDealDeamage ? proj.damage : 1f)*player.data.stats.GetRootData().shieldEfectiveness))*forward.normalized;
            healthHandler.TakeDamage(damage, hitPos, proj.projectileColor, proj.ownWeapon, proj.ownPlayer, true, true);

            destroy=true;
        }
        if(destroy) {
            // destroy the bullet
            UnityEngine.GameObject.Destroy(projectile);
        }
    }
}