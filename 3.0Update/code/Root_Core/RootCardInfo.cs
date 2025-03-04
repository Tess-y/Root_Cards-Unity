using ModdingUtils.Extensions;
using System.Linq;
using TMPro;
using UnboundLib;
using UnityEngine;

namespace RootCore {
    public class RootCardInfo : CardInfo
	{
		public enum CardRarity {
			Trinket,
            Common,
			Scarce,
			Uncommon,
			Exotic,
			Rare,
			Epic,
			Legendary,
			Mythical,
			Divine,
            Unique
        }


        [Header("Root Settings")]
        public CardRarity cardRarity;
        public string Key;
        public bool Hidden = false;
        public bool Restricted = false;
        public bool Reassign = true;
        public bool AntiCard = false;
        public bool IsCurse = false;
        public bool Nullable = true;
        public bool NeedsNull = false;
        public bool PickPhaseOnly = false;
        public string Tag = "root"; 
        public string Author;
        public bool Build = true;
        public bool StartDisabled = false;
        public bool Perminent = false;

        public CardInfo AlternetSource;

        public static Rarity GetRarity(CardRarity rarity) {
            return RarityLib.Utils.RarityUtils.GetRarity(rarity.ToString());
        }

        public void Setup() {
            GetComponent<CardInfo>().GetAdditionalData().canBeReassigned = Reassign;
            if(Core.plugins.Exists(plugin => plugin.Info.Metadata.GUID == "com.Root.Null")) {
                if(AntiCard)
                    NullInterface.SetAntiCard(GetComponent<CardInfo>());
                if(!Nullable)
                    NullInterface.MarkUnNullable(GetComponent<CardInfo>());
                if(NeedsNull)
                    NullInterface.NeedsNull(GetComponent<CardInfo>());
            }
            if(IsCurse) {
                WillInterface.FlagCurse(GetComponent<CardInfo>());
            }
            Core.instance.ExecuteAfterFrames(5, () => rarity = GetRarity(cardRarity));
            if(cardRarity == CardRarity.Unique || Perminent) allowMultiple = false;
            sourceCard = AlternetSource ?? this;
        }

        public void Start() {
            if(AlternetSource != null) sourceCard = AlternetSource;
            if(Core.Credits) {
                GameObject modNameObj = new GameObject("ModNameText");
                RectTransform[] allChildrenRecursive = gameObject.GetComponentsInChildren<RectTransform>();
                var edgeTransform = allChildrenRecursive.FirstOrDefault(obj => obj.gameObject.name == "EdgePart (1)");
                if(edgeTransform != null) {
                    GameObject bottomLeftCorner = edgeTransform.gameObject;
                    modNameObj.gameObject.transform.SetParent(bottomLeftCorner.transform);
                }

                TextMeshProUGUI modText = modNameObj.gameObject.AddComponent<TextMeshProUGUI>();
                modText.text = Tag;
                modNameObj.transform.localEulerAngles = new Vector3(0f, 0f, 135f);

                modNameObj.transform.localScale = Vector3.one;
                modNameObj.transform.localPosition = new Vector3(-50f, -50f, 0f);
                modText.alignment = TextAlignmentOptions.Bottom;
                modText.alpha = 0.1f;
                modText.fontSize = 60;


                modNameObj = new GameObject("AuthorNameText");
                edgeTransform = allChildrenRecursive.FirstOrDefault(obj => obj.gameObject.name == "EdgePart (2)");
                if(edgeTransform != null) {
                    GameObject bottomLeftCorner = edgeTransform.gameObject;
                    modNameObj.gameObject.transform.SetParent(bottomLeftCorner.transform);
                }

                modText = modNameObj.gameObject.AddComponent<TextMeshProUGUI>();
                modText.text = $"Card Idea By:\n{Author}";
                modText.autoSizeTextContainer = true;
                modNameObj.transform.localEulerAngles = new Vector3(0f, 0f, 135f);

                modNameObj.transform.localScale = Vector3.one;
                modNameObj.transform.localPosition = new Vector3(-75f, -75f, 0f);
                modText.alignment = TextAlignmentOptions.Bottom;
                modText.alpha = 0.1f;
                modText.fontSize = 54;
            }
        }

    }
}
