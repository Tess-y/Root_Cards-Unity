using System.Collections;
using System.Linq;
using UnboundLib.GameModes;
using UnityEngine;

namespace RootStandardCards {
    public class ImpendingDoomEffect:MonoBehaviour {
        Player player;

        void Awake() {
            player = GetComponentInParent<Player>();
            GameModeManager.AddHook(GameModeHooks.HookRoundStart, ReducePoints);
        }

        void OnDestroy() {
            GameModeManager.RemoveHook(GameModeHooks.HookRoundStart, ReducePoints);
        }

        public IEnumerator ReducePoints(IGameModeHandler gameModeHandler) {
            var score = gameModeHandler.GetTeamScore(player.teamID);
            var newScore = new TeamScore(score.points - PlayerManager.instance.players.Select(p=>p.teamID).Distinct().Count(), score.rounds);
            gameModeHandler.SetTeamScore(player.teamID, newScore);
            yield break;
        }

    }
}
