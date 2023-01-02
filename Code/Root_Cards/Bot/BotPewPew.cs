using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using SoundImplementation;
using UnboundLib;

public class BotPewPew: MonoBehaviour {
    Player player;
    public Gun gun;
    public SoundUnityEventPlayer sound;
    public float cooldown;
    public float range;
    float timeout;
    void Awake() {
        player = GetComponentInParent<Player>();
        timeout = 99f;
        gun.gameObject.AddComponent<Holdable>().holder = player.data;
    }

    void Start() {
        timeout = 0f;
    }

    void Update() {
        if(player.data.dead || !(bool)player.data.playerVel.GetFieldValue("simulated")) return;
        if(timeout > 0){
            timeout -= Time.deltaTime;
        }else{
            try{
                Player target = PlayerManager.instance.players.Where(p=> p.teamID != player.teamID && Vector3.Distance(transform.position,p.transform.position)<=range &&
                    PlayerManager.instance.CanSeePlayer(transform.position, player).canSee).OrderBy(p=> Vector3.Distance(transform.position,p.transform.position)).First();
                if(target == null) return;
                gun.SetFieldValue("forceShootDir", (Vector3)(target.transform.position - transform.position));
                gun.shootPosition = transform;
                gun.Attack(gun.currentCharge, true);
                sound.PlayStart();
            }catch{}
        }
    }
}
