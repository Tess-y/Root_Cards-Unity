using System;
using ModdingUtils.MonoBehaviours;
using UnboundLib;
using UnityEngine;

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
        gun.InvokeMethod("RemoveAttackAction", (Action)AttackAction);
    }

    public override void OnUpdate() {
        if(hungerLevel!=gunStatModifier.damage_mult) {
            hungerLevel=Mathf.Clamp(hungerLevel, 1, hungerMax);
            float dilute = 1-Mathf.Clamp(hungerLevel/(float)hungerMax, 0f, 1f);
            gunStatModifier.projectileColor=new Color(1f, dilute, dilute);
            ClearModifiers();
            gunStatModifier.damage_mult=hungerLevel;
            ApplyModifiers();
        }
    }

    public override void OnOnDisable() {
        hungerLevel=1;
    }
}
