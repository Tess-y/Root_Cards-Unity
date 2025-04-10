using BepInEx;
using HarmonyLib;
using RootCore;
using System.Reflection;
using UnityEngine;


[assembly: AssemblyVersionAttribute(RootStandardCards.Main.Version)]
namespace RootStandardCards {
    [BepInDependency("Systems.R00t.CoreModual")]
    [BepInPlugin(ModId, ModName, Version)]
	[BepInProcess("Rounds.exe")]
	public class Main:BaseUnityPlugin {
		private const string ModId = "Systems.R00t.Standard";
		private const string ModName = "Root Standard Cards ";
		public const string Version = "1.1.3";

		void Awake() {
			var harmony = new Harmony(ModId);
			harmony.PatchAll();
			var _ = Jotunn.Utils.AssetUtils.LoadAssetBundleFromResources("standardassets", typeof(Main).Assembly);

            Core.RegesterCards(_.LoadAsset<GameObject>("Standard").GetComponent<CardList>());
            _.LoadAsset<GameObject>("StandardPhotonPool").GetComponent<PhotonPool>().Regester();
        }


		void Start() {

		}
	}
}