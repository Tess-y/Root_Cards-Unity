using RootCore.CardConditions;

namespace RootAdvancedCards.CardConditions {
    public class MitosisCondion:CardCondition {
        public override bool IsPlayerAllowedCard(Player player) {
            return PlayerManager.instance.players.Count < 15;
        }
    }
}
