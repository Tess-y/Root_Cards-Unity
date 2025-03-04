using HarmonyLib;
using RWF.GameModes;
using System;
using System.Collections.Generic;
using System.Linq;
using UnboundLib;
using UnboundLib.GameModes;

namespace RootStandardCards.Patches {
    [Serializable]
    [HarmonyPatch(typeof(RWFGameMode), nameof(RWFGameMode.RPCA_NextRound))]
    public class RWFGameModePachRPCA_NextRound {

        static void Prefix(ref int[] winningTeamIDs) {
            foreach(Player player in PlayerManager.instance.players) {
                if(player != null && player.GetComponentInChildren<ImpendingDoomEffect>() != null) {
                    winningTeamIDs = winningTeamIDs.AddItem(player.teamID).ToArray();
                }
            }
        }
        static void Postfix(int[] winningTeamIDs) {
            List<int> previousRoundWinners = ((int[])GameModeManager.CurrentHandler.GameMode.GetFieldValue("previousRoundWinners")).ToList();
            GameModeManager.CurrentHandler.GameMode.SetFieldValue("previousRoundWinners", previousRoundWinners.Distinct().ToArray());
            List<int> previousPointWinners = ((int[])GameModeManager.CurrentHandler.GameMode.GetFieldValue("previousPointWinners")).ToList();
            if(previousPointWinners.Count != winningTeamIDs.Length) return;
            foreach(Player player in PlayerManager.instance.players) {
                if(player != null && player.GetComponentInChildren<ImpendingDoomEffect>() != null) {

                    previousPointWinners.Remove(player.teamID);
                }
            }
            GameModeManager.CurrentHandler.GameMode.SetFieldValue("previousPointWinners", previousPointWinners.ToArray());
        }
    }

    [Serializable]
    [HarmonyPatch(typeof(RWFGameMode), nameof(RWFGameMode.RoundOver), typeof(int[]))]
    public class RWFGameModePatchRoundOver {
        static void Prefix(RWFGameMode __instance, ref int[] winningTeamIDs) {
            winningTeamIDs = winningTeamIDs.Distinct().ToArray();
            foreach(Player player in PlayerManager.instance.players)
                if(player != null && winningTeamIDs.Contains(player.teamID) && player.GetComponentInChildren<ImpendingDoomEffect>() != null)
                    __instance.teamRounds[player.teamID] = __instance.teamRounds[player.teamID] - 1;
        }
    }
}
