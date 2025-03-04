using System.Collections;
using UnboundLib.GameModes;
using UnityEngine;

namespace RootDemonicCards {
    public class HealthCurse: MonoBehaviour {
	    float _Health = 0;
	    private Player _player;
	    public void Awake() {
	        if(_player==null)
	            _player=gameObject.GetComponent<Player>();
	        GameModeManager.AddHook(GameModeHooks.HookPointEnd, gm => reset());
	    }
	    public void Cull(float HP) {
	        if(_player==null)
	            _player=gameObject.GetComponent<Player>();
	
	        if(!ModdingUtils.Utils.PlayerStatus.PlayerAliveAndSimulated(_player))
	            return;
	        
	        _player.data.maxHealth+=_Health;
	        _Health=Mathf.Min(_Health+HP, _player.data.maxHealth-1);
	        _player.data.maxHealth-=_Health;
	    }
	    public IEnumerator reset() {
	        _player.data.maxHealth+=_Health;
	        _Health=0;
	        yield break;
	    }
	}
}
