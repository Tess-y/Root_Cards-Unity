using BepInEx;
using CardChoiceSpawnUniqueCardPatch.CustomCategories;
using HarmonyLib;
using RootCore;
using RootCore.ArtStuffs;
using System.Collections;
using System.Linq;
using UnboundLib.GameModes;
using UnityEngine;

namespace RootAdvancedCards {

    [BepInDependency("root.classes.manager.reborn")]
    [BepInDependency("Systems.R00t.CoreModual")]
    [BepInPlugin(ModId, ModName, Version)]
    [BepInProcess("Rounds.exe")]
    public class Main:BaseUnityPlugin {
        private const string ModId = "Systems.R00t.Advanced";
        private const string ModName = "Root Advanced Cards";
        public const string Version = "1.0.1";


        void Awake() {
            var harmony = new Harmony(ModId);
            harmony.PatchAll();
            var _ = Jotunn.Utils.AssetUtils.LoadAssetBundleFromResources("advancedassets", typeof(Main).Assembly);

            Core.RegesterCards(_.LoadAsset<GameObject>("Advanced").GetComponent<CardList>());
            _.LoadAsset<GameObject>("AdvancedPhotonPool").GetComponent<PhotonPool>().Regester();
        }



        void Start() {
            ModdingUtils.Utils.Cards.instance.AddCardValidationFunction((player, cardinfo) => {
                if(player.HasCard("Simulacrum")) {
                    if(!cardinfo.allowMultiple || cardinfo.categories.Intersect(new CardCategory[] {
                         CustomCardCategories.instance.CardCategory("GearUp_Card-Shuffle"),
                         CustomCardCategories.instance.CardCategory("CardManipulation"),
                         CustomCardCategories.instance.CardCategory("NoPreGamePick") //Shuffle has this, as far as i know it is the only card that does, and is the easyst way to blacklist shuffle.

                    }).Any()) {
                        return false;
                    }
                    if(cardinfo.rarity == RootCardInfo.GetRarity(RootCardInfo.CardRarity.Unique)) return false;
                }
                return true;
            });

            GameModeManager.AddHook(GameModeHooks.HookGameStart, GameStart);
            CardStatVarHandler.VarMap.Add("Ourobors", go=> $"{Oroboros.oroboros * 10}");
        }
        internal IEnumerator GameStart(IGameModeHandler gm) {
            Oroboros.oroboros = 1;
            yield break;
        }
    }
}
