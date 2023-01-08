using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnboundLib;

public class TunnelVision: MonoBehaviour {
    Player player;
    Resolution lastResolution;
    bool running;
    int odds;

    void Awake() {
        player=GetComponentInParent<Player>();
        running=false;
        odds=(int)((1/Time.fixedDeltaTime)*15);
    }

    void FixedUpdate() {
        if(player==null||!player.data.view.IsMine||!ModdingUtils.Utils.PlayerStatus.PlayerAliveAndSimulated(player)||running||CardChoice.instance.IsPicking)
            return;
        if(UnityEngine.Random.Range(0, odds--)==0)
            Confuse();
    }

    void Confuse() {
        lastResolution=Optionshandler.resolution;
        running=true;
        odds=(int)((1/Time.fixedDeltaTime)*15);
        Optionshandler.instance.SetResolution(new Resolution() { refreshRate=lastResolution.refreshRate, height=60, width=60 });
        RootCards.instance.ExecuteAfterSeconds(2.5F, () => { Optionshandler.instance.SetResolution(lastResolution); running=false; });
    }
}
