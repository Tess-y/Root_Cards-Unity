using System;
using System.Runtime.CompilerServices;
using HarmonyLib;
using UnityEngine;

[Serializable]
public class CharacterStatModifiersRootData
{
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
    public CharacterStatModifiersRootData()
    {
        reset();
    }

    public void reset(){
        
        shieldEfectiveness = 1;
        freeCards = 0;
        ammoCap = -1;
        bulletCap = -1;
        trueMaxAmmo = 3;
        lockedCard = null;
        witchTimeDuration = 0;
        stillShoping = false;
        knowledge = 0;
        hpCulling = 0;
        nullsPerPoint = 0;
        nullData = new NullData();
    }

    public class NullData
    {
        public float Health_multiplier;
        public float MovmentSpeed_multiplier;
        public float Damage_multiplier;
        public int gun_Reflects;
        public int gun_Ammo;
        public float Lifesteal;
        public float block_cdMultiplier;

        public int Revives;

        public NullData()
        {
            Health_multiplier = 1;
            MovmentSpeed_multiplier = 1;
            Damage_multiplier = 1;
            Lifesteal = 0;
            block_cdMultiplier = 1;
            gun_Reflects = 0;
            gun_Ammo = 0;
            Revives = 0;
        }

    }
}

public static class CharacterStatModifiersExtension
{
    public static readonly ConditionalWeakTable<CharacterStatModifiers, CharacterStatModifiersRootData> data =
        new ConditionalWeakTable<CharacterStatModifiers, CharacterStatModifiersRootData>();


    public static CharacterStatModifiersRootData GetRootData(this CharacterStatModifiers characterstats)
    {
        return data.GetOrCreateValue(characterstats);
    }

    public static void AddData(this CharacterStatModifiers characterstats, CharacterStatModifiersRootData value)
    {
        try
        {
            data.Add(characterstats, value);
        }
        catch (Exception) { }
    }

}
// reset additional CharacterStatModifiers when ResetStats is called
[HarmonyPatch(typeof(CharacterStatModifiers), "ResetStats")]
class CharacterStatModifiersPatchResetStats
{
    private static void Prefix(CharacterStatModifiers __instance)
    {
        __instance.GetRootData().reset();

        
        Transform[] allChildren = __instance.gameObject.GetComponentsInChildren<Transform>();
        foreach (Transform child in allChildren)
        {
            if(child.gameObject.name == "ShieldStone")
            {
                child.localScale = new Vector3(0.05f,0.05f,0.05f);
            }
        }
    }
}