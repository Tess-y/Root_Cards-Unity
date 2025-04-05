using BepInEx;
using HarmonyLib;
using RootCore;
using System.Collections;
using System.Collections.Generic;
using UnboundLib.GameModes;
using UnityEngine;
using WillsWackyManagers.Utils;

namespace Genie {

    [BepInDependency("Systems.R00t.CoreModual")]
    [BepInDependency("com.Root.Null")]
    [BepInPlugin(ModId, ModName, Version)]
    [BepInProcess("Rounds.exe")]
    public class Main:BaseUnityPlugin {
        private const string ModId = "Systems.R00t.Genie";
        private const string ModName = "Genie";
        public const string Version = "1.1.3";


        void Awake() {
            var harmony = new Harmony(ModId);
            harmony.PatchAll();
            Core.RegesterCards(Jotunn.Utils.AssetUtils.LoadAssetBundleFromResources("genieassets", typeof(Main).Assembly).LoadAsset<GameObject>("Genie").GetComponent<CardList>());
        }


        void Start() {

            GameModeManager.AddHook(GameModeHooks.HookPlayerPickStart, (gm) => { StartCoroutine(NoBlock.HideChocieBlock()); return new List<object>().GetEnumerator(); });
            GameModeManager.AddHook(GameModeHooks.HookPickEnd, (System.Func<IGameModeHandler, IEnumerator>)((gm) => GenieCard.WaitTillShopDone()));
            GameModeManager.AddHook(GameModeHooks.HookGameStart, GameStart);

            RerollManager.instance.RegisterRerollAction(GenieCard.GenieRerollAction);
        }

        internal IEnumerator GameStart(IGameModeHandler gm) {
            yield return GenieCard.Wish();
            yield return GenieCard.RestCardLock();
        }
    }
}
