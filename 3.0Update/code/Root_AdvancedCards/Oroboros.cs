using RootCore;

namespace RootAdvancedCards {
    public class Oroboros : OnAddEffect {

        internal static int oroboros = 1;

        public override void OnAdd(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats) {
            characterStats.GetRootData().flatProjectileDamage += 10 * oroboros;
            characterStats.GetRootData().flatHPboost += 10 * oroboros;
        }
    }
}
