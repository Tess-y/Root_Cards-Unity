using Photon.Pun;
using System.Collections;
using UnboundLib.GameModes;
using UnityEngine;

namespace Genie {
    internal class RoundDeath: MonoBehaviour {
	    public void Start() {
	        GameModeManager.AddHook(GameModeHooks.HookRoundStart, Kill);
	    }
	
	    public void OnDestroy() {
	        GameModeManager.RemoveHook(GameModeHooks.HookRoundStart, Kill);
	    }
	
	    private IEnumerator Kill(IGameModeHandler gm) {
	
	        GameModeManager.AddOnceHook(GameModeHooks.HookBattleStart, Die);
	        yield break;
	    }
	
	    private IEnumerator Die(IGameModeHandler gm) {
	        GetComponentInParent<Player>().data.view.RPC("RPCA_Die", RpcTarget.All, new Vector2(0, 1));
	        yield break;
	    }
	}
}
