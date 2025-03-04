using ModdingUtils.RoundsEffects;
using UnityEngine;

namespace RootAdvancedCards.GunEffects {
    internal class LethalAttacks: HitEffect {
	    public override void DealtDamage(Vector2 damage, bool selfDamage, Player damagedPlayer) {
	        damagedPlayer.data.health=-100000;
			damagedPlayer.data.healthHandler.DoDamage(damage,damagedPlayer.transform.position,Color.red,null,null,true,true,true);

        }
	}
}
