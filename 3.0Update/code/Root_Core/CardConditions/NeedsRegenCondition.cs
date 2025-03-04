namespace RootCore.CardConditions {
    public class NeedsRegenCondition:CardCondition {
        public int minRegen;
        public override bool IsPlayerAllowedCard(Player player) {
            return player.data.healthHandler.regeneration >= minRegen;
        }
    }
}
