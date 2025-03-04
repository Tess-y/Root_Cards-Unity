using BepInEx;
using HarmonyLib;
using Nullmanager;
using RarityLib.Utils;
using RootCore;
using System.Collections;
using System.Linq;
using UnboundLib.GameModes;
using UnityEngine;

namespace RootNulledCards {

    [BepInDependency("Systems.R00t.CoreModual")]
    [BepInDependency("com.Root.Null")]
    [BepInPlugin(ModId, ModName, Version)]
	[BepInProcess("Rounds.exe")]
	public class Main:BaseUnityPlugin {
		private const string ModId = "Systems.R00t.Nulls";
		private const string ModName = "Nulled Cards";
		public const string Version = "1.1.0";

		void Awake() {
			var harmony = new Harmony(ModId);
			harmony.PatchAll();
            Core.RegesterCards(Jotunn.Utils.AssetUtils.LoadAssetBundleFromResources("nullcardassets", typeof(Main).Assembly).LoadAsset<GameObject>("Nulled").GetComponent<CardList>());
        }


		void Start() {

            NullManager.instance.RegesterOnAddCallback(OnNullAdd);

            GameModeManager.AddHook(GameModeHooks.HookPlayerPickEnd, ExtraPicks);
            GameModeManager.AddHook(GameModeHooks.HookPointEnd, PointEnd);
        }

        internal IEnumerator ExtraPicks(IGameModeHandler gm) {
            foreach(Player player in PlayerManager.instance.players.ToArray()) {
                if(player.GetRootData().knowledge > 0) {
                    yield return GameModeManager.TriggerHook(GameModeHooks.HookPlayerPickStart);
                    CardChoiceVisuals.instance.Show(Enumerable.Range(0, PlayerManager.instance.players.Count).Where(i => PlayerManager.instance.players[i].playerID == player.playerID).First(), true);
                    yield return CardChoice.instance.DoPick(1, player.playerID, PickerType.Player);
                    yield return new WaitForSecondsRealtime(0.1f);
                    yield return GameModeManager.TriggerHook(GameModeHooks.HookPlayerPickEnd);
                }
            }
            yield break;
        }
        internal IEnumerator PointEnd(IGameModeHandler gm) {
            PlayerManager.instance.players.ToList().ForEach(player => {
                player.data.stats.AjustNulls(player.GetRootData().nullsPerPoint);
            });
            yield break;
        }
        internal void OnNullAdd(NullCardInfo card, Player player) {
            Gun gun = player.GetComponent<Holding>().holdable.GetComponent<Gun>();
            GunAmmo gunAmmo = gun.GetComponentInChildren<GunAmmo>();
            CharacterData data = player.GetComponent<CharacterData>();
            HealthHandler health = player.GetComponent<HealthHandler>();
            Gravity gravity = player.GetComponent<Gravity>();
            Block block = player.GetComponent<Block>();
            CharacterStatModifiers characterStats = player.GetComponent<CharacterStatModifiers>();

            data.maxHealth *= characterStats.GetRootData().nullData.Health_multiplier;
            characterStats.movementSpeed *= characterStats.GetRootData().nullData.MovmentSpeed_multiplier;
            characterStats.lifeSteal += characterStats.GetRootData().nullData.Lifesteal;
            block.cdMultiplier *= characterStats.GetRootData().nullData.block_cdMultiplier;
            gun.damage *= characterStats.GetRootData().nullData.Damage_multiplier;
            gun.reflects += characterStats.GetRootData().nullData.gun_Reflects;
            gunAmmo.maxAmmo += characterStats.GetRootData().nullData.gun_Ammo;

            if(RarityUtils.GetRarityData(card.rarity).relativeRarity <= RarityUtils.GetRarityData(CardInfo.Rarity.Rare).relativeRarity) {
                characterStats.respawns += characterStats.GetRootData().nullData.Revives;
            }
        }
    }
}

