using UnityEngine;

namespace RootCore {
    public class HandAjuster : MonoBehaviour
	{
		private Player player;
		private int actuallAdjustment;
		public int Adjustment;
		

	    void Awake()
	    {
	        player = GetComponentInParent<Player>();
	    }

		void Start() {
			var draws = DrawNCards.DrawNCards.GetPickerDraws(player.playerID);
            actuallAdjustment = Mathf.Clamp(draws + Adjustment, 1, 30) - draws;
            DrawNCards.DrawNCards.SetPickerDraws(player.playerID, draws + actuallAdjustment);
        }

		void OnDestroy() {
            var draws = DrawNCards.DrawNCards.GetPickerDraws(player.playerID);
            DrawNCards.DrawNCards.SetPickerDraws(player.playerID, draws - actuallAdjustment);
        }
	}
}
