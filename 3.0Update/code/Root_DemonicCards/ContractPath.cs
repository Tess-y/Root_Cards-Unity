using ClassesManagerReborn;
using RootCore;
using System.Collections;

namespace RootDemonicCards {
    public class ContractPath: ClassHandler {
	    public override IEnumerator Init() {
	        ClassesRegistry.Register(CardList.GetCardInfo("Lilith_Deal"), CardType.Entry|CardType.NonClassCard);
	        ClassesRegistry.Register(CardList.GetCardInfo("Contract"), CardType.Gate|CardType.NonClassCard, CardList.GetCardInfo("Lilith_Deal"));
	        ClassesRegistry.Register(CardList.GetCardInfo("Dark_Queen"), CardType.Card|CardType.NonClassCard, CardList.GetCardInfo("Contract"));
	        yield break;
	    }
	}
}
