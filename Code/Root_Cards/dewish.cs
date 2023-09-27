using System.Collections;
using System.Collections.Generic;
using UnboundLib.Cards;
using UnityEngine;
using ItemShops.Extensions;

public class dewish:CustomCard {

    public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats) {
        
    }

    public override void OnReassignCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats) {
        player.GetAdditionalData().bankAccount.Deposit("Wish", -1);
    }

    protected override GameObject GetCardArt() {
        throw new System.NotImplementedException();
    }

    protected override string GetDescription() {
        throw new System.NotImplementedException();
    }

    protected override CardInfo.Rarity GetRarity() {
        throw new System.NotImplementedException();
    }

    protected override CardInfoStat[] GetStats() {
        throw new System.NotImplementedException();
    }

    protected override CardThemeColor.CardThemeColorType GetTheme() {
        throw new System.NotImplementedException();
    }

    protected override string GetTitle() {
        throw new System.NotImplementedException();
    }
}
