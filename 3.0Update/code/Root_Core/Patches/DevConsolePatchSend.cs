using HarmonyLib;
using Photon.Pun;
using System.Linq;
using UnboundLib;
using UnboundLib.Networking;
using UnityEngine;

namespace RootCore.Patches {

    [HarmonyPatch(typeof(DevConsole), nameof(DevConsole.Send))]
    internal class DevConsolePatchSend
    {
        private static void Prefix(string message)
        {
            Core.Debug(message);
            Player controller = PlayerManager.instance.GetPlayerWithActorID(PhotonNetwork.LocalPlayer.ActorNumber);
            Core.Debug(controller);
            if (controller != null && controller.GetRootData().freeCards > 0 && !GameManager.instance.battleOngoing)
            {
                NetworkingManager.RPC(typeof(DevConsolePatchSend), nameof(AddCardToPlayer),message, PhotonNetwork.LocalPlayer.ActorNumber);
            }
        }

        [UnboundRPC]
        public static void AddCardToPlayer(string message, int playerActorNumber) {

            Core.Debug(message+ " " + playerActorNumber);
            Player controller = PlayerManager.instance.GetPlayerWithActorID(playerActorNumber);
            Core.Debug(controller);

            if(controller != null && controller.GetRootData().freeCards > 0 && !GameManager.instance.battleOngoing) {
                CardInfo[] cards = CardChoice.instance.cards.ToArray();
                int num = -1;
                float num2 = 0f;
                for(int i = 0; i < cards.Length; i++) {
                    string text = cards[i].GetComponent<CardInfo>().cardName.ToUpper();
                    text = text.Replace(" ", "");
                    string text2 = message.ToUpper();
                    text2 = text2.Replace(" ", "");
                    float num3 = 0f;
                    for(int j = 0; j < text2.Length; j++) {
                        if(text.Length > j && text2[j] == text[j]) {
                            num3 += 1f / (float)text2.Length;
                        }
                    }

                    num3 -= (float)Mathf.Abs(text2.Length - text.Length) * 0.001f;
                    if(num3 > 0.1f && num3 > num2) {
                        num2 = num3;
                        num = i;
                    }
                }

                if(num != -1) {
                    controller.GiveCard(cards[num]);
                    controller.GetRootData().freeCards -= 1;
                }
            }
        }
    }
}
