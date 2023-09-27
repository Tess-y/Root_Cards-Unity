using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EndlessAdd : OnAddEffect
{
    public override void OnAdd(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
    {
        RootCards.instance.StartCoroutine(LockCard(player));
    }

    public static IEnumerator LockCard(Player player){
        CardInfo card = player.data.currentCards.Last();
        yield return new WaitUntil(() => player.data.currentCards.Last() != card && player.data.currentCards.Last() != CardResgester.ModCards["Genie_Endless"]);
        player.data.stats.GetRootData().perpetualCard = player.data.currentCards.Last();
        player.data.currentCards.Last().sourceCard = player.data.currentCards.Last();
    }
}
