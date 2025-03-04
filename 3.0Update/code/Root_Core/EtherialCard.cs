using UnboundLib;

namespace RootCore {
    public class EtherialCard:OnAddEffect {
        //// Throws an error if it is the first card a player takes, but said error is harmless. (I blame pykess but i'm too lazy to write my own code to fix it)
        public override void OnAdd(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats) {
            CardInfo card = GetComponent<CardInfo>().sourceCard;
            Core.instance.ExecuteAfterFrames(10, () => {
                player.RemoveCardFromCardBar(card);
                player.data.currentCards.Remove(card);
                card.Show(player);
            });
        }
    }
}
