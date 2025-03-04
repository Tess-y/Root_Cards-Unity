using ModdingUtils.Utils;
using RootCore;
using System.Collections.Generic;
using System.Linq;
using UnboundLib;
using UnityEngine;

namespace RootStandardCards {
    public class GiveAnyCard : MonoBehaviour {

        void Start () {
            Core.instance.ExecuteAfterFrames(2, () => {
                Player player = GetComponentInParent<Player>();
                List<CardInfo> allCards = Cards.instance.ACTIVEANDHIDDENCARDS.ToList();
                List<CardInfo> validCards = allCards.Where(card => player.IsAllowedCard(card) || WillsWackyManagers.Utils.CurseManager.instance.PlayerIsAllowedCurse(player,card)).ToList();
                validCards.Shuffle();
                player.GiveCard(validCards[0]);
            });
        }

    }
}
