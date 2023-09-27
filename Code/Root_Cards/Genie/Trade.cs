using CardChoiceSpawnUniqueCardPatch.CustomCategories;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Trade : OnAddEffect
{
    public override void OnAdd(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
    {
        RootCards.instance.StartCoroutine(TradeCard(player));
    }

    public static IEnumerator TradeCard(Player player){
        CardInfo card = player.data.currentCards.Last();
        yield return new WaitUntil(() => player.data.currentCards.Last() != card && player.data.currentCards.Last() != CardResgester.ModCards["Genie_Traded"]);
        for(int _ = 0; _ < 60; _++) yield return null;
        var cards = player.data.currentCards.Where(c => c.cardName != "Genie" && c.cardName != player.data.currentCards.Last().cardName && !c.categories.Contains(CustomCardCategories.instance.CardCategory("GenieOutcome")));
        var avalable = cards.Where(cardi => RarityLib.Utils.RarityUtils.GetRarityData(cardi.rarity).relativeRarity == cards.Min(c=>RarityLib.Utils.RarityUtils.GetRarityData(c.rarity).relativeRarity)).ToArray();
        var trade = avalable[Random.Range((int)0, (int)avalable.Count())];
        ModdingUtils.Utils.Cards.instance.RemoveCardFromPlayer(player, trade);
    }
}
