using Photon.Pun;
using RootCore;
using UnityEngine;

namespace RootAdvancedCards.GunEffects {
    public class StunHit: CustomProjectileHit {
	    [PunRPC]
	    public override void RPCA_DoCusomeHit(Vector2 hitPoint, Vector2 hitNormal, Vector2 vel, int viewID = -1, int colliderID = -1, bool wasBlocked = false) {
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
						DestroyMe();
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
                DestroyMe();
            }
	    }

    }
}
