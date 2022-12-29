using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HarmonyLib;

public class LockPlayerSize: MonoBehaviour {

    internal Vector3 localScale;

    void Start() {
        localScale = GetComponentInParent<Player>().transform.localScale;
    }
}

[HarmonyPatch(typeof(CharacterStatModifiers), "ConfigureMassAndSize")]
class PlayerSizePatch {
    public static void Postfix(CharacterStatModifiers __instance) {
        if(__instance.GetComponentInChildren<LockPlayerSize>() is LockPlayerSize size) {
            __instance.transform.localScale=size.localScale;
        }
    }
}


