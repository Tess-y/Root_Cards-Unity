using UnityEngine;

namespace RootCore.CardConditions {
    public abstract class CardCondition:MonoBehaviour {
		[HideInInspector]
		public CardInfo cardInfo { get { return this.GetComponent<CardInfo>(); } }
		public abstract bool IsPlayerAllowedCard(Player player);

	}
}
