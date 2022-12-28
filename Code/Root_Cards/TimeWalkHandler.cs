using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using RWF.GameModes;
using UnboundLib.GameModes;
using UnityEngine;
/*
public class TimeWalkHandler {
    internal static Dictionary<Player, int> Picks;
    internal static Dictionary<Player, List<CardInfo>> CardsSeen;
    internal static bool TimeWalking = false;

    internal static IEnumerator TimeWalk(Player player) {
        ModdingUtils.Utils.Cards.instance.RemoveAllCardsFromPlayer(player);
        for(int _ = 0; _<5; _++)
            yield return null;
        int picks = Picks[player];
        TimeWalking=true;
        for(int i = 0; i<picks; i++) {
            yield return GameModeManager.TriggerHook(GameModeHooks.HookPlayerPickStart);
            CardChoiceVisuals.instance.Show(Enumerable.Range(0, PlayerManager.instance.players.Count).Where(i => PlayerManager.instance.players[i].playerID==player.playerID).First(), true);
            yield return CardChoice.instance.DoPick(1, player.playerID, PickerType.Player);
            yield return new WaitForSecondsRealtime(0.1f);
            yield return GameModeManager.TriggerHook(GameModeHooks.HookPlayerPickEnd);
        }
        TimeWalking=false;
        yield break;
    }

}

[HarmonyPatch(typeof(RWFGameMode), "RoundTransition", typeof(int[]))]
public class RWFGameModePatch {
    public static void Prefix(int[] winningTeamIDs) {
        int[] winningTeams = GameModeManager.CurrentHandler.GetGameWinners();
        if(winningTeams.Any()) {
            return;
        }
        PlayerManager.instance.players.ToList().ForEach(player => {
            if(!winningTeamIDs.Contains(player.playerID)) {
                TimeWalkHandler.Picks[player]+=RootCards.DrawsPerTurn;
            }
        });
    }
}

[HarmonyPatch]
public class cardchoicespawnuniquecardpatchPatch {
    static MethodBase TargetMethod() {
        return typeof(CardChoiceSpawnUniqueCardPatch.CardChoiceSpawnUniqueCardPatch).Assembly.GetTypes().ToList().Find(t => t.Name=="CardChoicePatchSpawnUniqueCard").GetMethod("Prefix", BindingFlags.NonPublic|BindingFlags.Static);
    }
    public static bool Prefix(bool __result, ref GameObject __0, ref CardChoice __1, Vector3 __2, Quaternion __3) {
        UnityEngine.Debug.Log(__1);
        if(TimeWalkHandler.TimeWalking) {
            Player player = PlayerManager.instance.players[__1.pickrID];
            if(!TimeWalkHandler.CardsSeen[player].Any())
                return true;
            CardInfo card = TimeWalkHandler.CardsSeen[player].First();
            TimeWalkHandler.CardsSeen[player].RemoveAt(0);
            if(!ModdingUtils.Utils.Cards.instance.PlayerIsAllowedCard(player, card)) {
                return true;
            }

            GameObject gameObject = (GameObject)typeof(CardChoice).InvokeMember("Spawn",
                    BindingFlags.Instance|BindingFlags.InvokeMethod|
                    BindingFlags.NonPublic, null, __0, new object[] { card.gameObject, __2, __3 });
            gameObject.GetComponent<CardInfo>().sourceCard=card.GetComponent<CardInfo>();
            gameObject.GetComponentInChildren<DamagableEvent>().GetComponent<Collider2D>().enabled=false;

            __0=gameObject;
            __result=false;
            return false;
        }
        return true;
    }

    [HarmonyPriority(int.MaxValue)]
    public static void Postfix(ref GameObject __0, CardChoice __1, Vector3 __2, Quaternion __3) {

        Player player;
        if((PickerType)Traverse.Create(__1).Field("pickerType").GetValue()==PickerType.Team) {
            player=PlayerManager.instance.GetPlayersInTeam(__1.pickrID)[0];
        } else {
            player=PlayerManager.instance.players[__1.pickrID];
        }
        TimeWalkHandler.CardsSeen[player].Add(__0.GetComponent<CardInfo>());
    }
}*/