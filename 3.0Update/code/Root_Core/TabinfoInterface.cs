
using System;
using System.Collections.Generic;
using TabInfo.Utils;

namespace RootCore {
    internal class TabinfoInterface {

        private static Dictionary<string, StatCategory> Categories = new Dictionary<string, StatCategory>();

        internal static void RegesterInfo(string category, string name, Func<Player, bool> displayCondition, Func<Player, string> displayValue, int priority) {
            
            if(!Categories.ContainsKey(category)) { Categories.Add(category, TabInfoManager.RegisterCategory(category, priority)); }
            var cat = Categories[category];
            TabInfoManager.RegisterStat(cat,name,displayCondition, displayValue);
        }

        internal static void SetUp() {
            var calculation = TabInfoManager.basicStats._stats["damage"].displayValue;
            TabInfoManager.basicStats._stats["damage"].displayValue = (player) => 
                string.Format("{0:F0}", float.Parse(calculation(player)) + player.GetRootData().flatProjectileDamage);
        }
    }
}
