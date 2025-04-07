using ModdingUtils.Extensions;
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


        [HideInInspector]
        public string modVertion;

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
            if(BepInEx.Bootstrap.Chainloader.Plugins.Exists(plugin => plugin.Info.Metadata.GUID == "com.Root.Null")) {
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
            gameObject.GetComponentInChildren<CardVisuals>().transform.Find("Canvas/Front/ModVertionNumber").GetComponent<TextMeshProUGUI>().text = modVertion; 

            if(Core.Credits) {
                Transform modNameObj = gameObject.GetComponentInChildren<CardVisuals>().transform.Find("Canvas/Front/ModNameText");
                modNameObj.gameObject.SetActive(true);
                modNameObj.GetComponent<TextMeshProUGUI>().text = Tag;

                Transform modAuthorObj = gameObject.GetComponentInChildren<CardVisuals>().transform.Find("Canvas/Front/AuthorNameText");
                modAuthorObj.gameObject.SetActive(true);
                modNameObj.GetComponent<TextMeshProUGUI>().text = $"Card Idea By:\n{Author}";
            }
        }

    }
}
