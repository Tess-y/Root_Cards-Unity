using System.Linq;

namespace RootCore.CardConditions {
    public class UniqueToTeamCondition:CardCondition {
        public override bool IsPlayerAllowedCard(Player player) {
            return !PlayerManager.instance.players.Any(p => p.teamID == player.teamID && p.HasCard(cardInfo));
        }
    }
}
