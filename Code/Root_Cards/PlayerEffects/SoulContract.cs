using System.Linq;
using UnityEngine;

internal class SoulContract: DealtDamageEffect {
    float Cooldown = 0;
    float storedDamage;
    public override void DealtDamage(Vector2 damage, bool selfDamage, Player damagedPlayer = null) {
        if(selfDamage) {
            if(Cooldown>0) {
                storedDamage+=damage.magnitude;
            } else {
                SoulDrain(damage, damagedPlayer);
                Cooldown=0.3f;
            }
        }
    }

    void Update() {
        if(Cooldown>0) {
            Cooldown-=Time.deltaTime;
            if(Cooldown<=0) {
                if(storedDamage>0)
                    SoulDrain(Vector2.up*storedDamage, GetComponentInParent<Player>());
                storedDamage=0;
            }
        }
    }

    private void SoulDrain(Vector2 damage, Player damagedPlayer) {
        foreach(Player player in PlayerManager.instance.players.Where(p => p.teamID!=damagedPlayer.playerID&&Vector3.Distance(damagedPlayer.transform.position, p.transform.position)<(14*damagedPlayer.transform.localScale.z))) {
            Vector2 newDamage = damage*(1-(Vector3.Distance(damagedPlayer.transform.position, player.transform.position)/(14*damagedPlayer.transform.localScale.z)));
            if(damagedPlayer.data.view.IsMine) {
                player.data.healthHandler.CallTakeDamage(newDamage, damagedPlayer.transform.position);
            }
            Instantiate(RootCards.Assets.LoadAsset<GameObject>("Soul"), player.transform.position, Quaternion.Euler(0, 0, 0)).AddComponent<SoulReturn>().SetPlayerAndHealth(damagedPlayer, newDamage.magnitude);
        }
    }

    internal class SoulReturn: MonoBehaviour {
        float health;
        Player player;
        float timer = 0;
        float duration;

        internal void SetPlayerAndHealth(Player player, float health) {
            this.player=player;
            this.health=health;
            transform.localScale=Vector3.one*UnityEngine.Mathf.Clamp(health/100, 0.25f, 5f);
            duration=Vector3.Distance(transform.position, player.transform.position)/7;
        }
        public void Update() {
            timer+=TimeHandler.deltaTime;
            gameObject.transform.position=Vector3.Lerp(gameObject.transform.position, player.transform.position, TimeHandler.deltaTime/(duration-timer));
            if(timer>duration+0.1f) {
                player.data.healthHandler.Heal(health);
                Destroy(gameObject);
            }
        }
    }
}
