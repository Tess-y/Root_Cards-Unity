using ClassesManagerReborn;
using RootCore;
using System.Collections;

namespace RootStones {
    public class Gauntlet:ClassHandler {

        public static RootCardInfo[] stones;
	    public override IEnumerator Init() {

            stones = new[] { CardList.GetCardInfo("Time_Stone"), CardList.GetCardInfo("Mind_Stone"),
                 CardList.GetCardInfo("Reality_Stone"),CardList.GetCardInfo("Space_Stone"),CardList.GetCardInfo("Soul_Stone"),CardList.GetCardInfo("Power_Stone"),};

            ClassesRegistry.Register(CardList.GetCardInfo("Quest"), CardType.Entry);

            foreach(var stone in stones) { ClassesRegistry.Register(stone, CardType.Card, CardList.GetCardInfo("Quest")); }

            ClassesRegistry.Register(CardList.GetCardInfo("Gauntlet"), CardType.Card, stones);
	        yield break;
	    }
	}
}
