using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Predestination: MonoBehaviour {
    Player player;
    int OldDraws;
    void Awake() {
        player=GetComponentInParent<Player>();
        OldDraws=DrawNCards.DrawNCards.GetPickerDraws(player.playerID);
    }

    void Update() { 
        if (player != null &&DrawNCards.DrawNCards.GetPickerDraws(player.playerID) != 1)
            DrawNCards.DrawNCards.SetPickerDraws(player.playerID, 1);
    }

    void OnDestroy() {
        DrawNCards.DrawNCards.SetPickerDraws(player.playerID, OldDraws);
    }
}
