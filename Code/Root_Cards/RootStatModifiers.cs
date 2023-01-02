using UnityEngine;
using UnboundLib;
using System.Collections.Generic;
using Photon.Pun;
using ItemShops.Extensions;
using System.Linq;
using Nullmanager;

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

        characterStats.AjustNulls(nulls);
        characterStats.GetRootData().witchTimeDuration+=WhichTime;

        characterStats.GetRootData().shieldEfectiveness*=shieldEfectiveness;

        if(ammoCap>0)
            characterStats.GetRootData().ammoCap=ammoCap;
        if(bulletCap>0)
            characterStats.GetRootData().bulletCap=bulletCap;

        if(knowledge)
            RootCards.instance.ExecuteAfterFrames(1, () => { characterStats.GetRootData().knowledge++; });

        characterStats.GetRootData().hpCulling=1-((1-characterStats.GetRootData().hpCulling)*(1-hpCulling));

        player.GetAdditionalData().bankAccount.Deposit("Wish", wishes);

        characterStats.GetRootData().nullsPerPoint+=nullsPerPoint;

        var nullDataToUpgraide = characterStats.GetRootData().nullData;
        {
            int nullcount = player.GetNullCount();
            int rareNullcound = 0;
            RarityLib.Utils.RarityUtils.Rarities.Values.ToList().ForEach(r => {
                if(r.relativeRarity<=RarityLib.Utils.RarityUtils.GetRarityData(CardInfo.Rarity.Rare).relativeRarity) {
                    rareNullcound+=player.GetNullCount(r.value);
                }
            });

            nullDataToUpgraide.Health_multiplier*=Null_Health_multiplier;
            data.maxHealth*=Mathf.Pow(Null_Health_multiplier, nullcount);

            nullDataToUpgraide.MovmentSpeed_multiplier*=Null_MovmentSpeed_multiplier;
            characterStats.movementSpeed*=Mathf.Pow(Null_MovmentSpeed_multiplier, nullcount);

            nullDataToUpgraide.Damage_multiplier*=Null_Damage_multiplier;
            gun.damage*=Mathf.Pow(Null_Damage_multiplier, nullcount);

            nullDataToUpgraide.Lifesteal+=Null_Lifesteal;
            characterStats.lifeSteal+=nullcount*Null_Lifesteal;

            nullDataToUpgraide.block_cdMultiplier*=Null_block_cdMultiplier;
            block.cdMultiplier*=Mathf.Pow(Null_block_cdMultiplier, nullcount);

            nullDataToUpgraide.gun_Reflects+=Null_gun_Reflects;
            gun.reflects+=nullcount*Null_gun_Reflects;

            nullDataToUpgraide.gun_Ammo+=Null_gun_Ammo;
            gunAmmo.maxAmmo+=nullcount*Null_gun_Ammo;

            nullDataToUpgraide.Revives+=Null_Revives;
            characterStats.respawns+=rareNullcound*Null_Revives;

            NullManager.instance.SetAdditionalNullStats(player, "Root_Cards", GetStatsForPlayer(player));

            RarityLib.Utils.RarityUtils.Rarities.Values.ToList().ForEach(r => {
                if(r.relativeRarity<=RarityLib.Utils.RarityUtils.GetRarityData(CardInfo.Rarity.Rare).relativeRarity) {
                    NullManager.instance.SetRarityNullStats(player, r.value, "Root_Cards", GetRareStatsForPlayer(player));
                }
            });
        }


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


        if(player.data.stats.GetRootData().ammoCap!=-1) {
            GunAmmo ammo = gun.GetComponentInChildren<GunAmmo>();
            ammo.maxAmmo=Mathf.Clamp(ammo.maxAmmo, 1, player.data.stats.GetRootData().ammoCap);
            usedAmmo=false;
        }
        if(player.data.stats.GetRootData().bulletCap!=-1) {
            gun.numberOfProjectiles=Mathf.Clamp(gun.numberOfProjectiles, 1, player.data.stats.GetRootData().bulletCap);
        }
        if(PhotonNetwork.IsMasterClient&&usedAmmo) {
            var ammo = new System.Random().Next(18)+7;
            NetworkingManager.RPC(typeof(RootStatModifiers),nameof(RPC_UsedAmmo),ammo,player.playerID);
        }
    }

    public static CardInfoStat[] GetStatsForPlayer(Player player) {
        CharacterStatModifiersRootData.NullData nullData = player.data.stats.GetRootData().nullData;
        List<CardInfoStat> stats = new List<CardInfoStat>();
        if(nullData.Health_multiplier>1)
            stats.Add(new CardInfoStat() {
                positive=true,
                stat="Health",
                amount=$"+{(int)((nullData.Health_multiplier-1)*100)}%",
                simepleAmount=CardInfoStat.SimpleAmount.notAssigned
            });

        if(nullData.MovmentSpeed_multiplier>1)
            stats.Add(new CardInfoStat() {
                positive=true,
                stat="Movemet Speed",
                amount=$"+{(int)((nullData.MovmentSpeed_multiplier-1)*100)}%",
                simepleAmount=CardInfoStat.SimpleAmount.notAssigned
            });

        if(nullData.Lifesteal>0)
            stats.Add(new CardInfoStat() {
                positive=true,
                stat="Lifesteal",
                amount=$"+{(int)((nullData.Lifesteal)*100)}%",
                simepleAmount=CardInfoStat.SimpleAmount.notAssigned
            });

        if(nullData.block_cdMultiplier<1)
            stats.Add(new CardInfoStat() {
                positive=true,
                stat="Block Cooldown",
                amount=$"-{(int)((1-nullData.block_cdMultiplier)*100)}%",
                simepleAmount=CardInfoStat.SimpleAmount.notAssigned
            });

        if(nullData.Damage_multiplier>1)
            stats.Add(new CardInfoStat() {
                positive=true,
                stat="Damage",
                amount=$"+{(int)((nullData.Damage_multiplier-1)*100)}%",
                simepleAmount=CardInfoStat.SimpleAmount.notAssigned
            });

        if(nullData.gun_Reflects>0)
            stats.Add(new CardInfoStat() {
                positive=true,
                stat=$"Bounce{(nullData.gun_Reflects==1 ? "" : "s")}",
                amount=$"+{nullData.gun_Reflects}",
                simepleAmount=CardInfoStat.SimpleAmount.notAssigned
            });

        if(nullData.gun_Ammo>0)
            stats.Add(new CardInfoStat() {
                positive=true,
                stat="Ammo",
                amount=$"+{nullData.gun_Ammo}",
                simepleAmount=CardInfoStat.SimpleAmount.notAssigned
            });
        return stats.ToArray();
    }

    public static CardInfoStat[] GetRareStatsForPlayer(Player player) {
        CharacterStatModifiersRootData.NullData nullData = player.data.stats.GetRootData().nullData;
        List<CardInfoStat> stats = new List<CardInfoStat>();
        if(nullData.Revives>0)
            stats.Add(new CardInfoStat() {
                positive=true,
                stat=$"{(nullData.Revives==1 ? "Life" : "Lives")}",
                amount=$"+{nullData.Revives}",
                simepleAmount=CardInfoStat.SimpleAmount.notAssigned
            });
        return stats.ToArray();
    }

    [UnboundLib.Networking.UnboundRPC]
    public static void RPC_UsedAmmo(int ammo, int playerID) {
        Player player = PlayerManager.instance.players.Find(p=>p.playerID==playerID);
        player.GetComponent<Holding>().holdable.GetComponentInChildren<GunAmmo>().maxAmmo+=ammo;
    }

}