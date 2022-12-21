using HarmonyLib;
using UnityEngine;
using UnboundLib;

[HarmonyPatch(typeof(HealthHandler), "DoDamage")]
public class DoDamagePatch{
    private static void Postfix(HealthHandler __instance, Vector2 damage, Player damagingPlayer)
        {
            if (damagingPlayer == null || damagingPlayer.data.stats.GetRootData().hpCulling <= 0) return;
            Player player = (Player)Traverse.Create(__instance).Field("player").GetValue();
            player.gameObject.GetOrAddComponent<HealthCurse>().Cull(damage.magnitude * damagingPlayer.data.stats.GetRootData().hpCulling);
        }
}