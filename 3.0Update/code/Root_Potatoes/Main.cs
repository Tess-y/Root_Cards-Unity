using BepInEx;
using HarmonyLib;
using RootCore;
using UnityEngine;

namespace RootPotatoes {

    [BepInDependency("Systems.R00t.CoreModual")]
    [BepInPlugin(ModId, ModName, Version)]
	[BepInProcess("Rounds.exe")]
	public class Main:BaseUnityPlugin {
		private const string ModId = "Systems.R00t.Potato";
		private const string ModName = "Potatoes";
		public const string Version = "1.0.0";

		void Awake() {
			var harmony = new Harmony(ModId);
			harmony.PatchAll();
			Core.RegesterCards(Jotunn.Utils.AssetUtils.LoadAssetBundleFromResources("potatoassets", typeof(Main).Assembly).LoadAsset<GameObject>("Potato").GetComponent<CardList>());
		}


		void Start() {

		}
	}
}
