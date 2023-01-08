using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;
using UnboundLib;

public class StunHit: ProjectileHit {
    public override void Hit(HitInfo hit, bool forceCall = false) {
        List<HealthHandler> playersHit = (List<HealthHandler>)this.GetFieldValue("playersHit");
        MoveTransform move = (MoveTransform)this.GetFieldValue("move");
        int num = -1;
        if((bool)hit.transform) {
            PhotonView component = hit.transform.root.GetComponent<PhotonView>();
            if((bool)component) {
                num=component.ViewID;
            }
        }
        int num2 = -1;
        if(num==-1) {
            Collider2D[] componentsInChildren = MapManager.instance.currentMap.Map.GetComponentsInChildren<Collider2D>();
            for(int i = 0; i<componentsInChildren.Length; i++) {
                if(componentsInChildren[i]==hit.collider) {
                    num2=i;
                }
            }
        }
        HealthHandler healthHandler = null;
        if((bool)hit.transform) {
            healthHandler=hit.transform.GetComponent<HealthHandler>();
        }
        bool flag = false;
        if((bool)healthHandler) {
            if(playersHit.Contains(healthHandler)) {
                return;
            }
            if(view.IsMine&&healthHandler.GetComponent<Block>().IsBlocking()) {
                flag=true;
            }
            StartCoroutine((IEnumerator)this.InvokeMethod("HoldPlayer", healthHandler));
        }
        if(view.IsMine||forceCall) {
            if(sendCollisions) {
                view.RPC("RPCA_DoStunHit", RpcTarget.All, hit.point, hit.normal, (Vector2)move.velocity, num, num2, flag);
            } else {
                RPCA_DoStunHit(hit.point, hit.normal, move.velocity, num, num2, flag);
            }
        }
    }
    [PunRPC]
    public void RPCA_DoStunHit(Vector2 hitPoint, Vector2 hitNormal, Vector2 vel, int viewID = -1, int colliderID = -1, bool wasBlocked = false) {
        MoveTransform move = (MoveTransform)this.GetFieldValue("move");
        HitInfo hitInfo = new HitInfo();
        if((bool)move) {
            move.velocity=vel;
        }
        hitInfo.point=hitPoint;
        hitInfo.normal=hitNormal;
        hitInfo.collider=null;
        if(viewID!=-1) {
            PhotonView photonView = PhotonNetwork.GetPhotonView(viewID);
            hitInfo.collider=photonView.GetComponentInChildren<Collider2D>();
            hitInfo.transform=photonView.transform;
        } else if(colliderID!=-1) {
            hitInfo.collider=MapManager.instance.currentMap.Map.GetComponentsInChildren<Collider2D>()[colliderID];
            hitInfo.transform=hitInfo.collider.transform;
        }
        HealthHandler healthHandler = null;
        if((bool)hitInfo.transform) {
            healthHandler=hitInfo.transform.GetComponent<HealthHandler>();
        }
        if(isAllowedToSpawnObjects) {
            base.transform.position=hitInfo.point;
        }
        if((bool)hitInfo.collider) {
            ProjectileHitSurface component = hitInfo.collider.GetComponent<ProjectileHitSurface>();
            if((bool)component&&component.HitSurface(hitInfo, base.gameObject)==ProjectileHitSurface.HasToStop.HasToStop) {
                return;
            }
        }
        if((bool)healthHandler) {
            Block component2 = healthHandler.GetComponent<Block>();
            if(wasBlocked) {
                component2.DoBlock(base.gameObject, base.transform.forward, hitInfo.point);
                if(destroyOnBlock) {
                    this.InvokeMethod("DestroyMe");
                }
                sinceReflect=0f;
                return;
            }
            CharacterStatModifiers component3 = healthHandler.GetComponent<CharacterStatModifiers>();
            if(movementSlow!=0f&&!wasBlocked) {
                component3.RPCA_AddSlow(movementSlow);
            }
        }
        float num = 1f;
        PlayerVelocity playerVelocity = null;
        if((bool)hitInfo.transform) {
            playerVelocity=hitInfo.transform.GetComponentInParent<PlayerVelocity>();
        }
        if((bool)hitInfo.collider) {
            StunHandler componentInParent = hitInfo.collider.GetComponentInParent<StunHandler>();
            if((bool)componentInParent) {
                if((bool)healthHandler&&percentageDamage!=0f) {
                    damage+=healthHandler.GetComponent<CharacterData>().maxHealth*percentageDamage;
                }

                componentInParent.AddStun((damage*dealDamageMultiplierr*0.01f)+componentInParent.GetComponent<CharacterData>().stunTime);
            }
        }
        bool flag = false;
        if(effects!=null&&effects.Count!=0) {
            for(int j = 0; j<effects.Count; j++) {
                HasToReturn num5 = effects[j].DoHitEffect(hitInfo);
                if(num5==HasToReturn.hasToReturn) {
                    flag=true;
                }
                if(num5==HasToReturn.hasToReturnNow) {
                    return;
                }
            }
        }
        if(!flag) {
            deathEvent.Invoke();
            this.InvokeMethod("DestroyMe");
        }
    }


}
