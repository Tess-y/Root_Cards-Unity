using HarmonyLib;
using RootCore;
using System;
using System.Collections;
using System.Linq;
using UnboundLib;
using UnityEngine;

namespace RootAdvancedCards.Patches {
    [Serializable]
	[HarmonyPatch(typeof(CardChoice), nameof(CardChoice.IDoEndPick))]
	internal class SharedPick {
        public static int Picker = -1;
        public static bool eaten = false;
        public static bool eating = false;
		public static bool Prefix(CardChoice __instance, GameObject pickedCard, int theInt, int pickId, ref IEnumerator __result) {
            UnityEngine.Debug.Log($"{pickedCard} {theInt} {pickId}");
            if(Picker == pickId) eaten = true;
            if(!PlayerManager.instance.GetPlayerWithID(pickId).HasCard("Leftovers")) Picker = pickId;
            UnityEngine.Debug.Log($"{Picker}");
            UnityEngine.Debug.Log($"{eaten} {PlayerManager.instance.GetPlayerWithID(pickId).HasCard("Leftovers")} {!PlayerManager.instance.players.Any(player => player.HasCard("Leftovers"))}");
            if(eaten || PlayerManager.instance.GetPlayerWithID(pickId).HasCard("Leftovers") ||
                !PlayerManager.instance.players.Any(player => player.HasCard("Leftovers"))) {
                __instance.pickrID = Picker;
                Picker = -1;
                eaten = false; 
                eating = false;
                UIHandler.instance.StopShowPicker();
                return true;
            }
            eating = true;

            __result = SinglePick(__instance,pickedCard, theInt,pickId);
			return false;
        }
        [HarmonyPatch(typeof(ApplyCardStats),nameof(ApplyCardStats.Pick))]
        [HarmonyPrefix]
        public static void Reset(int pickerID) {
            if(eating && Picker != -1) {
                CardChoice.instance.pickrID = Picker;
                UnityEngine.Debug.Log($"Eaten!!");
                eaten = true;
            }
        }

		public static IEnumerator SinglePick(CardChoice __instance, GameObject pickedCard, int theInt = 0, int pickId = -1) {
            Vector3 startPos = pickedCard.transform.position;
            Vector3 endPos = CardChoiceVisuals.instance.transform.position;
            float c2 = 0f;
            while(c2 < 1f) {
                CardChoiceVisuals.instance.framesToSnap = 1;
                Vector3 position = Vector3.LerpUnclamped(startPos, endPos, __instance.curve.Evaluate(c2));
                pickedCard.transform.position = position;
                __instance.transform.GetChild(theInt).position = position;
                c2 += Time.deltaTime * __instance.speed;
                yield return null;
            }
            GamefeelManager.GameFeel((startPos - endPos).normalized * 2f);
            pickedCard.GetComponentInChildren<CardVisuals>().Leave();
            Player newPlayer = PlayerManager.instance.players.First(player => player.HasCard("Leftovers"));
            __instance.pickrID = newPlayer.playerID;
            
            for(int i = 0; i < __instance.spawnedCards.Count; i++) {
                if(__instance.spawnedCards[i] == pickedCard) {
                    __instance.transform.GetChild(theInt).position = startPos;
                    if(newPlayer.data.view.IsMine)
                        __instance.spawnedCards[i] = __instance.Spawn(CardChoiceSpawnUniqueCardPatch.CardChoiceSpawnUniqueCardPatch.NullCard.gameObject,
                        __instance.children[i].position, __instance.children[i].rotation);
                }
            }

            for(int i = 0; i < __instance.spawnedCards.Count; i++) {
                if(!newPlayer.IsAllowedCard(__instance.spawnedCards[i].GetComponentInChildren<CardInfo>())) {
                    UnityEngine.Object.Destroy(__instance.spawnedCards[i]);
                    if(newPlayer.data.view.IsMine)
                        __instance.spawnedCards[i] = __instance.Spawn(CardChoiceSpawnUniqueCardPatch.CardChoiceSpawnUniqueCardPatch.NullCard.gameObject,
                            __instance.children[i].position, __instance.children[i].rotation);
                }
            }
            
            for(int i = 0; i < __instance.spawnedCards.Count; i++) {
                __instance.spawnedCards[i].GetOrAddComponent<PublicInt>().theInt = i;
            }
            UIHandler.instance.ShowPicker(__instance.pickrID, __instance.pickerType);
           yield break;
        }
	}
}
