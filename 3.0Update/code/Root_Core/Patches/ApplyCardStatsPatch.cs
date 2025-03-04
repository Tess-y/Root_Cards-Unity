using HarmonyLib;
using System.Linq;


namespace RootCore.Patches {
    [HarmonyPatch(typeof(ApplyCardStats), "ApplyStats")]
    internal class ApplyCardStatsPatch {
	    private static void Postfix(ApplyCardStats __instance, Player ___playerToUpgrade) {
	        var aad = __instance.GetComponent<RootStatModifiers>();
	        if(aad!=null) {
	            aad.Apply(___playerToUpgrade);
	        }
	        __instance.GetComponents<OnAddEffect>().ToList().ForEach(effect => { effect.Run(___playerToUpgrade); });
	    }
	}
}
