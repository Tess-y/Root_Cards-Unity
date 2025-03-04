using System.Linq;
using UnboundLib;
using WillsWackyManagers.Utils;

namespace RootCore {
    public class WillInterface {
        public static void FlagCurse(CardInfo card) {
            Core.instance.ExecuteAfterFrames(3, () => {
                CurseManager.instance.RegisterCurse(card);
                var categories = card.categories.ToList();
                categories.Add(CurseManager.instance.curseCategory);
                card.categories = categories.ToArray();
            });
        }

        internal static void cardsSkippedForRerolls(RootCardInfo card) {
            RerollManager.instance.cardsSkippedForRerolls.Add(card);
        }
    }
}
