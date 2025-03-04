using HarmonyLib;
using UnityEngine;

namespace RootAdvancedCards.Patches {
    public class LockPlayerSize: MonoBehaviour {
	
	
	    void FixedUpdate() {
	        GetComponentInParent<Player>().data.health=0;
	    }
	}
	
	[HarmonyPatch(typeof(CharacterStatModifiers), "ConfigureMassAndSize")]
	class PlayerSizePatch {
	    public static void Postfix(CharacterStatModifiers __instance) {
	        if(__instance.GetComponentInChildren<LockPlayerSize>() is LockPlayerSize) {
	            __instance.transform.localScale= Vector3.one;
				__instance.data.playerVel.mass = 25f;
	        }
	    }
	}
	
	
}
