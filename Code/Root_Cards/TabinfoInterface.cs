using System;
using TabInfo.Utils;
using UnboundLib;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using HarmonyLib;
using ItemShops.Extensions;
using Landfall.Network;
using Photon.Pun;
using System.Reflection;
using Steamworks;
using UnboundLib.Utils;

internal class TabinfoInterface {
    public static void Setup() {
        var cat = TabInfoManager.RegisterCategory("Root Stats", 6);
        TabInfoManager.RegisterStat(cat, "Eternal Card", (p) => p.data.stats.GetRootData().lockedCard!=null, (p) => $"{p.data.stats.GetRootData().lockedCard.cardName}");
        TabInfoManager.RegisterStat(cat, "Wishes", (p) => p.GetAdditionalData().bankAccount.HasFunds(Genie.wishes), (p) => $"{p.GetAdditionalData().bankAccount.Money["Wish"]}");
        TabInfoManager.RegisterStat(cat, "Block Efectiveness", (p) => p.data.stats.GetRootData().shieldEfectiveness!=1, (p) => $"{Math.Round(p.data.stats.GetRootData().shieldEfectiveness*100, 1)}");
        TabInfoManager.RegisterStat(cat, "HP Culling", (p) => p.data.stats.GetRootData().hpCulling!=0, (p) => $"{Math.Round(p.data.stats.GetRootData().hpCulling*100, 1)}%");
        TabInfoManager.RegisterStat(cat, "ROOT:", (p) => p.data.stats.GetRootData().SteamID=="76561198060618523", (p) => "Player Is Root");
    }
}
