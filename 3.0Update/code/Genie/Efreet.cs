using RootCore;

namespace Genie {
    public class Efreet:OnAddEffect {
        public override void OnAdd(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats) {
            GenieCard.CursePlayer(player);
        }
    }
}
