using Rarity_Bundle;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RootCurses.Effects {
    public class RainbowCard:MonoBehaviour {
        CardVisuals visuals;
        List<CardThemeColor> cardThemeColors;
        System.Random random = new System.Random();
        public void Start() {
            visuals = GetComponentInChildren<CardVisuals>();
            cardThemeColors = CardChoice.instance.cardThemes.ToList();
        }
        public void Update() {
            Color color = GetComponentInChildren<UniqueRarityColor>().gameObject.GetComponent<CanvasRenderer>().GetColor();
            if(visuals.isSelected) {
                visuals.defaultColor = color;
                visuals.chillColor = visuals.defaultColor;
                for(int i = 0; i < visuals.images.Length; i++) {
                    visuals.images[i].color = color;
                }
                visuals.nameText.color = visuals.defaultColor;
            }
        }
    }
}