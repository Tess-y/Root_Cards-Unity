using RootCore;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace Genie {
    public class EternityAdd : OnAddEffect
	{
	    public override void OnAdd(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
	    {
	        Core.instance.StartCoroutine(LockCard(player));
	    }
	
	    public static IEnumerator LockCard(Player player){
	        CardInfo card = player.data.currentCards.Last();
	        yield return new WaitUntil(() => player.data.currentCards.Last() != card && player.data.currentCards.Last() != CardList.GetCardInfo("Genie_Eternity"));
	        player.GetRootData().lockedCard = player.data.currentCards.Last();
	        player.data.currentCards.Last().sourceCard = player.data.currentCards.Last();
	    }
	}
}
