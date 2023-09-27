using HarmonyLib;
using Photon.Pun;
using System.Collections.Generic;
using UnboundLib;
using UnityEngine;

[HarmonyPatch(typeof(CardChoice), "SpawnUniqueCard")]
internal class CardSpawnPatch {

    private static void Postfix(List<GameObject> ___spawnedCards, Transform[] ___children, int ___pickrID, ref GameObject __result) {

        var player = GetPlayerWithID(___pickrID);
        if(player != null && ___spawnedCards.Count == 0 && player.data.stats.GetRootData().perpetualCard != null) {
            GameObject old = __result;
            RootCards.instance.ExecuteAfterFrames(3, () => PhotonNetwork.Destroy(old));
            __result = PhotonNetwork.Instantiate(player.data.stats.GetRootData().perpetualCard.name, __result.transform.position, __result.transform.rotation);
        }
        if(player != null && ___spawnedCards.Count == ___children.Length -1 && player.data.stats.GetRootData().DelayedCard != null) {
            GameObject old = __result;
            RootCards.instance.ExecuteAfterFrames(3, () => PhotonNetwork.Destroy(old));
            __result = PhotonNetwork.Instantiate(player.data.stats.GetRootData().DelayedCard.name, __result.transform.position, __result.transform.rotation);
            player.data.stats.GetRootData().DelayedCard = null;
        }
    }
    internal static Player GetPlayerWithID(int playerID) {
        for(int i = 0; i < PlayerManager.instance.players.Count; i++) {
            if(PlayerManager.instance.players[i].playerID == playerID) {
                return PlayerManager.instance.players[i];
            }
        }
        return null;
    }
}
