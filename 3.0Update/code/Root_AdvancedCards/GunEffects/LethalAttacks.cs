using Photon.Pun;
using UnityEngine;

namespace RootAdvancedCards.GunEffects {
    internal class LethalAttacks: RayHitEffect {
        public override HasToReturn DoHitEffect(HitInfo hit) {
            if(!hit.transform) {
                return HasToReturn.canContinue;
            }
            if(hit.transform.GetComponent<CharacterData>() is CharacterData data) {
                if(data.health < 0f && !data.dead) {
                    if(data.stats.remainingRespawns > 0) {
                        data.view.RPC("RPCA_Die_Phoenix", RpcTarget.All, Vector2.down);
                    } else {
                        data.view.RPC("RPCA_Die", RpcTarget.All, Vector2.down);
                    }
                }
            }
            return HasToReturn.canContinue;
        }
    }
}
