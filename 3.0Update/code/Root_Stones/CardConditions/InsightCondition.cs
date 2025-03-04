using CardChoiceSpawnUniqueCardPatch.CustomCategories;
using ModdingUtils.Utils;
using RootCore;
using RootCore.CardConditions;
using System.Linq;

namespace RootStones.CardConditions {
    public class InsightCondition:CardCondition {
        public override bool IsPlayerAllowedCard(Player player) {
            return player.data.currentCards.Where(c => c != CardList.GetCardInfo("Mind_Stone") && Cards.instance.PlayerIsAllowedCard(player, c) &&
                    !c.categories.Intersect(new CardCategory[] {
                         CustomCardCategories.instance.CardCategory("GearUp_Card-Shuffle"),
                         CustomCardCategories.instance.CardCategory("NoPreGamePick") //Shuffle has this, as far as i know it is the only card that does, and is the easyst way to blacklist shuffle.
                        }).Any() &&
                    !c.IsHiddenCard()).Count() > 3;
        }
    }
}