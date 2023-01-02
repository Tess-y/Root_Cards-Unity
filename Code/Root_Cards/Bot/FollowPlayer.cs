using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnboundLib;
using UnboundLib.Cards;
using UnityEngine;

public class FollowPlayer: MonoBehaviour {

    public float followSpeed;
    public float Distance;
    Player ownPlayer;
    void Awake() {
        ownPlayer = GetComponentInParent<Player>();
    }

    void Start() {
        this.ExecuteAfterFrames(2,()=>transform.SetParent(null));
    }

    void Update() {
        if(transform.parent != null) return;

        transform.GetChild(0).gameObject.SetActive(ownPlayer.gameObject.activeSelf);

        Vector3 TargetA = ownPlayer.transform.position + (Vector3.right*Distance) + (Vector3.up*0.2f);
        Vector3 TargetB = ownPlayer.transform.position + (Vector3.left*Distance) + (Vector3.up*0.2f);
        if(Vector3.Distance(transform.position,TargetA)>Vector3.Distance(transform.position,TargetB)){
           transform.position = Vector3.MoveTowards(transform.position,TargetA,followSpeed*Time.deltaTime);
        }else{
           transform.position = Vector3.MoveTowards(transform.position,TargetB,followSpeed*Time.deltaTime);
        }
    }
}


