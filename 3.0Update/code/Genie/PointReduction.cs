using System.Collections;
using UnboundLib.GameModes;
using UnityEngine;

namespace Genie {
    public class PointReduction: MonoBehaviour {
	    Player player;
	
	    void Awake() {
	        player = GetComponentInParent<Player>();
	        GameModeManager.AddHook(GameModeHooks.HookRoundStart,ReducePoints);
	    }
	
	    void OnDestroy() {
	        GameModeManager.RemoveHook(GameModeHooks.HookRoundStart,ReducePoints);
	    }
	
	    public IEnumerator ReducePoints(IGameModeHandler gameModeHandler){
	        var score = gameModeHandler.GetTeamScore(player.teamID);
	        var newScore = new TeamScore( score.points- (int)(GetComponent<AttackLevel>().LevelScale() - 1), score.rounds);
	        gameModeHandler.SetTeamScore(player.teamID,newScore);
	        yield break;
	    }
	}
}
