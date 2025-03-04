using RootCore;
using UnityEngine;

namespace RootAdvancedCards.GunEffects {
    internal class Devourer: DealtDamageEffect {
	    public Hunger hunger;
	    public override void DealtDamage(Vector2 damage, bool selfDamage, Player damagedPlayer = null) {
	        Core.Debug(hunger);
	        hunger.gunStatModifier.projectileColor=Color.white;
	        hunger.hungerLevel=1;
	    }
	}
}
