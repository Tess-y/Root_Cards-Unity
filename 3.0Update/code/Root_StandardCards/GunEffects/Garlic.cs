using UnityEngine;

namespace RootStandardCards.GunEffects {
    public class Garlic:DealtDamageEffect {
	    public override void DealtDamage(Vector2 damage, bool selfDamage, Player damagedPlayer = null) {
	        if(damagedPlayer != null) {
	            float multiplyer = (float)damage.magnitude;
	            multiplyer *= damagedPlayer.data.stats.lifeSteal;
	            multiplyer += damagedPlayer.data.stats.regen / damagedPlayer.data.maxHealth;
	            damagedPlayer.data.healthHandler.TakeDamage(damage.normalized * multiplyer, damagedPlayer.transform.position,null,null,true,true);
	        }
	    }
	}
}
