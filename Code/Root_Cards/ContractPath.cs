using System.Collections;
using ClassesManagerReborn;

public class ContractPath: ClassHandler {
    public override IEnumerator Init() {
        ClassesRegistry.Register(CardResgester.ModCards["Lilith_Deal"], CardType.Entry|CardType.NonClassCard);
        ClassesRegistry.Register(CardResgester.ModCards["Contract"], CardType.Gate|CardType.NonClassCard, CardResgester.ModCards["Lilith_Deal"]);
        ClassesRegistry.Register(CardResgester.ModCards["Dark_Queen"], CardType.Card|CardType.NonClassCard, CardResgester.ModCards["Contract"]);
        yield break;
    }
}