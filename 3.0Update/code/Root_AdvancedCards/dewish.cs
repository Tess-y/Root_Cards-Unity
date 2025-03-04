using RootCore;
using UnboundLib.Cards;
using UnityEngine;

namespace RootAdvancedCards {
    public class dewish:CustomCard {
	
	    public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats) {
	        
	    }
	
	    public override void OnReassignCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats) {
			characterStats.GetRootData().freeCards--;

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
}
