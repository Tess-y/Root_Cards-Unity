using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ModdingUtils.Utils;
using Nullmanager;
using UnboundLib;
using UnityEngine;

public class DeNuller : OnAddEffect
{
    public override void OnAdd(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
    {
        RootCards.instance.ExecuteAfterFrames(2, () =>
        {
            List<CardInfo> cards = data.currentCards;
            List<int> nulls = new List<int>();
            List<CardInfo> nulleds = new List<CardInfo>();
            for (int i = 0; i< cards.Count; i++)
            {
                if (cards[i].GetComponent<CardInfo>() is NullCardInfo nullCard)
                {
                    nulls.Add(i);
                    nulleds.Add(nullCard.NulledSorce);
                }
            }
            Unbound.Instance.StartCoroutine(ReplaceCards(player, nulls.ToArray(), nulleds.ToArray(), editCardBar: true));
        });
    }

    public IEnumerator ReplaceCards(Player player, int[] indeces, CardInfo[] newCards, string[] twoLetterCodes = null, bool editCardBar = true)
    {
        if (twoLetterCodes == null)
        {
            twoLetterCodes = new string[indeces.Length];
            for (int i = 0; i < twoLetterCodes.Length; i++)
            {
                twoLetterCodes[i] = "";
            }
        }
        List<bool> reassign = new List<bool>();

        List<CardInfo> list = new List<CardInfo>();
        foreach (CardInfo currentCard in player.data.currentCards)
        {
            list.Add(currentCard);
        }

        List<CardInfo> newCardsToAssign = new List<CardInfo>();
        List<string> twoLetterCodesToAssign = new List<string>();
        int num = 0;
        for (int j = 0; j < list.Count; j++)
        {
            if (!indeces.Contains(j))
            {
                newCardsToAssign.Add(list[j]);
                twoLetterCodesToAssign.Add("");
                reassign.Add(true);
            }
            else if (newCards[num] == null)
            {
                newCardsToAssign.Add(list[j]);
                twoLetterCodesToAssign.Add("");
                num++;
                reassign.Add(true);
            }
            else
            {
                newCardsToAssign.Add(newCards[num]);
                twoLetterCodesToAssign.Add(twoLetterCodes[num]);
                num++;
                reassign.Add(false);
            }
        }

        ModdingUtils.Utils.Cards.instance.RemoveAllCardsFromPlayer(player, editCardBar);
        yield return new WaitForSecondsRealtime(0.1f);
        if (editCardBar)
        {
            CardBarUtils.instance.ClearCardBar(player);
        }

        ModdingUtils.Utils.Cards.instance.AddCardsToPlayer(player, newCardsToAssign.ToArray(), reassign.ToArray(), twoLetterCodesToAssign.ToArray(), null, null, editCardBar);
    }
}
