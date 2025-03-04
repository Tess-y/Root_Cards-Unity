using System.Collections;
using System.Linq;
using UnboundLib.GameModes;
using UnityEngine;

namespace Genie {
    public class Curseed: MonoBehaviour {
	    Player player;
	    void Awake() {
	        player = GetComponentInParent<Player>();
	        GameModeManager.AddHook(GameModeHooks.HookPointStart, Curse);
	    }
	
	    void OnDestroy() {
	        GameModeManager.RemoveHook(GameModeHooks.HookPointStart, Curse);
	    }
	
	    private IEnumerator Curse(IGameModeHandler _){
	        WillsWackyManagers.Utils.CurseManager.instance.CursePlayer(player,(c,p)=>
	            !c.categories.Contains(CardChoiceSpawnUniqueCardPatch.CustomCategories.CustomCardCategories.instance.CardCategory("KindCurse")));
	        yield break;
	    }
	}
}
