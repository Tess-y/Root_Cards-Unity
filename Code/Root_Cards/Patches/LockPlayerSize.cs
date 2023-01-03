﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HarmonyLib;
using UnboundLib;

public class LockPlayerSize: MonoBehaviour {

    internal Vector3 localScale;

    void Start() {
        localScale = GetComponentInParent<Player>().transform.localScale;
        GetComponentInParent<Player>().data.maxHealth=0;
        GetComponentInParent<Player>().data.health=0;
    }
}

[HarmonyPatch(typeof(CharacterStatModifiers), "ConfigureMassAndSize")]
class PlayerSizePatch {
    public static void Postfix(CharacterStatModifiers __instance) {
        if(__instance.GetComponentInChildren<LockPlayerSize>() is LockPlayerSize) {
            __instance.transform.localScale= Vector3.one;
            ((CharacterData)__instance.GetFieldValue("data")).playerVel.SetFieldValue("mass",25f);
        }
    }
}


