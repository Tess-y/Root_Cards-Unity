using BepInEx;
using HarmonyLib;
using RootCore;
using System.Reflection;
using UnityEngine;


[assembly: AssemblyVersionAttribute(RootBeta.Main.Version)]
namespace RootBeta {

    [BepInDependency("Systems.R00t.CoreModual")]
	[BepInPlugin(ModId, ModName, Version)]
	[BepInProcess("Rounds.exe")]
	public class Main:BaseUnityPlugin {
		private const string ModId = "Systems.R00t.Unstable";
		private const string ModName = "Root Cards BETA";
		public const string Version = "1.1.7";

		void Awake() {
			var harmony = new Harmony(ModId);
			harmony.PatchAll();
			Core.RegesterCards(Jotunn.Utils.AssetUtils.LoadAssetBundleFromResources("betaassets", typeof(Main).Assembly).LoadAsset<GameObject>("Beta").GetComponent<CardList>(), true);
		}

	}
}
