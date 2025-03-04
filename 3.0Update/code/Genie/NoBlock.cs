using RootCore;
using System.Collections;
using UnityEngine;

namespace Genie {
    public class NoBlock:MonoBehaviour {
	    void Start() {
	        GetComponentInParent<Player>().data.block.enabled = false;
	        GetComponentInParent<Player>().data.block.counter = 0.25f;
	        GetComponentInParent<Player>().transform.Find("Limbs/ArmStuff/ShieldStone").gameObject.SetActive(false);
	    }
	
	    void OnDestroy() {
	        GetComponentInParent<Player>().data.block.enabled = true;
	        GetComponentInParent<Player>().transform.Find("Limbs/ArmStuff/ShieldStone").gameObject.SetActive(true);
	    }
	
	    public static IEnumerator HideChocieBlock() {
	        while(!CardChoice.instance.IsPicking) yield return null;
	        CardChoiceVisuals.instance.transform.Find("Card Choice Face/ArmStuff/ShieldStone").gameObject.SetActive(!PlayerManager.instance.players.Find(player => CardChoice.instance.pickrID == player.playerID).HasCard("Genie_Exposed"));
	        yield break;
	    }
	}
	
}
