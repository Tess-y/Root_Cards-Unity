using HarmonyLib;
using Nullmanager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnboundLib;
using UnboundLib.Networking;
using UnityEngine;

[Serializable]
[HarmonyPatch(typeof(CardChoice), "IDoEndPick")]
internal class CardChoicePatchIDoEndPick
{
    public static void Prefix(CardChoice __instance, ref List<GameObject> ___spawnedCards, ref GameObject pickedCard, ref float ___speed)
    {
        if (pickedCard == null) return;
        if(pickedCard.GetComponent<DistillAcquisition>() != null){
            for (int i = 0; i < ___spawnedCards.Count; i++)
            {
                if ((bool)___spawnedCards[i])
                {
                    if (___spawnedCards[i].gameObject != pickedCard)
                    {
                        __instance.StartCoroutine(GrabCard(___spawnedCards[i], ___speed, __instance));
                    }
                }
            }
            if (PlayerManager.instance.players.Find(p => p.playerID == __instance.pickrID).data.view.IsMine)
                NetworkingManager.RPC(typeof(CardChoicePatchIDoEndPick), nameof(GiveNulls), __instance.pickrID, ___spawnedCards.Count - 1);
            ___spawnedCards.Clear();
            ___spawnedCards.Add(pickedCard);
        }
    }

    private static IEnumerator GrabCard(GameObject card, float speed, CardChoice __instance)
    {

        if (PlayerManager.instance.players.Find(p => p.playerID == __instance.pickrID).data.view.IsMine)
            card.GetComponentInChildren<ApplyCardStats>().Pick(__instance.pickrID, forcePick: false, (PickerType)__instance.GetFieldValue("pickerType"));
        Vector3 startPos = card.transform.position;
        Vector3 endPos = CardChoiceVisuals.instance.transform.position;
        float c2 = 0f;
        while (c2 < 1f)
        {
            CardChoiceVisuals.instance.framesToSnap = 1;
            Vector3 position = Vector3.LerpUnclamped(startPos, endPos, __instance.curve.Evaluate(c2));
            card.transform.position = position;
            c2 += Time.deltaTime * speed;
            yield return null;
        }

        GamefeelManager.GameFeel((startPos - endPos).normalized * 2f);
        card.GetComponentInChildren<CardVisuals>().Leave();
        card.GetComponentInChildren<CardVisuals>().Pick();
    }

    [UnboundRPC]
    public static void GiveNulls(int playerID,int amount)
    {
        PlayerManager.instance.players.Find(p => p.playerID == playerID).data.stats.AjustNulls((int)(amount*2.5f));
    }
}