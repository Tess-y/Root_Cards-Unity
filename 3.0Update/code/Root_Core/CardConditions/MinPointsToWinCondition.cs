using UnboundLib.GameModes;

namespace RootCore.CardConditions {
    public class MinPointsToWinCondition:CardCondition {
        public int Count;
        public override bool IsPlayerAllowedCard(Player player) {
            return ((int)GameModeManager.CurrentHandler.Settings["pointsToWinRound"]) >= Count;
        }
    }
}
