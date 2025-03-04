using RootCore;
using UnboundLib;
using UnityEngine;

namespace RootCurses {
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
	        if((FullScreenMode)Optionshandler.fullScreen == FullScreenMode.ExclusiveFullScreen) Optionshandler.fullScreen = Optionshandler.FullScreenOption.WindowedFullScreen;
	        running=true;
	        odds=(int)((1/Time.fixedDeltaTime)*15);
	        Screen.SetResolution(60, 60, (FullScreenMode)Optionshandler.fullScreen);
	        Core.instance.ExecuteAfterSeconds(2.5F, () => { Optionshandler.instance.SetResolution(Optionshandler.resolution); running=false; });
	    }
	}
}
