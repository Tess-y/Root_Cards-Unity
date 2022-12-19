﻿using BepInEx;
using UnboundLib;
using UnboundLib.Cards;
using HarmonyLib;
using CardChoiceSpawnUniqueCardPatch.CustomCategories;
using UnityEngine;
using UnboundLib.GameModes;
using System;
using System.Collections;
using BepInEx.Configuration;
using TMPro;
using UnboundLib.Utils.UI;
using ItemShops.Utils;
using UnityEngine.UI;
using WillsWackyManagers.Utils;
using RarityLib.Utils;
using System.Linq;
using Nullmanager;

// These are the mods required for our Mod to work
[BepInDependency("com.willis.rounds.unbound", BepInDependency.DependencyFlags.HardDependency)]
[BepInDependency("pykess.rounds.plugins.moddingutils", BepInDependency.DependencyFlags.HardDependency)]
[BepInDependency("pykess.rounds.plugins.cardchoicespawnuniquecardpatch", BepInDependency.DependencyFlags.HardDependency)]
[BepInDependency("com.willuwontu.rounds.itemshops", BepInDependency.DependencyFlags.HardDependency)]
[BepInDependency("com.willuwontu.rounds.managers", BepInDependency.DependencyFlags.HardDependency)]
[BepInDependency("root.classes.manager.reborn")]
[BepInDependency("root.cardtheme.lib")]
[BepInDependency("root.rarity.lib")]
[BepInDependency("com.Root.Null")]
// Declares our Mod to Bepin
[BepInPlugin(ModId, ModName, Version)]
// The game our Mod Is associated with
[BepInProcess("Rounds.exe")]
public class RootCards : BaseUnityPlugin
{
    public static ConfigEntry<bool> DEBUG;
    public static ConfigEntry<bool> Credits;
    private const string ModId = "com.Root.Cards";
    private const string ModName = "RootCards";
    public const string Version = "1.0.0"; // What version are we On (major.minor.patch)?
    internal static AssetBundle Assets;
    public const string ModInitials = "Root"; 
    public static RootCards instance { get; private set; }
    public static CardCategory PotatoCategory; 
    private static CardCategory[] _noLotteryCategories;
    public static CardCategory[] noLotteryCategories {get{
        if(_noLotteryCategories == null)
            _noLotteryCategories =  new CardCategory[] { CardChoiceSpawnUniqueCardPatch.CustomCategories.CustomCardCategories.instance.CardCategory("CardManipulation"), CardChoiceSpawnUniqueCardPatch.CustomCategories.CustomCardCategories.instance.CardCategory("NoRandom") };
        return _noLotteryCategories;
    }}

    //    <EmbeddedResource Include="Assets\AssetBundles\rootassets" />
    void Awake()
    {
        DEBUG = base.Config.Bind<bool>(ModInitials, "Debug", false, "Enable to turn on concole spam from our mod");
        Credits = base.Config.Bind<bool>(ModInitials, "Credits", false, "Enable to turn on Card Credits");
        var harmony = new Harmony(ModId);
        harmony.PatchAll();
        instance = this;
        RarityUtils.AddRarity("Trinket", 3, new Color(0.38f, 0.38f, 0.38f), new Color(0.0978f, 0.1088f, 0.1321f));
        CardThemeLib.CardThemeLib.instance.CreateOrGetType("DarknessBlack", new CardThemeColor() { bgColor = new Color(0.1978f, 0.1088f, 0.1321f), targetColor = new Color(0.0978f, 0.1088f, 0.1321f) });
        Assets = Jotunn.Utils.AssetUtils.LoadAssetBundleFromResources("rootassets", typeof(RootCards).Assembly);
        Assets.LoadAsset<GameObject>("CardResgester").GetComponent<CardResgester>().Regester();

    }
    void Start()
    {
    
        Unbound.RegisterMenu("Root Settings", delegate () { }, new Action<GameObject>(this.NewGUI), null, true);

        NullManager.instance.RegesterOnAddCallback(OnNullAdd);

        GameModeManager.AddHook(GameModeHooks.HookPlayerPickEnd, (gm) => ExtraPicks());
        GameModeManager.AddHook(GameModeHooks.HookGameStart, (gm) => SetUpContract());
        GameModeManager.AddHook(GameModeHooks.HookGameStart, (gm) => Genie.Wish());
        GameModeManager.AddHook(GameModeHooks.HookPickEnd, (gm) => Genie.WaitTillShopDone());
        GameModeManager.AddHook(GameModeHooks.HookGameStart, (gm) => Genie.RestCardLock());
        GameModeManager.AddHook(GameModeHooks.HookPointEnd, PointEnd);
    }
    

    public static void Debug(object message)
    {
        if (DEBUG.Value)
        {
            UnityEngine.Debug.Log("ROOT=>" + message);
        }
    }

    private void NewGUI(GameObject menu)
    {
        TextMeshProUGUI textMeshProUGUI;
        MenuHandler.CreateText("Root Settings", menu, out textMeshProUGUI, 60, false, null, null, null, null);
        MenuHandler.CreateToggle(RootCards.Credits.Value, "Card Credits", menu, delegate (bool value)
        {
            RootCards.Credits.Value = value;
        }, 50, false, Color.magenta, null, null, null);
        MenuHandler.CreateToggle(RootCards.DEBUG.Value, "Debug Mode", menu, delegate (bool value)
        {
            RootCards.DEBUG.Value = value;
        }, 50, false, Color.red, null, null, null);
    }

    internal static IEnumerator SetUpContract(){
        RarityLib.Utils.RarityUtils.SetCardRarityModifier(CardResgester.ModCards["Contract"],20);
        RarityLib.Utils.RarityUtils.SetCardRarityModifier(CardResgester.ModCards["Dark_Queen"],200);
        yield break;
    }

    internal static IEnumerator ExtraPicks()
    {
        foreach (Player player in PlayerManager.instance.players.ToArray())
        {
            if (player.data.stats.GetRootData().knowledge > 0)
            {
                yield return GameModeManager.TriggerHook(GameModeHooks.HookPlayerPickStart);
                CardChoiceVisuals.instance.Show(Enumerable.Range(0, PlayerManager.instance.players.Count).Where(i => PlayerManager.instance.players[i].playerID == player.playerID).First(), true);
                yield return CardChoice.instance.DoPick(1, player.playerID, PickerType.Player);
                yield return new WaitForSecondsRealtime(0.1f);
                yield return GameModeManager.TriggerHook(GameModeHooks.HookPlayerPickEnd);
            }
        }
        yield break;
    }

    internal static IEnumerator PointEnd(IGameModeHandler gm){
        PlayerManager.instance.players.ToList().ForEach(player => {
            player.data.stats.AjustNulls(player.data.stats.GetRootData().nullsPerPoint);
        });
        yield break;
    }
    internal static void OnNullAdd(NullCardInfo card, Player player){
        Gun gun = player.GetComponent<Holding>().holdable.GetComponent<Gun>();
        GunAmmo gunAmmo = gun.GetComponentInChildren<GunAmmo>();
        CharacterData data = player.GetComponent<CharacterData>();
        HealthHandler health = player.GetComponent<HealthHandler>();
        Gravity gravity = player.GetComponent<Gravity>();
        Block block = player.GetComponent<Block>();
        CharacterStatModifiers characterStats = player.GetComponent<CharacterStatModifiers>();

        data.maxHealth *= characterStats.GetRootData().nullData.Health_multiplier;
        characterStats.movementSpeed *= characterStats.GetRootData().nullData.MovmentSpeed_multiplier;
        characterStats.lifeSteal += characterStats.GetRootData().nullData.Lifesteal;
        block.cdMultiplier *= characterStats.GetRootData().nullData.block_cdMultiplier;
        gun.damage *= characterStats.GetRootData().nullData.Damage_multiplier;
        gun.reflects += characterStats.GetRootData().nullData.gun_Reflects;
        gunAmmo.maxAmmo += characterStats.GetRootData().nullData.gun_Ammo;
    }
}