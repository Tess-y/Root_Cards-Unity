using RootCore;
using System.Linq;
using UnboundLib;
using UnboundLib.GameModes;
using UnityEngine;

namespace RootAdvancedCards {
    public class GiveGrizzly : MonoBehaviour{

        void Start() {
            Core.instance.ExecuteAfterFrames(2, () => {
                var players = PlayerManager.instance.players.ToList();
                foreach(var player in players) {
                    if(GameModeManager.CurrentHandler.GetTeamScore(player.teamID).rounds == 0) {
                        player.GiveCard("Grizzly");
                    }
                }
            });
        }

    }
}
