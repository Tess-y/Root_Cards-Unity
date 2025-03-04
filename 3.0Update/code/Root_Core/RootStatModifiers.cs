using ItemShops.Extensions;
using Photon.Pun;
using UnboundLib;
using UnityEngine;

namespace RootCore {
    public class RootStatModifiers: MonoBehaviour {
	    public int nulls = 0;
	    public int WhichTime = 0;
	    public bool knowledge = false;
	    public float ShieldStoneSize = 1;
	    public float shieldEfectiveness = 1;
	    public float projectileSizeMult = 1;
	    public int ammoCap = -1;
	    public int bulletCap = -1;
	    public int wishes = 0;
	    public float hpCulling;
	    public int nullsPerPoint;
	    public bool usedAmmo = false;
	    public bool invertion = false;
	    public bool simple = false;
		public float damageCap = 0;
		public float damageCapWindow = 0;
		public GameObject AddObjectToOpponents = null;
		public GameObject AddObjectToTeam = null;
		public GameObject AddObjectToFreinds = null;

	
	
	        public float Null_Health_multiplier = 1;
	        public float Null_MovmentSpeed_multiplier = 1;
	        public float Null_Damage_multiplier = 1;
	        public float Null_block_cdMultiplier = 1;
	        public int Null_gun_Reflects = 0;
	        public int Null_gun_Ammo = 0;
	        public float Null_Lifesteal = 0;
	        public int Null_Revives = 0;
	
	    public void Apply(Player player) {
	        Gun gun = player.GetComponent<Holding>().holdable.GetComponent<Gun>();
	        GunAmmo gunAmmo = gun.GetComponentInChildren<GunAmmo>();
	        CharacterData data = player.GetComponent<CharacterData>();
	        HealthHandler health = player.GetComponent<HealthHandler>();
	        player.GetComponent<Movement>();
	        Gravity gravity = player.GetComponent<Gravity>();
	        Block block = player.GetComponent<Block>();
	        CharacterStatModifiers characterStats = player.GetComponent<CharacterStatModifiers>();
	
	        gun.projectileSize+=1;
	        gun.projectileSize*=projectileSizeMult;
	        gun.projectileSize-=1;
	
	        characterStats.GetRootData().witchTimeDuration+=WhichTime;
	
	        characterStats.GetRootData().shieldEfectiveness*=shieldEfectiveness;
			if(damageCap != 0) {
				if(characterStats.GetRootData().damageCap != 0)
					characterStats.GetRootData().damageCap = Mathf.Min(characterStats.GetRootData().damageCap, damageCap);
				else characterStats.GetRootData().damageCap = damageCap;
			}
            if(characterStats.GetRootData().damageCapWindow != 0 && damageCapWindow != 0)
                characterStats.GetRootData().damageCapWindow = Mathf.Min(characterStats.GetRootData().damageCapWindow, damageCapWindow);
            else characterStats.GetRootData().damageCapWindow = damageCapWindow;

            if(ammoCap>0)
	            characterStats.GetRootData().ammoCap=ammoCap;
	        if(bulletCap>0)
	            characterStats.GetRootData().bulletCap=bulletCap;
	
	        if(knowledge)
	            Core.instance.ExecuteAfterFrames(1, () => { characterStats.GetRootData().knowledge++; });
	        if(simple)
	            player.GetRootData().simple = true;
	
	        characterStats.GetRootData().hpCulling=1-((1-characterStats.GetRootData().hpCulling)*(1-hpCulling));
			if(this.GetComponent<RootCardInfo>().Key.ContainsOR("Genie", "Efreet"))
				player.GetAdditionalData().bankAccount.Deposit("Wish", wishes);
			else
				characterStats.GetRootData().freeCards += wishes;
	
	        characterStats.GetRootData().nullsPerPoint+=nullsPerPoint;

			NullInterface.ApplyNullStuffsToPlayer(player, this);
	
	
	        Transform[] allChildren = player.gameObject.GetComponentsInChildren<Transform>();
	        foreach(Transform child in allChildren) {
	            if(child.gameObject.name=="ShieldStone") {
	                child.localScale*=ShieldStoneSize;
	            }
	        }
	
	
	        if(invertion) {
	            float damage = gun.damage*55*gun.bulletDamageMultiplier;
	            gun.damage=data.maxHealth/55f;
	            player.data.maxHealth=damage;
	        }
	
	
	        if(player.GetRootData().ammoCap!=-1) {
	            GunAmmo ammo = gun.GetComponentInChildren<GunAmmo>();
	            ammo.maxAmmo=Mathf.Clamp(ammo.maxAmmo, 1, player.GetRootData().ammoCap);
	            usedAmmo=false;
	        }
	        if(player.GetRootData().bulletCap!=-1) {
	            gun.numberOfProjectiles=Mathf.Clamp(gun.numberOfProjectiles, 1, player.GetRootData().bulletCap);
	        }
	        if(PhotonNetwork.IsMasterClient&&usedAmmo) {
	            var ammo = new System.Random().Next(18)+7;
	            NetworkingManager.RPC(typeof(RootStatModifiers),nameof(RPC_UsedAmmo),ammo,player.playerID);
	        }


			if(AddObjectToOpponents) {
                foreach (var p in PlayerManager.instance.players)
                {
                    if(p.teamID != player.teamID) {
						characterStats.objectsAddedToPlayer.Add(Instantiate(AddObjectToOpponents, p.transform.position, p.transform.rotation, p.transform));
					}
                }
            }
			if(AddObjectToTeam) {
                foreach (var p in PlayerManager.instance.players)
                {
                    if(p.teamID == player.teamID) {
						characterStats.objectsAddedToPlayer.Add(Instantiate(AddObjectToTeam, p.transform.position, p.transform.rotation, p.transform));
					}
                }
            }
			if(AddObjectToFreinds) {
                foreach (var p in PlayerManager.instance.players)
                {
                    if(p.teamID == player.teamID && p.playerID != player.playerID) {
						characterStats.objectsAddedToPlayer.Add(Instantiate(AddObjectToFreinds, p.transform.position, p.transform.rotation, p.transform));
					}
                }
            }
	    }
	
	    [UnboundLib.Networking.UnboundRPC]
	    public static void RPC_UsedAmmo(int ammo, int playerID) {
	        Player player = PlayerManager.instance.players.Find(p=>p.playerID==playerID);
	        player.GetComponent<Holding>().holdable.GetComponentInChildren<GunAmmo>().maxAmmo+=ammo;
	    }
	
	}
}
