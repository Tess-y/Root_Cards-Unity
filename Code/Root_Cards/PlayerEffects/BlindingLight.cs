using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using HarmonyLib;

[HarmonyPatch(typeof(CardChoice))]
public class BlindingLight: MonoBehaviour {
    [HarmonyPostfix]
    [HarmonyPatch(nameof(CardChoice.GetCardColor))]
    public static void GetCardColor(ref Color __result){
        Patch(ref __result);
    }
    [HarmonyPostfix]
    [HarmonyPatch(nameof(CardChoice.GetCardColor2))]
    public static void GetCardColor2(ref Color __result){
        Patch(ref __result);
    }
    public static void Patch(ref Color __result){
        if(PlayerManager.instance.players.Any(p=>p.data.view.IsMine && p.GetComponentInChildren<BlindingLight>() != null)){
            __result = Color.white;
        }
    }
}
