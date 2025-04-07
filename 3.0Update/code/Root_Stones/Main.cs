using BepInEx;
using HarmonyLib;
using RootCore;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;


[assembly: AssemblyVersionAttribute(RootStones.Main.Version)]
namespace RootStones {

    [BepInDependency("root.classes.manager.reborn")]
    [BepInDependency("Systems.R00t.CoreModual")]
    [BepInPlugin(ModId, ModName, Version)]
	[BepInProcess("Rounds.exe")]
	public class Main:BaseUnityPlugin {
		private const string ModId = "Systems.R00t.InfinityStones";
		private const string ModName = "Root Stones";
		public const string Version = "1.2.1";
        public static AssetBundle Assets;
		public static List<string> Stones = new List<string>() { "Time_Stone", "Mind_Stone", "Reality_Stone", "Space_Stone", "Soul_Stone", "Power_Stone" };

        void Awake() {
			var harmony = new Harmony(ModId);
			harmony.PatchAll();

			Assets = Jotunn.Utils.AssetUtils.LoadAssetBundleFromResources("stoneassets", typeof(Main).Assembly);
            Core.RegesterCards(Assets.LoadAsset<GameObject>("Stone").GetComponent<CardList>());
		}


		void Start() {
			TabInfoRegesterer.RegesterInfo(TabInfoRegesterer.cat, "Stones", HasStones, GetStoneString);
		}

		bool HasStones(Player player) {
			if(player == null) return false;
			foreach (string stone in Stones)
				if(player.HasCard(stone)) return true;

			return false;
		}

		string GetStoneString(Player player) {

            if(player == null) return "ERROR";
            int stonecount = 0;
            foreach(string stone in Stones)
                if(player.HasCard(stone)) stonecount++;
			if(stonecount == 6 && player.HasCard("Gauntlet")) return "Empowered";
			return $"{stonecount}/6";

        }
	}
}
