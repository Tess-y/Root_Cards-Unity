using UnityEngine;

internal class Devourer: DealtDamageEffect {
    public Hunger hunger;
    public override void DealtDamage(Vector2 damage, bool selfDamage, Player damagedPlayer = null) {
        RootCards.Debug(hunger);
        hunger.gunStatModifier.projectileColor=Color.white;
        hunger.hungerLevel=1;
    }
}
