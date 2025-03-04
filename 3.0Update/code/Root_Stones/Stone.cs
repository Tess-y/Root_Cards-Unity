using RootCore;
using System.Linq;

namespace RootStones {
    public class Stone:OnAddEffect {
        public override void OnAdd(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats) {
            CardInfo.Rarity rarity = RarityBundle.RarityBundle.Divine;
            switch(Gauntlet.stones.Select(stone => player.HasCard(stone)?1:0).Sum()) {
                case 1:
                    rarity = RarityBundle.RarityBundle.Mythical;
                    break;
                case 2:
                    rarity = RarityBundle.RarityBundle.Legendary;
                    break;
                case 3:
                    rarity = RarityBundle.RarityBundle.Epic;
                    break;
                case 4:
                    rarity = RarityBundle.RarityBundle.Rare;
                    break;
                case 5:
                    rarity = RarityBundle.RarityBundle.Exotic;
                    break;
            }
            Gauntlet.stones.ToList().ForEach(stone => stone.rarity = rarity);
        }
    }
}
