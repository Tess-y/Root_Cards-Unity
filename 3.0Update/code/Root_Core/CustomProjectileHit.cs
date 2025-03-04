using Photon.Pun;
using UnityEngine;

namespace RootCore {
    public abstract class CustomProjectileHit: ProjectileHit {

        public override void Hit(HitInfo hit, bool forceCall = false) {
            int num = -1;
            if((bool)hit.transform) {
                PhotonView component = hit.transform.root.GetComponent<PhotonView>();
                if((bool)component) {
                    num = component.ViewID;
                }
            }
            int num2 = -1;
            if(num == -1) {
                Collider2D[] componentsInChildren = MapManager.instance.currentMap.Map.GetComponentsInChildren<Collider2D>();
                for(int i = 0; i < componentsInChildren.Length; i++) {
                    if(componentsInChildren[i] == hit.collider) {
                        num2 = i;
                    }
                }
            }
            HealthHandler healthHandler = null;
            if((bool)hit.transform) {
                healthHandler = hit.transform.GetComponent<HealthHandler>();
            }
            bool flag = false;
            if((bool)healthHandler) {
                if(playersHit.Contains(healthHandler)) {
                    return;
                }
                if(view.IsMine && healthHandler.GetComponent<Block>().IsBlocking()) {
                    flag = true;
                }
                HoldPlayer(healthHandler);
            }
            if(view.IsMine || forceCall) {
                if(sendCollisions) {
                    view.RPC("RPCA_DoCusomeHit", RpcTarget.All, hit.point, hit.normal, (Vector2)move.velocity, num, num2, flag);
                } else {
                    RPCA_DoCusomeHit(hit.point, hit.normal, move.velocity, num, num2, flag);
                }
            }
        }

        public abstract void RPCA_DoCusomeHit(Vector2 hitPoint, Vector2 hitNormal, Vector2 vel, int viewID = -1, int colliderID = -1, bool wasBlocked = false);

    }
}
