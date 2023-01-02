using System.Collections.Generic;
using Photon.Pun;
using UnboundLib;
using UnboundLib.Utils;
using UnityEngine;

public class CardResgester: MonoBehaviour {
    public RootCardInfo[] cards;
    internal static Dictionary<string, CardInfo> ModCards;

    public void Regester() {
        ModCards=new Dictionary<string, CardInfo>();
        foreach(var card in cards) {
            if(card==null)
                continue;
            card.name=$"Root-Card  {card.Key} ({card.Tag})";
            ModCards.Add(card.Key, card);
            PhotonNetwork.PrefabPool.RegisterPrefab(card.name, card.gameObject);
            if(card.Hidden) {
                ModdingUtils.Utils.Cards.instance.AddHiddenCard(card);
            } else {
                CardManager.cards.Add(card.name, new Card(card.Tag, Unbound.config.Bind("Cards: "+card.Tag, card.name, true), card));
            }
            card.Setup();
        }
    }

}
