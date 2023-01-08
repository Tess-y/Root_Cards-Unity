using System.Linq;
using ModdingUtils.Extensions;
using Nullmanager;
using TMPro;
using UnityEngine;
using UnboundLib;

public class RootCardInfo: CardInfo {
    [Header("Root Settings")]
    public string Key;
    public bool Hidden = false;
    public bool Reassign = true;
    public bool AntiCard = false;
    public bool IsCurse = false;
    public bool Nullable = true;
    public bool NeedsNull = false;
    public bool PickPhaseOnly = false;
    public string Tag = "root";

    public string Author;

    public void Setup() {
        GetComponent<CardInfo>().GetAdditionalData().canBeReassigned=Reassign;
        if(AntiCard) {
            GetComponent<CardInfo>().SetAntiCard();
            GetComponent<CardInfo>().cardBase=NullManager.instance.AntiCardBase;
        }
        if(!Nullable)
            GetComponent<CardInfo>().MarkUnNullable();
        if(NeedsNull)
            GetComponent<CardInfo>().NeedsNull();
        if(IsCurse) {
            RootCards.instance.ExecuteAfterFrames(3, () => {
                WillsWackyManagers.Utils.CurseManager.instance.RegisterCurse(GetComponent<CardInfo>());
                var categories = GetComponent<CardInfo>().categories.ToList();
                categories.Add(WillsWackyManagers.Utils.CurseManager.instance.curseCategory);
                GetComponent<CardInfo>().categories=categories.ToArray();
            });
        }
    }

    public void Start() {
        if(RootCards.Credits.Value) {
            GameObject modNameObj = new GameObject("ModNameText");
            RectTransform[] allChildrenRecursive = gameObject.GetComponentsInChildren<RectTransform>();
            var edgeTransform = allChildrenRecursive.FirstOrDefault(obj => obj.gameObject.name=="EdgePart (1)");
            if(edgeTransform!=null) {
                GameObject bottomLeftCorner = edgeTransform.gameObject;
                modNameObj.gameObject.transform.SetParent(bottomLeftCorner.transform);
            }

            TextMeshProUGUI modText = modNameObj.gameObject.AddComponent<TextMeshProUGUI>();
            modText.text=Tag;
            modNameObj.transform.localEulerAngles=new Vector3(0f, 0f, 135f);

            modNameObj.transform.localScale=Vector3.one;
            modNameObj.transform.localPosition=new Vector3(-50f, -50f, 0f);
            modText.alignment=TextAlignmentOptions.Bottom;
            modText.alpha=0.1f;
            modText.fontSize=60;


            modNameObj=new GameObject("AuthorNameText");
            edgeTransform=allChildrenRecursive.FirstOrDefault(obj => obj.gameObject.name=="EdgePart (2)");
            if(edgeTransform!=null) {
                GameObject bottomLeftCorner = edgeTransform.gameObject;
                modNameObj.gameObject.transform.SetParent(bottomLeftCorner.transform);
            }

            modText=modNameObj.gameObject.AddComponent<TextMeshProUGUI>();
            modText.text=$"Designed by:\n{Author}";
            modText.autoSizeTextContainer=true;
            modNameObj.transform.localEulerAngles=new Vector3(0f, 0f, 135f);

            modNameObj.transform.localScale=Vector3.one;
            modNameObj.transform.localPosition=new Vector3(-75f, -75f, 0f);
            modText.alignment=TextAlignmentOptions.Bottom;
            modText.alpha=0.1f;
            modText.fontSize=54;
        }
    }
}
