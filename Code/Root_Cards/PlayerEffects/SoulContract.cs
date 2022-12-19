using System.Linq;
using UnityEngine;

internal class SoulContract : DealtDamageEffect
{
    public override void DealtDamage(Vector2 damage, bool selfDamage, Player damagedPlayer = null)
    {
        if (selfDamage)
        {
            foreach(Player player in PlayerManager.instance.players.Where(p => p.teamID != damagedPlayer.playerID && Vector3.Distance(damagedPlayer.transform.position, p.transform.position) < (20 * damagedPlayer.transform.localScale.z)))
            {
                Vector2 newDamage = damage / (1+(Vector3.Distance(damagedPlayer.transform.position, player.transform.position)/20));
                if (damagedPlayer.data.view.IsMine)
                {
                    player.data.healthHandler.CallTakeDamage(newDamage, damagedPlayer.transform.position);
                }
                Instantiate(RootCards.Assets.LoadAsset<GameObject>("Soul"), player.transform.position, Quaternion.Euler(0, 0, 0)).AddComponent<SoulReturn>().SetPlayerAndHealth(damagedPlayer, newDamage.magnitude);
            }
        }
    }
    internal class SoulReturn : MonoBehaviour
    {
        float health;
        Player player;
        float timer = 0;

        internal void SetPlayerAndHealth(Player player, float health)
        {
            this.player = player;
            this.health = health;
        }
        public void Update()
        {
            timer += TimeHandler.deltaTime;
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, player.transform.position, TimeHandler.deltaTime/(2-timer));
            if(timer > 2.1f)
            { 
                player.data.healthHandler.Heal(health);
                Destroy(gameObject);
            }
        }
    }
}
