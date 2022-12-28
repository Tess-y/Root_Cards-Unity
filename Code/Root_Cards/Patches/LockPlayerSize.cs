using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HarmonyLib;

public class LockPlayerSize: MonoBehaviour {

    Vector3 localScale;

    void Start() {
        localScale = GetComponentInParent<Player>().transform.localScale;
    }
    [HarmonyPatch(typeof(CharacterData), "ConfigureMassAndSize")]
    private class Patch {
        public static void Postfix(CharacterData __instance){
            if(__instance.GetComponentInChildren<LockPlayerSize>() is LockPlayerSize size){
                __instance.transform.localScale = size.localScale;
            }
        }
    }
}

