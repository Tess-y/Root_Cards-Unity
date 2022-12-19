using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnboundLib;
using UnboundLib.GameModes;
using UnityEngine;
using UnityEngine.UI.ProceduralImage;

internal class LoopedTime : MonoBehaviour
{
    private float timer = 0;
    private bool stored = false;
    private Vector3 playerPos;
    private float playerHealth;
    private int playerAmmo;
    private bool playerIsRelaoding;
    private float playerReloadCounter;
    private float playerFreeReloadCounter;
    private float playerBlockCounter;
    public Player player;
    public GunAmmo gunAmmo;
    public Gun gun;
    public Block block;



    [Range(0f, 1f)]
    public float counter;

    public float timeToFill = 5f;

    public float timeToEmpty = 0f;

    public float duration = 1;

    public ProceduralImage outerRing;

    public ProceduralImage fill;

    public Transform rotator;

    public Transform still;

    public GameObject pentagram;
    
    private GameObject _pentagram;

    public bool Enabled = false;



    public void Awake()
    {
        player = GetComponentInParent<Player>();
        gun = player.data.weaponHandler.gun;
        gunAmmo = gun.GetComponentInChildren<GunAmmo>();
        block = GetComponentInParent<Block>();
    }

    public void Start()
    {
        GameModeManager.AddHook(GameModeHooks.HookPointEnd, Reset);
        GameModeManager.AddHook(GameModeHooks.HookPointStart, Enable);
    }
    private IEnumerator Enable(IGameModeHandler gm)
    {
        Enabled = true;
        yield break;
    }

    private IEnumerator Reset(IGameModeHandler gm)
    {
        try
        {
            Enabled = false;
            timer = 0;
            outerRing.color = new Color32(0, 255, 0, 255);
            Destroy(_pentagram);
            stored = false;
        }
        catch(Exception e) { RootCards.Debug(e); }
        yield break;
    }

    public void Update()
    {
        if (Enabled && player != null && player.data.view.IsMine)
        {
            timer += Time.deltaTime;
            counter = timer / 5;
            this.outerRing.fillAmount = this.counter;
            this.fill.fillAmount = this.counter;
            this.rotator.transform.localEulerAngles = new Vector3(0f, 0f, -Mathf.Lerp(0f, 360f, this.counter));

            if (timer > 1.5 && timer < 2)
            {
                if (outerRing.color.r == 0)
                {
                    outerRing.color = new Color32(255, 150, 0, 255);
                    rotator.gameObject.GetComponentInChildren<ProceduralImage>().color = outerRing.color;
                }
                else
                {
                    outerRing.color = new Color32(150, 255, 0, 255);
                    rotator.gameObject.GetComponentInChildren<ProceduralImage>().color = outerRing.color;
                }
            }
            else if (!stored && timer >= 2)
            {
                outerRing.color = new Color32(255, 0, 0, 255);
                rotator.gameObject.GetComponentInChildren<ProceduralImage>().color = outerRing.color;
                playerPos = player.transform.position;
                playerHealth = player.data.health;
                playerAmmo = (int)gunAmmo.GetFieldValue("currentAmmo");
                playerIsRelaoding = gun.isReloading;
                playerReloadCounter = (float)gunAmmo.GetFieldValue("reloadCounter");
                playerFreeReloadCounter = (float)gunAmmo.GetFieldValue("freeReloadCounter");
                playerBlockCounter = block.counter;
                /*
                if(gunAmmo == null) gunAmmo = gun.GetComponentInChildren<GunAmmo>();
                playerAmmo = (int)gunAmmo.GetType().GetField("currentAmmo", System.Reflection.BindingFlags.NonPublic
| System.Reflection.BindingFlags.Instance).GetValue(gunAmmo);*/
                stored = true;

                _pentagram = Instantiate(pentagram, playerPos, Quaternion.identity);
                _pentagram.GetOrAddComponent<Canvas>().sortingOrder = 10000;
                _pentagram.transform.localScale = Vector3.one * 0.1f;
                RootCards.instance.StartCoroutine(GameModeManager.TriggerHook("TimeLoop-" + player.playerID + "-Save"));
            }
            else if (stored && timer <= 5)
            {
                _pentagram.transform.localScale = Vector3.one * ((1-((timer - 2) / 3)) * 0.15f);
            }
            else if (timer >= 5)
            {
                outerRing.color = new Color32(0, 255, 0, 255);
                rotator.gameObject.GetComponentInChildren<ProceduralImage>().color = outerRing.color;
                timer = 0;
                stored = false;
                block.counter = playerBlockCounter;

                player.GetComponentInParent<PlayerCollision>().IgnoreWallForFrames(2);
                player.transform.position = playerPos;
                player.data.healthHandler.Heal(playerHealth - player.data.health);



                ///Gun ammo shit

                gunAmmo.SetFieldValue("currentAmmo", playerAmmo);
                gun.isReloading = playerIsRelaoding;
                gunAmmo.SetFieldValue("reloadCounter", playerReloadCounter);
                gunAmmo.SetFieldValue("freeReloadCounter", playerFreeReloadCounter);
                for (int num = gunAmmo.populate.transform.childCount - 1; num >= 0; num--)
                {
                    if (gunAmmo.populate.transform.GetChild(num).gameObject.activeSelf)
                    {
                        Destroy(gunAmmo.populate.transform.GetChild(num).gameObject);
                    }
                }

                gunAmmo.populate.times = (int)gunAmmo.GetFieldValue("currentAmmo");
                gunAmmo.populate.DoPopulate();
                typeof(GunAmmo).InvokeMember("SetActiveBullets", BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.NonPublic, null, gunAmmo, new object[1]
                {
                    true
                });

                ///END
                /*
                gunAmmo.GetType().GetField("currentAmmo", System.Reflection.BindingFlags.NonPublic
| System.Reflection.BindingFlags.Instance).SetValue(gunAmmo,playerAmmo);*/
                RootCards.instance.StartCoroutine(GameModeManager.TriggerHook("TimeLoop-" + player.playerID + "-Load"));
                Destroy(_pentagram);
            } 
        }
        else{
            this.outerRing.fillAmount = 0;
            this.fill.fillAmount = 0;
        }
    }
}
