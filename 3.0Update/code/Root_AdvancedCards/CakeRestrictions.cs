using RootCore;
using System.Linq;


namespace RootAdvancedCards {
    public class CakeRestrictions {
        public static string[] cakes = new string[10]{
            "Cake_Trinket",
            "Cake_Common",
            "Cake_Scarce",
            "Cake_Uncommon",
            "Cake_Exotic",
            "Cake_Rare",
            "Cake_Epic",
            "Cake_Legendary",
            "Cake_Mythical",
            "Cake_Divine"
        };
        public static bool CanHaveCake(Player player, RootCardInfo Cake) {
            if(!Cake.Key.Contains("Cake_")) return true;
            if(player is null) return false;
            int cakesCount = player.data.currentCards.Where(card => card is RootCardInfo rootCard && rootCard.Key.StartsWith("Cake_")).Count();
            return (cakes.IndexOf(Cake.Key) -3) < cakesCount;
        }
    }
}
