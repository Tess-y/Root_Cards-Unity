using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnboundLib;
using PlayerTimeScale;

public class ShotBustBullet: MonoBehaviour {
    public static int count;
    int id;
    void Awake() {
        id = count++;
        transform.rotation=getShootRotation(id, 6);
        transform.position+=transform.forward;
        if(count==6)
            count=0; 
    }
    void LateUpdate(){
        if(GetComponent<MoveTransform>().distanceTravelled > 9){
            Photon.Pun.PhotonNetwork.Destroy(gameObject);
        }
    }

    private Quaternion getShootRotation(int bulletID, int numOfProj) {
        Vector3 vector = transform.forward;
        var spread=numOfProj*0.05f;
        float even = bulletID*((spread*2)/(numOfProj-1))-spread;

        vector+=Vector3.Cross(vector, Vector3.forward)*even;
        return Quaternion.LookRotation(vector);
    }
}
