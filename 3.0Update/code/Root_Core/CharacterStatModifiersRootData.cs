using HarmonyLib;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace RootCore {
    [Serializable]
    public class CharacterStatModifiersRootData {
	    public float shieldEfectiveness;
	    public int freeCards;
	    public int ammoCap;
	    public int bulletCap;
	    public int trueMaxAmmo;
	    public CardInfo lockedCard;
	    public float witchTimeDuration;
	    public bool stillShoping;
	    public int knowledge;
	    public float hpCulling;
	    public int nullsPerPoint;
	    public NullData nullData;
	    public bool simple;
	    public CardInfo perpetualCard;
	    public CardInfo DelayedCard;
		public float damageCap;
		public float damageCapWindow;
        public string SteamID;
        public float damageCapFilled;
		public float flatProjectileDamage;
		public float flatHPboost;
        public float tempflatHPboost;

        public CharacterStatModifiersRootData() {
	        reset();
	        knowledge=0;
	        SteamID="";
	        stillShoping=false;
            freeCards = 0;

        }
	
	    public void reset() {
	
	        shieldEfectiveness=1;
	        ammoCap=-1;
	        bulletCap=-1;
	        trueMaxAmmo=3;
	        lockedCard=null;
	        witchTimeDuration=0;
	        hpCulling=0;
	        nullsPerPoint=0;
	        nullData=new NullData();
	        simple=false;
	        perpetualCard=null;
	        DelayedCard = null;
			damageCap=0;
			damageCapWindow=0;
			damageCapFilled = 0;
            flatProjectileDamage = 0;
			flatHPboost = 0;
        }
	
	    public class NullData {
	        public float Health_multiplier;
	        public float MovmentSpeed_multiplier;
	        public float Damage_multiplier;
	        public int gun_Reflects;
	        public int gun_Ammo;
	        public float Lifesteal;
	        public float block_cdMultiplier;
	
	        public int Revives;
	
	        public NullData() {
	            Health_multiplier=1;
	            MovmentSpeed_multiplier=1;
	            Damage_multiplier=1;
	            Lifesteal=0;
	            block_cdMultiplier=1;
	            gun_Reflects=0;
	            gun_Ammo=0;
	            Revives=0;
	        }
	
	    }
	}
	
	public static class CharacterStatModifiersExtension {
	    public static readonly ConditionalWeakTable<CharacterStatModifiers, CharacterStatModifiersRootData> data =
	        new ConditionalWeakTable<CharacterStatModifiers, CharacterStatModifiersRootData>();
	
	
	    public static CharacterStatModifiersRootData GetRootData(this CharacterStatModifiers characterstats) {
	        return data.GetOrCreateValue(characterstats);
	    }
	
	    public static CharacterStatModifiersRootData GetRootData(this Player player) {
			return player.data.stats.GetRootData();
	    }
	
	    public static void AddData(this CharacterStatModifiers characterstats, CharacterStatModifiersRootData value) {
	        try {
	            data.Add(characterstats, value);
	        } catch(Exception) { }
	    }
	
	}
	// reset additional CharacterStatModifiers when ResetStats is called
	[HarmonyPatch(typeof(CharacterStatModifiers), "ResetStats")]
	class CharacterStatModifiersPatchResetStats {
	    private static void Prefix(CharacterStatModifiers __instance) {
	        __instance.GetRootData().reset();
	
	
	        Transform[] allChildren = __instance.gameObject.GetComponentsInChildren<Transform>();
	        foreach(Transform child in allChildren) {
	            if(child.gameObject.name=="ShieldStone") {
	                child.localScale=new Vector3(0.05f, 0.05f, 0.05f);
	            }
	        }
	    }
	}
}
