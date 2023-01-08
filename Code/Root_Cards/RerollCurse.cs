using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnboundLib.GameModes;
using UnityEngine;
using WillsWackyManagers.Utils;

public class RerollCurse: OnAddEffect {
    public override void OnAdd(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats) {
        GameModeManager.AddOnceHook(GameModeHooks.HookPlayerPickEnd, (_)=>AddHook(player));
    }
    public static IEnumerator AddHook(Player player){
        GameModeManager.AddOnceHook(GameModeHooks.HookPickEnd, (_)=>DoReroll(player), GameModeHooks.Priority.Last);
        yield break;
    }
    public static IEnumerator DoReroll(Player player) {
        if(ModdingUtils.Utils.Cards.instance.RemoveCardFromPlayer(player,CardResgester.ModCards["Infinite_Reroll"],editCardBar: true) == 0) yield break;
        yield return new WaitForSeconds(0.25f);
        yield return RerollManager.instance.Reroll(player,false,true, CardResgester.ModCards["Infinite_Reroll"]);
    }
}

