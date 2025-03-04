using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnboundLib;
using UnboundLib.Utils;
using UnboundLib.Utils.UI;
using UnityEngine;
using UnityEngine.UI;

namespace RootCore.Patches {
    [HarmonyPatch(typeof(ToggleCardsMenuHandler),"Start")]
    public class ToggleCardsMenuPatch {

        public static void Postfix() {
            Core.instance.ExecuteAfterSeconds(0.75f, () => {
                List<Transform> catagorys = new List<Transform>();
                Transform categoryContent = (Transform)typeof(ToggleCardsMenuHandler).GetField("categoryContent", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(ToggleCardsMenuHandler.instance);
                Dictionary<string, Transform> scrollViews = new Dictionary<string, Transform>();
                int count = categoryContent.childCount;
                for(int i = 0; i < count; i++) {
                    Transform child = categoryContent.GetChild(i);
                    scrollViews.Add(child.name, child);
                }
                foreach(var category in CardManager.categories) {
                    catagorys.Add(scrollViews[category]);
                }
                Transform rootCards = scrollViews["Root"]; 
                catagorys.Remove(rootCards);
                List<Transform> rootCatagorys = new List<Transform>();
                foreach(var category in scrollViews) {
                    if(category.Key.StartsWith("Root (")) {
                        rootCatagorys.Add(category.Value);
                        category.Value.gameObject.SetActive(false);
                    }
                }
                catagorys.RemoveAll(rootCatagorys.Contains);

                rootCards.GetComponent<Button>().onClick.AddListener(() => {
                    rootCatagorys.ForEach(catagory => {
                        if(!catagory.gameObject.activeSelf) {
                            catagory.gameObject.SetActive(true);
                            Core.instance.ExecuteAfterFrames(2, () => { catagory.localPosition += Vector3.right * 8; });
                        }
                    });
                });

                foreach(var otherCategory in catagorys) {
                    otherCategory.GetComponent<Button>().onClick.AddListener(() => {
                        rootCatagorys.ForEach(catagory => {
                            catagory.gameObject.SetActive(false);
                        });
                    });
                }
                rootCards.GetComponentInChildren<Toggle>().gameObject.SetActive(false);
                var text = ToggleCardsMenuHandler.scrollViews["Root"].Find("Viewport/Content").gameObject.AddComponent<TextMeshProUGUI>();
                text.color = new Color(0.6f, 0.5f, 0.8f);
                text.text = "  \r\n   Welcome to Root Cards,\r\n   this is a modular mod created by Lilith and Tessy.\r\n   \r\n   Cards added by different modules are separated into\r\n   sub-categories seen to the right under the 'Root' tab.\r\n   \r\n   If no sub-categories showed up after selecting 'Root'\r\n   it is likely that you do not have any modules installed.\r\n   please note that the core module does \r\n   almost nothing by itself, merely adding functionality\r\n   for the actual content moduals.\r\n   \r\n   We hope you enjoy our cards.\r\n   \r\n   If you have any feedback, or would like to report a bug,\r\n   please reach out to @__root__ in the RMC discord server.\r\n   ";
                text.fontSize = 20;
            });
        }

    }
}
