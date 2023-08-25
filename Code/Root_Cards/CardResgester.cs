using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CardChoiceSpawnUniqueCardPatch.CustomCategories;
using Photon.Pun;
using UnboundLib;
using UnboundLib.Utils;
using UnityEngine;

public class CardResgester: MonoBehaviour {
    public RootCardInfo[] cards;
    internal static Dictionary<string, RootCardInfo> ModCards;

    public void Regester() {
        ModCards=new Dictionary<string, RootCardInfo>();
        foreach(var card in cards) {
            if(card==null)
                continue;
            card.name=$"Root-Card  {card.Key} ({card.Tag})";
            ModCards.Add(card.Key, card);
            PhotonNetwork.PrefabPool.RegisterPrefab(card.name, card.gameObject);
            if(card.Hidden) {
                ModdingUtils.Utils.Cards.instance.AddHiddenCard(card);
                CustomCardCategories.instance.UpdateAndPullCategoriesFromCard(card);
            } else if(card.Restricted) {
                ModdingUtils.Utils.Cards.instance.AddHiddenCard(card);
                CustomCardCategories.instance.UpdateAndPullCategoriesFromCard(card);
                RootCards.instance.ExecuteAfterFrames(15, ()=>
                    ((ObservableCollection<CardInfo>)typeof(CardManager).GetField("activeCards", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static).GetValue(null)).Add(card));
            } else {
                CardManager.cards.Add(card.name, new Card(card.Tag, Unbound.config.Bind("Cards: "+card.Tag, card.name, true), card));
            }

            if(card.categories.Contains(CustomCardCategories.instance.CardCategory("SkipReroleCard")))
                WillsWackyManagers.Utils.RerollManager.instance.cardsSkippedForRerolls.Add(card);

            card.Setup();
        }
    }

}
