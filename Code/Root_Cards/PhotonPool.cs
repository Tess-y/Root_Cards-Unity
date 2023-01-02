using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;

public class PhotonPool: MonoBehaviour {
    public List<GameObject> Prefabs;
    public void Regester() {
        foreach(var prefab in Prefabs)
            PhotonNetwork.PrefabPool.RegisterPrefab(prefab.name, prefab);
    }
}
