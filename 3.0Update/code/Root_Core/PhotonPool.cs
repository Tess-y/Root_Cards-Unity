using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

namespace RootCore {
    public class PhotonPool: MonoBehaviour {
	    public List<GameObject> Prefabs;
	    public void Regester() {
	        foreach(var prefab in Prefabs)
	            PhotonNetwork.PrefabPool.RegisterPrefab(prefab.name, prefab);
	    }
	}
}
