using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnboundLib;
using UnboundLib.GameModes;
using UnboundLib.Networking;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI.ProceduralImage;

public class MortalityTimer:MonoBehaviour {

    private float timer = 0;
    public Player player;

    [Range(0f, 1f)]
    public float counter;

    public float timeToFill = 30f;

    public float timeToEmpty = 0f;

    public float duration = 1;

    public ProceduralImage outerRing;

    public ProceduralImage fill;

    public Transform rotator;

    public Transform still;

    public GameObject Scythe;

    public bool Enabled = false;
    public void Awake() {
        player = GetComponentInParent<Player>();
    }

    void Start() {
        GameModeManager.AddHook(GameModeHooks.HookPointEnd, Pause);
        GameModeManager.AddHook(GameModeHooks.HookRoundEnd, Reset);
        GameModeManager.AddHook(GameModeHooks.HookPointStart, Enable);
        outerRing.color = new Color32(106, 33, 145, 255);
        fill.color = new Color32(255, 0, 64, 150);
        counter = 0;
        outerRing.fillAmount = counter;
        fill.fillAmount = counter;
        rotator.transform.localEulerAngles = new Vector3(0f, 0f, -Mathf.Lerp(0f, 360f, counter));
    }
    void OnDestroy() {
        GameModeManager.RemoveHook(GameModeHooks.HookPointEnd, Pause);
        GameModeManager.RemoveHook(GameModeHooks.HookRoundEnd, Reset);
        GameModeManager.RemoveHook(GameModeHooks.HookPointStart, Enable);
    }

    void Update() {
        if(!Enabled || player.data.dead) return;
        timer += Time.deltaTime;

        counter = timer / timeToFill;
        outerRing.fillAmount = counter;
        fill.fillAmount = counter;
        rotator.transform.localEulerAngles = new Vector3(0f, 0f, -Mathf.Lerp(0f, 360f, counter));
        if(counter > 1 && !player.data.dead && player.data.view.IsMine) {
            NetworkingManager.RPC(typeof(MortalityTimer), nameof(Kill), player.playerID);
            if(player.data.stats.remainingRespawns > 0) {
                player.data.view.RPC("RPCA_Die_Phoenix", RpcTarget.All, Vector2.down * 1000);
            } else {
                player.data.view.RPC("RPCA_Die", RpcTarget.All, Vector2.down * 1000);
            }
        }
    }
    [UnboundRPC]
    public static void Kill(int playerId) {
        MortalityTimer mortality = PlayerManager.instance.players.Find(p => p.playerID == playerId).GetComponentInChildren<MortalityTimer>();
        var scy = Instantiate(mortality.Scythe, mortality.transform.position + (mortality.transform.up * mortality.transform.parent.localScale.y / 3), Quaternion.identity);
        scy.GetComponent<Animator>().enabled = true;
        Destroy(scy, 15);
        mortality.timer = 1.5f;
    }

    private IEnumerator Pause(IGameModeHandler gm) {
        Enabled = false;
        yield break;
    }

    private IEnumerator Enable(IGameModeHandler gm) {
        Enabled = true;
        yield break;
    }

    private IEnumerator Reset(IGameModeHandler gm) {
        try {
            Enabled = false;
            timer = 0;
            counter = 0;
            outerRing.fillAmount = counter;
            fill.fillAmount = counter;
            rotator.transform.localEulerAngles = new Vector3(0f, 0f, -Mathf.Lerp(0f, 360f, counter));
        } catch(Exception e) { RootCards.Debug(e); }
        yield break;
    }
}
