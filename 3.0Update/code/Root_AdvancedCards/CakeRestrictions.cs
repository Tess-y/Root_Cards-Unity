using ClassesManagerReborn;
using RootCore;
using System.Collections;


namespace RootAdvancedCards {
    public class CakeRestrictions:ClassHandler {
        public override IEnumerator Init() {
            ClassesRegistry.Register(CardList.GetCardInfo("Cake_Trinket"), CardType.Entry | CardType.NonClassCard);
            ClassesRegistry.Register(CardList.GetCardInfo("Cake_Common"), CardType.Gate | CardType.NonClassCard, CardList.GetCardInfo("Cake_Trinket"));
            ClassesRegistry.Register(CardList.GetCardInfo("Cake_Scarce"), CardType.Card | CardType.NonClassCard, CardList.GetCardInfo("Cake_Common"));
            ClassesRegistry.Register(CardList.GetCardInfo("Cake_Uncommon"), CardType.Card | CardType.NonClassCard, CardList.GetCardInfo("Cake_Scarce"));
            ClassesRegistry.Register(CardList.GetCardInfo("Cake_Exotic"), CardType.Card | CardType.NonClassCard, CardList.GetCardInfo("Cake_Uncommon"));
            ClassesRegistry.Register(CardList.GetCardInfo("Cake_Rare"), CardType.Card | CardType.NonClassCard, CardList.GetCardInfo("Cake_Exotic"));
            ClassesRegistry.Register(CardList.GetCardInfo("Cake_Epic"), CardType.Card | CardType.NonClassCard, CardList.GetCardInfo("Cake_Rare"));
            ClassesRegistry.Register(CardList.GetCardInfo("Cake_Legendary"), CardType.Card | CardType.NonClassCard, CardList.GetCardInfo("Cake_Epic"));
            ClassesRegistry.Register(CardList.GetCardInfo("Cake_Mythical"), CardType.Card | CardType.NonClassCard, CardList.GetCardInfo("Cake_Legendary"));
            ClassesRegistry.Register(CardList.GetCardInfo("Cake_Divine"), CardType.Card | CardType.NonClassCard, CardList.GetCardInfo("Cake_Mythical"));
            yield break;
        }
    }
}
