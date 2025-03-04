﻿using RootCore;
using UnityEngine;
using UnityEngine.UI;

namespace RootStones {
    public class TitanHandler : MonoBehaviour{

        Image hpSheild;
        Player player;

        void Start() {
            player = GetComponentInParent<Player>();
            var hp = transform.parent.gameObject.GetComponentInChildren<HealthBar>().hp;
            hpSheild = Instantiate(hp,hp.transform.parent);
            hpSheild.color = new Color(0.449f,0f,0.597f);
        }

        void Update() {
            float hpBlocked = player.data.health - ((player.GetRootData().damageCap * player.data.maxHealth) - player.GetRootData().damageCapFilled);
            hpSheild.fillAmount = Mathf.Clamp(0, hpBlocked/player.data.maxHealth, 1);

        }

        void OnDestroy() { 
            Destroy(hpSheild);
        }

    }
}
