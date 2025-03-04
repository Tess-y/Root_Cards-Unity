using CardChoiceSpawnUniqueCardPatch.CustomCategories;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace RootCore.ArtStuffs {
    public class CardTypeGraber: MonoBehaviour {
	
	    public static Dictionary<CardCategory, string> map;
	
	    void Awake() {
	        if(map == null){
	            map = new Dictionary<CardCategory, string>(){
	                {WillsWackyManagers.Utils.CurseManager.instance.curseCategory,"Curse"},
	                {CustomCardCategories.instance.CardCategory("ProjectileAugment"),"Projectile Augment"},
	                {CustomCardCategories.instance.CardCategory("GivesNulls"),"Distillery"}
	            };
	        }
	    }
	
	    void Start() {
	        var text = GetComponent<TextMeshProUGUI>();
	        if(GetComponentInParent<CardInfo>() is CardInfo card && card.categories.Intersect(map.Keys).Any()){
	            text.text = map[card.categories.First(c => map.Keys.Contains(c))];
	        }else{
	            text.text = "";
	        }
	    }
	}
}
