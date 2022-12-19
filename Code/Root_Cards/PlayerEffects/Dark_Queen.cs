using UnityEngine;
using UnboundLib;

public class Dark_Queen : MonoBehaviour
{
    public GameObject SoulStone;
    private GameObject _SoulStone;
    private float timer = 0;
    void Awake()
    {
        GetComponentInParent<Gravity>().enabled = false;
        GetComponentInParent<PlayerVelocity>().enabled = false;
        Transform gun = GetComponentInParent<WeaponHandler>().gun.transform;
        gun.Find("Spring/Handle").gameObject.SetActive(false);
        gun.Find("Spring/Barrel").gameObject.SetActive(false);
        gun.Find("Spring/Ammo/Canvas").gameObject.SetActive(false);
        _SoulStone = Instantiate(SoulStone,gun);
    }

    void OnDestroy(){
        GetComponentInParent<Gravity>().enabled = true;
        GetComponentInParent<PlayerVelocity>().enabled = true;
        Transform gun = GetComponentInParent<WeaponHandler>().gun.transform;
        gun.Find("Spring/Handle").gameObject.SetActive(true);
        gun.Find("Spring/Barrel").gameObject.SetActive(true);
        gun.Find("Spring/Ammo/Canvas").gameObject.SetActive(true);
        Destroy(_SoulStone);
    }

    void Update(){
        if(timer > 0){
            timer -= TimeHandler.deltaTime;
            if(timer <= 0)_SoulStone.SetActive(true);
        }
    }

    public void Telleport()
    {
        if(timer<=0){
            Vector3 vector = MainCam.instance.cam.ScreenToWorldPoint(Input.mousePosition);
            Player player = GetComponentInParent<Player>();
            vector.z = player.transform.position.z;
            if (Physics2D.OverlapCircle(vector, 0.5f) || !player.data.view.IsMine) return;
            player.GetComponentInParent<PlayerCollision>().IgnoreWallForFrames(2);
            player.transform.position = vector;
            RootCards.instance.ExecuteAfterFrames(2, ()=>player.data.healthHandler.CallTakeDamage(Vector2.up * player.data.maxHealth*0.34f, vector, null, player,true));
            _SoulStone.SetActive(false);
            timer = 2.5f;
        }

    }
}
