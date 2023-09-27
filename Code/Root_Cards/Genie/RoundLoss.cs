using System.Collections;
using System.Collections.Generic;
using UnboundLib.GameModes;
using UnityEngine;

public class RoundLoss:MonoBehaviour {
    public int cost = 0;
    // Start is called before the first frame update
    void Start() {
        GameModeManager.CurrentHandler.SetTeamScore(GetComponentInParent<Player>().teamID,
            new TeamScore(0, GameModeManager.CurrentHandler.GetTeamScore(GetComponentInParent<Player>().teamID).rounds - cost));
    }

    void OnDestroy() {
        GameModeManager.CurrentHandler.SetTeamScore(GetComponentInParent<Player>().teamID,
            new TeamScore(0, GameModeManager.CurrentHandler.GetTeamScore(GetComponentInParent<Player>().teamID).rounds + cost));
    }
}
