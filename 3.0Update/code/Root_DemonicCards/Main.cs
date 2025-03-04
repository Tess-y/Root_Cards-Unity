using BepInEx;
using HarmonyLib;
using RootCore;
using System.Collections;
using UnboundLib.GameModes;
using UnityEngine;


namespace RootDemonicCards {
    [BepInDependency("root.classes.manager.reborn")]
    [BepInDependency("Systems.R00t.CoreModual")]
    [BepInPlugin(ModId, ModName, Version)]
    [BepInProcess("Rounds.exe")]
    public class Main:BaseUnityPlugin {
        private const string ModId = "Systems.R00t.DemonicCards";
        private const string ModName = "Demonic Cards";
        public const string Version = "1.1.0";

        public static AssetBundle Assets;

        void Awake() {
            var harmony = new Harmony(ModId);
            harmony.PatchAll();
            Assets = Jotunn.Utils.AssetUtils.LoadAssetBundleFromResources("demonassets", typeof(Main).Assembly);
            Core.RegesterCards(Assets.LoadAsset<GameObject>("Demonic").GetComponent<CardList>());
        }


        void Start() {

            GameModeManager.AddHook(GameModeHooks.HookGameStart, GameStart);
        }
        internal void SetUpContract() {
            RarityLib.Utils.RarityUtils.SetCardRarityModifier(CardList.GetCardInfo("Contract"), 20);
            RarityLib.Utils.RarityUtils.SetCardRarityModifier(CardList.GetCardInfo("Dark_Queen"), 200);
        }

        internal IEnumerator GameStart(IGameModeHandler gm) {
            SetUpContract();
            yield break;
        }
    }
}
