using Photon.Realtime;
using RootCore;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnboundLib;
using UnboundLib.GameModes;
using UnityEngine;

namespace RootStones {
    public class PointErasure : MonoBehaviour
	{
		Player player;
		Dictionary<int,int> lastTeamRounds = new Dictionary<int,int>();
	    void Start()
	    {
			player = GetComponentInParent<Player>();
			GameModeManager.AddHook(GameModeHooks.HookPointEnd, OnPointEnd);
			GameModeManager.AddHook(GameModeHooks.HookRoundEnd, OnRoundEnd);
			lastTeamRounds = ((Dictionary<int, int>)GameModeManager.CurrentHandler.GameMode.GetFieldValue("teamRounds")).ToDictionary(entry => entry.Key, entry => entry.Value); ;

        }
	
		void OnDestroy() {

            GameModeManager.RemoveHook(GameModeHooks.HookPointEnd, OnPointEnd);
            GameModeManager.RemoveHook(GameModeHooks.HookRoundEnd, OnRoundEnd);
        }

		internal IEnumerator OnPointEnd(IGameModeHandler gm) {
            Dictionary<int, int> teamRounds = (Dictionary<int, int>)GameModeManager.CurrentHandler.GameMode.GetFieldValue("teamRounds");
			bool endOfRound= teamRounds.Keys.Count() != lastTeamRounds.Keys.Count();
			foreach(var kvp in lastTeamRounds) {
				if(!teamRounds.ContainsKey(kvp.Key) || teamRounds[kvp.Key] != kvp.Value) endOfRound = true;
			}
			if(endOfRound) {
                lastTeamRounds = teamRounds.ToDictionary(entry => entry.Key, entry => entry.Value); ;
				yield break;
            }
            if(!gm.GetPointWinners().Contains(player.teamID)) {
                yield break;
            }
            var teams = PlayerManager.instance.players.Select(p => p.teamID).Distinct();
			int LeadingTeam = teams.Where(id => id != player.teamID).OrderBy(id => gm.GetTeamScore(id).points).Last();
			TeamScore score = gm.GetTeamScore(LeadingTeam);
			gm.SetTeamScore(LeadingTeam, new TeamScore(score.points-1,score.rounds));
        }

		internal IEnumerator OnRoundEnd(IGameModeHandler gm) {
			if(!player.HasCard("Gauntlet")) yield break;
			if(!gm.GetRoundWinners().Contains(player.teamID)) {
                yield break;
            }
            var teams = PlayerManager.instance.players.Select(p => p.teamID).Distinct();
			int LeadingTeam = teams.Where(id => id != player.teamID).OrderBy(id => gm.GetTeamScore(id).rounds).Last();
			TeamScore score = gm.GetTeamScore(LeadingTeam);
			gm.SetTeamScore(LeadingTeam, new TeamScore(score.points,score.rounds-1));
        }
	}
}
