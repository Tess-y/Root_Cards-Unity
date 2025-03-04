using RootCore.CardConditions;
using System.Linq;
using UnboundLib.GameModes;

namespace RootAdvancedCards.CardConditions {
    public class TooSoonCondition:CardCondition {
		public override bool IsPlayerAllowedCard(Player player) {
			var Teams = PlayerManager.instance.players.Select(x => x.teamID).Distinct().ToList();
			int toWin = (int)GameModeManager.CurrentHandler.Settings["roundsToWinGame"];
			bool tooSoon = false;
			foreach(var team in Teams) {
				if(GameModeManager.CurrentHandler.GetTeamScore(team).rounds == toWin - 1) {
					if(tooSoon) return false;
					tooSoon = true;
				} else if(GameModeManager.CurrentHandler.GetTeamScore(team).rounds != 0) {
					return false;
				}
			}
			return tooSoon;
		}

	}
}
