using HarmonyLib;
using RootCore;
using System.Linq;
using UnboundLib;
using UnityEngine;

namespace RootCurses.Patches {
    [HarmonyPatch(typeof(Map), "Start")]
    public class MapBlindEffect {

        static void Postfix(Map __instance) {
            __instance.mapIsReadyAction += () => {
                Core.instance.ExecuteAfterSeconds(1.5f, () => {
                if(PlayerManager.instance.players.Any(p => p.data.view.IsMine && p.HasCard("Map_Blind"))) {
                        var renderers = __instance.GetComponentsInChildren<SpriteRenderer>();
                        foreach (var item in renderers)
                        {
                            Object.Destroy(item);
                        }
                    }
                });
            };
        }

    }
}
