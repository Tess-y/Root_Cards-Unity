
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using UnboundLib.Utils;
using UnityEngine;

public class PowerDistillation : OnAddEffect
{
    public override void OnAdd(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
    {
        RootCards.instance.StartCoroutine(addRandomCards(player, gun, gunAmmo, data, health, gravity, block, characterStats));
    }
    public IEnumerator addRandomCards(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
    {
        yield return null;
        for (int i = 0; i < 2; i++)
        {
            CardInfo randomCard = ModdingUtils.Utils.Cards.instance.NORARITY_GetRandomCardWithCondition(player, gun, gunAmmo, data, health, gravity, block, characterStats, this.condition);
            if (randomCard == null)
            {
                // if there is no valid card, then try drawing from the list of all cards (inactive + active) but still make sure it is compatible
                CardInfo[] allCards = ((ObservableCollection<CardInfo>)typeof(CardManager).GetField("activeCards", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null)).ToList().Concat((List<CardInfo>)typeof(CardManager).GetField("inactiveCards", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null)).ToArray();
                randomCard = ModdingUtils.Utils.Cards.instance.DrawRandomCardWithCondition(allCards, player, null, null, null, null, null, null, null, this.condition);

            }

            //ModdingUtils.Utils.Cards.instance.AddCardToPlayer(player, randomCard, false, "", 2f);
            int cardCount = player.data.currentCards.Count();
            ModdingUtils.Utils.Cards.instance.AddCardToPlayer(player, randomCard, addToCardBar: true);
            var time = 0f;
            yield return new WaitUntil(() =>
            {
                time += Time.deltaTime;
                return ((player.data.currentCards.Count > cardCount) || (player.data.currentCards[player.data.currentCards.Count - 1] == randomCard) || (time > 5f));
            });
            ModdingUtils.Utils.CardBarUtils.instance.ShowAtEndOfPhase(player, randomCard);
        }
        yield break;
    }
    public bool condition(CardInfo card, Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            // do not allow duplicates of cards with allowMultiple == false (handled by moddingutils)
            // card rarity must be as desired
            // card cannot be another cardmanipulation card
            // card cannot be from a blacklisted catagory of any other card (handled by moddingutils)

            return (card.rarity == CardInfo.Rarity.Rare) && !card.categories.Intersect(RootCards.noLotteryCategories).Any();

        }
}