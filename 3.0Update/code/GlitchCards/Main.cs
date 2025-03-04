using BepInEx;
using HarmonyLib;
using RootCore;
using UnityEngine;

namespace GlitchCards {

    [BepInDependency("Systems.R00t.CoreModual")]
    [BepInPlugin(ModId, ModName, Version)]
	[BepInProcess("Rounds.exe")]
	public class Main:BaseUnityPlugin {
		private const string ModId = "Systems.R00t.Glitch";
		private const string ModName = "Glitched Cards";
		public const string Version = "1.0.0";

		void Awake() {
			var harmony = new Harmony(ModId);
			harmony.PatchAll();
			Core.RegesterCards(Jotunn.Utils.AssetUtils.LoadAssetBundleFromResources("glitchassets", typeof(Main).Assembly).LoadAsset<GameObject>("Glitch").GetComponent<CardList>());
		}


		void Start() {

		}
	}
}
