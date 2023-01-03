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
        OldDraws=DrawNCards.DrawNCards.GetPickerDraws(player.teamID);
    }

    void Update() { 
        if (player != null &&DrawNCards.DrawNCards.GetPickerDraws(player.teamID) != 1)
            DrawNCards.DrawNCards.SetPickerDraws(player.teamID, 1);
    }

    void OnDestroy() {
        DrawNCards.DrawNCards.SetPickerDraws(player.teamID, OldDraws);
    }
}
