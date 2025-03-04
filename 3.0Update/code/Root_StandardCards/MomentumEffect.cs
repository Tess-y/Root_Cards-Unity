using System.Collections;
using UnboundLib.GameModes;
using UnityEngine;

namespace RootStandardCards {
    public class MomentumEffect:MonoBehaviour {
        Player player;
        TeamScore oldScore;
        void Awake() {
            player = GetComponentInParent<Player>();
            oldScore = GameModeManager.CurrentHandler.GetTeamScore(player.teamID);
            GameModeManager.AddHook(GameModeHooks.HookPointEnd, StorePoints);
            GameModeManager.AddHook(GameModeHooks.HookRoundStart, RestorePoints);
        }

        void OnDestroy() {
            GameModeManager.RemoveHook(GameModeHooks.HookPointEnd, StorePoints);
            GameModeManager.RemoveHook(GameModeHooks.HookRoundStart, RestorePoints);
        }

        public IEnumerator StorePoints(IGameModeHandler gameModeHandler) {
            var score = gameModeHandler.GetTeamScore(player.teamID);
            if(score.points != 0 || score.rounds != oldScore.rounds) {
                oldScore = score;
            }
            yield break;
        }

        public IEnumerator RestorePoints(IGameModeHandler gameModeHandler) {
            var score = gameModeHandler.GetTeamScore(player.teamID);
            var newScore = new TeamScore(score.points + oldScore.points, score.rounds);
            gameModeHandler.SetTeamScore(player.teamID, newScore);
            yield break;
        }

    }
}
