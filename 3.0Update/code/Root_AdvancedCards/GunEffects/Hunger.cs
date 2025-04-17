using ModdingUtils.MonoBehaviours;
using RootCore;
using UnityEngine;

namespace RootAdvancedCards.GunEffects {
    internal class Hunger: ReversibleEffect {
	    internal int hungerLevel = 1;
	    internal int hungerGrowth = 0;
	    internal int hungerMax = 30;
	    public void AttackAction() { hungerLevel+=hungerGrowth; }
	
	    public override void OnAwake() {
	        Hunger hunger = transform.parent.GetComponentInChildren<Hunger>();
	        hunger.hungerGrowth++;
	        if(hunger!=this) {
	            DestroyImmediate(gameObject);
	        }
	    }
	
	    public override void OnStart() {
	        gunStatModifier.projectileColor=Color.white;
	        SetLivesToEffect(int.MaxValue);
	        gun.AddAttackAction(AttackAction);
	    }
	    public override void OnOnDestroy() {
	        gun.RemoveAttackAction(AttackAction);
	    }
	
	    public override void OnUpdate() {
	        if(hungerLevel!=gunStatModifier.damage_mult && ModdingUtils.Utils.PlayerStatus.PlayerAliveAndSimulated(player)) {
	            hungerLevel=Mathf.Clamp(hungerLevel, 1, hungerMax);
	            float dilute = 1-Mathf.Clamp(hungerLevel/(float)hungerMax, 0f, 1f);
	            gunStatModifier.projectileColor= Colour.New(1, dilute, dilute);
	            ClearModifiers();
	            gunStatModifier.damage_mult=hungerLevel;
	            ApplyModifiers();
	        }
	    }
	
	    public override void OnOnDisable() {
	        hungerLevel=1;
	    }
	}
}
