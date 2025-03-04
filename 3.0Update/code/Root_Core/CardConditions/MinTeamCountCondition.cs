using System.Linq;

namespace RootCore.CardConditions {
    public class MinTeamCountCondition:CardCondition {
        public int Count;
        public override bool IsPlayerAllowedCard(Player player) {
            return PlayerManager.instance.players.Select(p=>p.teamID).Distinct().Count() >= Count;
        }
    }
}
