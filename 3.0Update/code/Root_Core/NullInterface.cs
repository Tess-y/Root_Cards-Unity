
using Nullmanager;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RootCore {
    public static class NullInterface 
	{
		public static void SetAntiCard(CardInfo card) {
			card.SetAntiCard();
			card.cardBase = NullManager.instance.AntiCardBase;
		}

		public static void MarkUnNullable(CardInfo card) {
			card.MarkUnNullable();
		}

		public static void NeedsNull(CardInfo card) {
			card.NeedsNull();
		}

		internal static void ApplyNullStuffsToPlayer(Player player, RootStatModifiers modifiers) {
            Gun gun = player.GetComponent<Holding>().holdable.GetComponent<Gun>();
            GunAmmo gunAmmo = gun.GetComponentInChildren<GunAmmo>();
            CharacterData data = player.GetComponent<CharacterData>();
            HealthHandler health = player.GetComponent<HealthHandler>();
            player.GetComponent<Movement>();
            Gravity gravity = player.GetComponent<Gravity>();
            Block block = player.GetComponent<Block>();
            CharacterStatModifiers characterStats = player.GetComponent<CharacterStatModifiers>();

            characterStats.AjustNulls(modifiers.nulls);

            var nullDataToUpgraide = characterStats.GetRootData().nullData;
            {
                int nullcount = player.GetNullCount();
                int legNullcound = 0;
                RarityLib.Utils.RarityUtils.Rarities.Values.ToList().ForEach(r => {
                    if(r.relativeRarity <= RarityLib.Utils.RarityUtils.GetRarityData(RootCardInfo.GetRarity(RootCardInfo.CardRarity.Legendary)).relativeRarity) {
                        legNullcound += player.GetNullCount(r.value);
                    }
                });

                nullDataToUpgraide.Health_multiplier *= modifiers.Null_Health_multiplier;
                data.maxHealth *= Mathf.Pow(modifiers.Null_Health_multiplier, nullcount);

                nullDataToUpgraide.MovmentSpeed_multiplier *= modifiers.Null_MovmentSpeed_multiplier;
                characterStats.movementSpeed *= Mathf.Pow(modifiers.Null_MovmentSpeed_multiplier, nullcount);

                nullDataToUpgraide.Damage_multiplier *= modifiers.Null_Damage_multiplier;
                gun.damage *= Mathf.Pow(modifiers.Null_Damage_multiplier, nullcount);

                nullDataToUpgraide.Lifesteal += modifiers.Null_Lifesteal;
                characterStats.lifeSteal += nullcount * modifiers.Null_Lifesteal;

                nullDataToUpgraide.block_cdMultiplier *= modifiers.Null_block_cdMultiplier;
                block.cdMultiplier *= Mathf.Pow(modifiers.Null_block_cdMultiplier, nullcount);

                nullDataToUpgraide.gun_Reflects += modifiers.Null_gun_Reflects;
                gun.reflects += nullcount * modifiers.Null_gun_Reflects;

                nullDataToUpgraide.gun_Ammo += modifiers.Null_gun_Ammo;
                gunAmmo.maxAmmo += nullcount * modifiers.Null_gun_Ammo;

                nullDataToUpgraide.Revives += modifiers.Null_Revives;
                characterStats.respawns += legNullcound * modifiers.Null_Revives;

                NullManager.instance.SetAdditionalNullStats(player, "Root_Cards", GetStatsForPlayer(player));

                RarityLib.Utils.RarityUtils.Rarities.Values.ToList().ForEach(r => {
                    if(r.relativeRarity <= RarityLib.Utils.RarityUtils.GetRarityData(RootCardInfo.GetRarity(RootCardInfo.CardRarity.Legendary)).relativeRarity) {
                        NullManager.instance.SetRarityNullStats(player, r.value, "Root_Cards", GetLedgStatsForPlayer(player));
                    }
                });
            }
        }

        public static CardInfoStat[] GetStatsForPlayer(Player player) {
            CharacterStatModifiersRootData.NullData nullData = player.GetRootData().nullData;
            List<CardInfoStat> stats = new List<CardInfoStat>();
            if(nullData.Health_multiplier > 1)
                stats.Add(new CardInfoStat() {
                    positive = true,
                    stat = "Health",
                    amount = $"+{(int)((nullData.Health_multiplier - 1) * 100)}%",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                });

            if(nullData.MovmentSpeed_multiplier > 1)
                stats.Add(new CardInfoStat() {
                    positive = true,
                    stat = "Movemet Speed",
                    amount = $"+{(int)((nullData.MovmentSpeed_multiplier - 1) * 100)}%",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                });

            if(nullData.Lifesteal > 0)
                stats.Add(new CardInfoStat() {
                    positive = true,
                    stat = "Lifesteal",
                    amount = $"+{(int)((nullData.Lifesteal) * 100)}%",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                });

            if(nullData.block_cdMultiplier < 1)
                stats.Add(new CardInfoStat() {
                    positive = true,
                    stat = "Block Cooldown",
                    amount = $"-{(int)((1 - nullData.block_cdMultiplier) * 100)}%",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                });

            if(nullData.Damage_multiplier > 1)
                stats.Add(new CardInfoStat() {
                    positive = true,
                    stat = "Damage",
                    amount = $"+{(int)((nullData.Damage_multiplier - 1) * 100)}%",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                });

            if(nullData.gun_Reflects > 0)
                stats.Add(new CardInfoStat() {
                    positive = true,
                    stat = $"Bounce{(nullData.gun_Reflects == 1 ? "" : "s")}",
                    amount = $"+{nullData.gun_Reflects}",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                });

            if(nullData.gun_Ammo > 0)
                stats.Add(new CardInfoStat() {
                    positive = true,
                    stat = "Ammo",
                    amount = $"+{nullData.gun_Ammo}",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                });
            return stats.ToArray();
        }

        public static CardInfoStat[] GetLedgStatsForPlayer(Player player) {
            CharacterStatModifiersRootData.NullData nullData = player.GetRootData().nullData;
            List<CardInfoStat> stats = new List<CardInfoStat>();
            if(nullData.Revives > 0)
                stats.Add(new CardInfoStat() {
                    positive = true,
                    stat = $"{(nullData.Revives == 1 ? "Life" : "Lives")}",
                    amount = $"+{nullData.Revives}",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                });
            return stats.ToArray();
        }
    }
}
