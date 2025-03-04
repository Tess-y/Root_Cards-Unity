using UnityEngine;

namespace RootCore {
    public abstract class OnAddEffect:MonoBehaviour {
		public void Run(Player player) {
			Gun gun = player.GetComponent<Holding>().holdable.GetComponent<Gun>();
			GunAmmo gunAmmo = gun.GetComponentInChildren<GunAmmo>();
			CharacterData data = player.GetComponent<CharacterData>();
			HealthHandler health = player.GetComponent<HealthHandler>();
			Gravity gravity = player.GetComponent<Gravity>();
			Block block = player.GetComponent<Block>();
			CharacterStatModifiers characterStats = player.GetComponent<CharacterStatModifiers>();
			OnAdd(player, gun, gunAmmo, data, health, gravity, block, characterStats);
		}

		public abstract void OnAdd(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats);
	}

}
