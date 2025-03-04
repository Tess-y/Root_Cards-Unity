using System.Linq;

namespace RootCore.CardConditions {
    public class HasTeammatesCondition:CardCondition {
        public override bool IsPlayerAllowedCard(Player player) {
            return PlayerManager.instance.players.Any(p => p.teamID == player.teamID && p.playerID != player.playerID);
        }
    }
}
