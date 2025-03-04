using HarmonyLib;
using RootCore;
using System;
using System.Linq;
using TMPro;

namespace RootCurses.Patches {
    [HarmonyPatch(typeof(CardVisuals), "ChangeSelected")]
    public class BableEffect {
        public static char[] allChars = "1234567890+-*/=abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTU?WXYZ?.,".ToCharArray();
        public static void Prefix(CardVisuals __instance, bool setSelected, bool ___isSelected) {
            Core.Debug("BABLE");
            if(setSelected != ___isSelected && PlayerManager.instance.players.Any(p => p.data.view.IsMine && p.HasCard("Babel"))) {
                var texts = __instance.GetComponentsInChildren<TextMeshProUGUI>();
                var random = new Random();
                foreach(var text in texts) {
                    var chars = text.text.ToCharArray();
                    for(int i = 0; i < chars.Length; i++) {
                        if(!char.IsWhiteSpace(chars[i]))
                            chars[i] = allChars[random.Next(0, allChars.Length)];
                    }
                    text.text = chars.ArrayToString();
                }
            }
        }
    }

}
