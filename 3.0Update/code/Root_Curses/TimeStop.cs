using PlayerTimeScale;
using RootCore;
using UnboundLib;
using UnboundLib.Networking;
using UnityEngine;

namespace RootCurses {
    public class TimeStop: MonoBehaviour {
	    Player player;
	    PlayerTimeScale.PlayerTimeScale timeScale;
	    float time;
	    const float min = 3;
	    const float max = 10;
	    void Awake() {
	        player=GetComponentInParent<Player>();
	        timeScale = player.ApplyTimeScale(1);
	        time=UnityEngine.Random.value*(max-min)+min;
	    }
	
	    void OnDestroy() {
	        Destroy(timeScale);
	    }
	
	    void Update() {
	        if(player.data.view.IsMine && ModdingUtils.Utils.PlayerStatus.PlayerAliveAndSimulated(player) && time > 0){
	            time -= TimeHandler.deltaTime;
	            if(time <= 0f){
	               NetworkingManager.RPC(this.GetType(),"DoStopTime", player.playerID, transform.GetSiblingIndex());
	            }
	        }
	    }
	
	    internal void StopTime(){
	        timeScale.Scale = 0;
	        GetComponent<ParticleSystem>().Play();
	        Core.instance.ExecuteAfterSeconds(0.5f,()=> { time=UnityEngine.Random.value*(max-min)+min; timeScale.Scale=1; });
	    }
	    
	
	    [UnboundRPC]
	    public static void DoStopTime(int playerID, int index){
	        Player player = PlayerManager.instance.players.Find(p=> p.playerID == playerID);
	        player.transform.GetChild(index).GetComponent<TimeStop>().StopTime();
	    }
	}
}
