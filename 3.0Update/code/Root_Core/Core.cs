using BepInEx;
using BepInEx.Configuration;
using CardChoiceSpawnUniqueCardPatch.CustomCategories;
using HarmonyLib;
using Photon.Pun;
using RootCore.CardConditions;
using Steamworks;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using TMPro;
using UnboundLib;
using UnboundLib.GameModes;
using UnboundLib.Utils;
using UnboundLib.Utils.UI;
using UnityEngine;


[assembly: AssemblyVersionAttribute(RootCore.Core.Version)]
namespace RootCore {

    [BepInDependency("com.willis.rounds.unbound")]
    [BepInDependency("pykess.rounds.plugins.moddingutils")]
    [BepInDependency("pykess.rounds.plugins.cardchoicespawnuniquecardpatch")]
    [BepInDependency("root.cardtheme.lib")]
    [BepInDependency("root.rarity.lib")]
    [BepInDependency("com.willuwontu.rounds.itemshops")]
    [BepInDependency("com.willuwontu.rounds.tabinfo", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("com.willuwontu.rounds.managers", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("root.classes.manager.reborn", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInPlugin(ModId, ModName, Version)]
    [BepInProcess("Rounds.exe")]
    public class Core:BaseUnityPlugin {
        private const string ModId = "Systems.R00t.CoreModual";
        private const string ModName = "Root Core";
        public const string Version = "1.4.0";
        public static ConfigEntry<bool> DEBUG;
        public static bool Credits;
        public static Core instance;
        private static CardCategory[] _noLotteryCategories;

        public static CardCategory[] NoLotteryCategories {
            get {
                if(_noLotteryCategories == null)
                    _noLotteryCategories = new CardCategory[] { CustomCardCategories.instance.CardCategory("CardManipulation"), CustomCardCategories.instance.CardCategory("NoRandom") };
                return _noLotteryCategories;
            }
        }

        void Awake() {
            instance = this;
            DEBUG = base.Config.Bind<bool>("Root", "Debug", false, "Enable to turn on concole spam from our mod");
            Credits = PlayerPrefs.GetInt(ModId + ".Credits", 1) != 0;

            Jotunn.Utils.AssetUtils.LoadAssetBundleFromResources("rootcore", typeof(Core).Assembly);

            var harmony = new Harmony(ModId);
            harmony.PatchAll(); 
        }

        void Start() {
            CardThemeLib.CardThemeLib.instance.CreateOrGetType("DarknessBlack", new CardThemeColor() { bgColor = new Color(0.1978f, 0.1088f, 0.1321f), targetColor = new Color(0.0978f, 0.1088f, 0.1321f) });
            CardThemeLib.CardThemeLib.instance.CreateOrGetType("RootCake", new CardThemeColor() { bgColor = new Color(0.7134047f, 0f, 1f, 0.5176471f), targetColor = new Color(0.2971698f, 0.6546086f, 1f) });
            CardThemeLib.CardThemeLib.instance.CreateOrGetType("Abnormality", new CardThemeColor() { bgColor = new Color(0.212f, 0.031f, 0.031f), targetColor = new Color(0.933f, 0.949f, 0.612f) });
            CardThemeLib.CardThemeLib.instance.CreateOrGetType("Inscryption", new CardThemeColor() { bgColor = new Color(0.51f, 0.455f, 0.333f), targetColor = new Color(0.641f, 0.222f, 0.143f) });
            
            if(BepInEx.Bootstrap.Chainloader.Plugins.Exists(plugin => plugin.Info.Metadata.GUID == "com.willuwontu.rounds.tabinfo")) {
                TabInfoRegesterer.Setup();
            }

            Unbound.RegisterMenu("Root Settings", delegate () { }, new Action<GameObject>(this.NewGUI), null, true);

            GameModeManager.AddHook(GameModeHooks.HookGameStart, GameStart);
            GameModeManager.AddHook(GameModeHooks.HookPointStart, PointStart);
            GameModeManager.AddHook(GameModeHooks.HookPointEnd, PointEnd);

            CardManager.categories.Add("Root");
            ModdingUtils.Utils.Cards.instance.AddCardValidationFunction((player, cardinfo) => cardinfo.sourceCard == null || cardinfo.sourceCard.allowMultiple || !player.HasCard(cardinfo.sourceCard));
        }

        public static void RegesterCards(CardList list, bool betaOverwriteDoNotUse = false) {

            string modVertion = (new StackTrace().GetFrame(1).GetMethod().ReflectedType.Assembly.GetName().Version.ToString() + " ").Replace(".0 ", "");

            foreach(RootCardInfo card in list.CardsToRegester) {
                if(card == null || !card.Build) continue;
                card.modVertion = modVertion;
                card.name = $"Root-Card  {card.Key} ({card.Tag})" + (betaOverwriteDoNotUse?"  BETA":"");
                if(betaOverwriteDoNotUse && CardList.ModCards.ContainsKey(card.Key)) {
                    CardManager.cards.Remove(CardList.ModCards[card.Key].name);
                    ModdingUtils.Utils.Cards.instance.hiddenCards.Remove(CardList.ModCards[card.Key]);
                    CardList.ModCards.Remove(card.Key);
                }
                CardList.ModCards.Add(card.Key, card);
                PhotonNetwork.PrefabPool.RegisterPrefab(card.name, card.gameObject);
                CustomCardCategories.instance.UpdateAndPullCategoriesFromCard(card);
                if(card.Hidden) {
                    ModdingUtils.Utils.Cards.instance.AddHiddenCard(card);
                } else if(card.Restricted) {
                    ModdingUtils.Utils.Cards.instance.AddHiddenCard(card);
                    instance.ExecuteAfterFrames(15, () =>
                        ((ObservableCollection<CardInfo>)typeof(CardManager).GetField("activeCards", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static).GetValue(null)).Add(card));
                } else {
                    if(card.Tag == "Root")
                        CardManager.cards.Add(card.name, new Card($"{card.Tag} ({list.name})", Unbound.config.Bind("Cards: " + card.Tag, card.name, !card.StartDisabled), card));
                    else
                        CardManager.cards.Add(card.name, new Card($"{card.Tag}", Unbound.config.Bind("Cards: " + card.Tag, card.name, !card.StartDisabled), card));
                }

                if(card.categories.Contains(CustomCardCategories.instance.CardCategory("SkipReroleCard")))
                    WillInterface.cardsSkippedForRerolls(card);

                card.Setup();
                foreach(CardCondition condition in card.GetComponents<CardCondition>()) {
                    ModdingUtils.Utils.Cards.instance.AddCardValidationFunction((player, cardinfo) => cardinfo != card || condition.IsPlayerAllowedCard(player));
                }
            }
        }

        private void NewGUI(GameObject menu) {
            TextMeshProUGUI textMeshProUGUI;
            MenuHandler.CreateText("Root Settings", menu, out textMeshProUGUI, 60, false, null, null, null, null);
            MenuHandler.CreateToggle(Credits, "Card Credits", menu, delegate (bool value) {
                Credits = value;
                PlayerPrefs.SetInt(ModId + ".Credits", value ? 1 : 0);
            }, 50, false, Color.magenta, null, null, null);
            MenuHandler.CreateToggle(DEBUG.Value, "Debug Mode", menu, delegate (bool value) {
                DEBUG.Value = value;
            }, 50, false, Color.red, null, null, null);
        }
        internal IEnumerator GameStart(IGameModeHandler gm) {
            PlayerManager.instance.players.ForEach(player => {
                if(player.data.view.IsMine) {
                    NetworkingManager.RPC(typeof(Core), nameof(SetSteamID), player.playerID, SteamUser.GetSteamID().m_SteamID.ToString());
                    Debug($"My SteamID is {SteamUser.GetSteamID().m_SteamID}");
                }
            });
            yield break;
        }

        internal IEnumerator PointStart(IGameModeHandler gm) {
            PlayerManager.instance.players.ForEach(player => {
                player.data.maxHealth += player.GetRootData().flatHPboost;
                player.data.health += player.GetRootData().flatHPboost;
                player.GetRootData().tempflatHPboost = player.GetRootData().flatHPboost;
            });

            yield break;
        }
        internal IEnumerator PointEnd(IGameModeHandler gm) {
            PlayerManager.instance.players.ForEach(player => {
                player.data.maxHealth -= player.GetRootData().tempflatHPboost;
            });

            yield break;
        }

        [UnboundLib.Networking.UnboundRPC]
        static void SetSteamID(int playerID, string steamID) {
            Debug($"Player{playerID}'s SteamID is {steamID}");
            if(PlayerManager.instance.players.Find(p => p.playerID == playerID) is Player player) {
                player.GetRootData().SteamID = steamID;
            }
        }


        public static void AddCardRquirement(CardInfo card, Func<Player, bool> func) {
            ModdingUtils.Utils.Cards.instance.AddCardValidationFunction((p, c) => c == card && func(p));
        }


        public static void Debug(object message) {
            if(message is null)
                message = "{{NULL}}";
            if(DEBUG.Value) {
                UnityEngine.Debug.Log("ROOT=>" + message);
            }
        }
    }
}
