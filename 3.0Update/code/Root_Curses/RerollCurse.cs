using RootCore;
using System.Collections;
using UnboundLib.GameModes;
using UnityEngine;
using WillsWackyManagers.Utils;

namespace RootCurses {
    public class RerollCurse: OnAddEffect {
	    public override void OnAdd(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats) {
	        GameModeManager.AddOnceHook(GameModeHooks.HookPlayerPickEnd, (_)=>AddHook(player));
	    }
	    public static IEnumerator AddHook(Player player){
	        GameModeManager.AddOnceHook(GameModeHooks.HookPickEnd, (_)=>DoReroll(player.playerID), GameModeHooks.Priority.Last);
	        yield break;
	    }
	    public static IEnumerator DoReroll(int playerID) {
			var player = PlayerManager.instance.players.Find(p=>p.playerID == playerID);
            if(player == null || !player.data.currentCards.Contains(CardList.GetCardInfo("Infinite_Reroll")))
				yield break;
            if(ModdingUtils.Utils.Cards.instance.RemoveCardFromPlayer(player,CardList.GetCardInfo("Infinite_Reroll"),editCardBar: true) == 0) yield break;
	        yield return new WaitForSeconds(0.25f);
	        yield return RerollManager.instance.Reroll(player,false,true, CardList.GetCardInfo("Infinite_Reroll"));
	        yield return new WaitForSeconds(0.25f);
	        if (!player.HasCard("Infinite_Reroll"))
                player.GiveCard("Infinite_Reroll");
	    }
	}
	
}
