using UnityEngine;
internal class PainfullAttacks: DealtDamageEffect {
    public override void DealtDamage(Vector2 damage, bool selfDamage, Player damagedPlayer) {
        if(selfDamage||damagedPlayer==null)
            return;
        this.gameObject.GetComponentInParent<Player>().data.healthHandler.DoDamage(damage, damagedPlayer.data.playerVel.position, Color.magenta, damagingPlayer: this.gameObject.GetComponentInParent<Player>(), ignoreBlock: true);
    }
}
