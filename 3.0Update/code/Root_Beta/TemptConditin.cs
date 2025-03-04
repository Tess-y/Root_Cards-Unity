using RootCore;
using RootCore.CardConditions;

namespace RootCurses.CardConditions {
    public class TemptConditin:CardCondition {
        public override bool IsPlayerAllowedCard(Player player) {
            return !player.HasCard("Forbidden_Knowledge");
        }
    }
}
