using BepInEx;
using HarmonyLib;
using RootCore;
using System.Reflection;
using UnityEngine;


[assembly: AssemblyVersionAttribute(RootCurses.Main.Version)]
namespace RootCurses {

    [BepInDependency("Systems.R00t.CoreModual")]
    [BepInDependency("com.willuwontu.rounds.managers")]
    [BepInDependency("com.root.player.time")]
    [BepInPlugin(ModId, ModName, Version)]
	[BepInProcess("Rounds.exe")]
	public class Main:BaseUnityPlugin {
		private const string ModId = "Systems.R00t.Curses";
		private const string ModName = "Root Curses";
		public const string Version = "1.1.1";

		void Awake() {
			var harmony = new Harmony(ModId);
			harmony.PatchAll();
			Core.RegesterCards(Jotunn.Utils.AssetUtils.LoadAssetBundleFromResources("curseassets", typeof(Main).Assembly).LoadAsset<GameObject>("Curse").GetComponent<CardList>());
		}


		void Start() {

		}
	}
}
