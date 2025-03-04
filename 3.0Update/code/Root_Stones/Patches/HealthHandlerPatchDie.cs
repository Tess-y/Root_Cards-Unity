using HarmonyLib;
using ModdingUtils.MonoBehaviours;
using RootCore;
using System;
using System.Collections;
using UnboundLib.GameModes;

namespace RootStones.Patches {
    [Serializable]
	[HarmonyPatch(typeof(HealthHandler))]
	internal class HealthHandlerPatchDie {


		[HarmonyPostfix]
		[HarmonyPriority(Priority.Last)]
        [HarmonyPatch(nameof(HealthHandler.RPCA_Die))]
        [HarmonyPatch(nameof(HealthHandler.RPCA_Die_Phoenix))]
        private static void RPCA_Die(HealthHandler __instance, CharacterData ___data, Player ___player) {
			Player damagingPlayer = ___data.lastSourceOfDamage;

            if(__instance != null && damagingPlayer != null && damagingPlayer != ___player
                && damagingPlayer.HasCard("Soul_Stone")) {
				if(damagingPlayer.HasCard("Gauntlet")){
					var effect = damagingPlayer.gameObject.AddComponent<ReversibleEffect>();
					effect.SetLivesToEffect(999999999);
					effect.characterStatModifiersModifier.respawns_add = 1;
					effect.applyImmediately = true;
					IEnumerator reapply(IGameModeHandler gm) { effect.ApplyModifiers(); yield break; }
                    IEnumerator disable(IGameModeHandler gm) { GameModeManager.RemoveHook(GameModeHooks.HookBattleStart, reapply); yield break; }
                    GameModeManager.AddHook(GameModeHooks.HookBattleStart, reapply, GameModeHooks.Priority.Last);
					GameModeManager.AddHook(GameModeHooks.HookRoundEnd, disable);
				}else damagingPlayer.data.stats.remainingRespawns++;
				___data.lastSourceOfDamage = null;
            }
		}

	}
}
	
