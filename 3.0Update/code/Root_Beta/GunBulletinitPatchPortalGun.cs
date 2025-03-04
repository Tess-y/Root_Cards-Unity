using HarmonyLib;
using RootCore;
using System.Collections.Generic;
using UnboundLib;
using UnityEngine;

namespace RootAdvancedCards.GunEffects {

    [HarmonyPatch(typeof(Gun), nameof(Gun.BulletInit))]
    public class GunBulletinitPatchPortalGun {
        public static Dictionary<Player, GameObject> Bulets = new Dictionary<Player, GameObject>();
        public static Dictionary<Player, List<GameObject>> Bulet2 = new Dictionary<Player, List<GameObject>>();
        public static void Prefix(Gun __instance, GameObject bullet) {
            if(!Bulets.ContainsKey(__instance.player)) Bulets[__instance.player] = null;
            if(!Bulet2.ContainsKey(__instance.player)) Bulet2[__instance.player] = new List<GameObject>();
            if(__instance.holdable == null) return;
            if(__instance.player != null && __instance.player.HasCard("Unstable_Portal")) {
                //bullet.GetComponentInChildren<Collider2D>().enabled = false;
                GameObject oldBullet = Bulets[__instance.player];
                if(((UnityEngine.Object)oldBullet) != null) {
                    Vector3 vector = MainCam.instance.GetComponent<Camera>().WorldToScreenPoint(oldBullet.transform.position);
                    vector.x /= Screen.width;
                    vector.y /= Screen.height;
                    vector = new Vector3(Mathf.Clamp(vector.x, 0f, 1f), Mathf.Clamp(vector.y, 0f, 1f), vector.z);
                    if(vector.x > 0f && vector.x < 1f && vector.y < 1f && vector.y > 0f) {
                        bullet.transform.position = oldBullet.transform.position - (oldBullet.transform.rotation.eulerAngles.normalized * 0.3f);
                    }
                    oldBullet.GetComponentInChildren<SpawnedAttack>().SetColor(Color.red);
                }
                Core.instance.ExecuteAfterFrames(2, () => bullet.GetComponentInChildren<SpawnedAttack>().SetColor(Color.cyan));
                Bulets[__instance.player] = bullet;
            }
            Bulet2[__instance.player].Add(bullet);
        }

    }

    [HarmonyPatch(typeof(ProjectileCollision), nameof(ProjectileCollision.TakeDamage))]
    public class PreventColition {
        [HarmonyPriority(Priority.First)]
        public static bool Prefix(ProjectileCollision __instance, ref float dmg) {
            if(__instance.GetComponentInParent<ProjectileHit>() is ProjectileHit hit && hit.ownPlayer != null && hit.ownPlayer.HasCard("Unstable_Portal")) {
                dmg = 0f;
                return false;
            }

            return true;
        }
    }
}
