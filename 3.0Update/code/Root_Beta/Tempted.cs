using ModdingUtils.Utils;
using RootCore;

namespace RootCurses {
    public class Tempted:OnAddEffect {
        public override void OnAdd(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats) {
            CardInfo card = player.data.currentCards.Find(c => c.rarity == RarityBundle.RarityBundle.Unique);
            if (card != null) {
                Cards.instance.ReplaceCard(player, card,CardList.GetCardInfo("Forbidden_Knowledge"));
                CardBarUtils.instance.ShowAtEndOfPhase(player, card);
                CardBarUtils.instance.ShowAtEndOfPhase(player, CardList.GetCardInfo("Forbidden_Knowledge"));
            } else {
                player.GiveCard("Forbidden_Knowledge");
            }
        }
    }
}
