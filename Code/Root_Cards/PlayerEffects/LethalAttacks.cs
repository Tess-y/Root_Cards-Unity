using ModdingUtils.RoundsEffects;
using UnityEngine;

internal class LethalAttacks : HitEffect
{
    public override void DealtDamage(Vector2 damage, bool selfDamage, Player damagedPlayer)
    {
        damagedPlayer.data.health = -100000;
    }
}
