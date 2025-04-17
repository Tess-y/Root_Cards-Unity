using RootCore;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace RootStones {
    public class TeleportStone : MonoBehaviour
	{
		public ParticleSystem[] parts;
	
		public ParticleSystem[] remainParts;
	
		public float distance = 10f;

		public float softCooldown;

		public float hardCooldown;

		public float delay;

		internal int delayStacks;

		internal float cooldownTimer;
	
		public LayerMask mask;
	
		private CharacterData data;
	
		private AttackLevel level;

		private float GetHardCooldown() {
			return data.player.HasCard("Gauntlet")? softCooldown : hardCooldown;
		}
	
		private void Start()
		{
			parts = GetComponentsInChildren<ParticleSystem>();
			data = GetComponentInParent<CharacterData>();
			level = GetComponentInParent<AttackLevel>();
			Block componentInParent = GetComponentInParent<Block>();
            componentInParent.SuperFirstBlockAction += Go;
			cooldownTimer = GetHardCooldown();
			delayStacks = 0;
			transform.parent.Find("Limbs/ArmStuff/ShieldStone/Canvas/Image").localScale *= 1.2f;
            transform.parent.Find("Limbs/ArmStuff/ShieldStone/Canvas/Image").GetComponent<Image>().color = Colour.New(0.3, 0.6,1);
        }

		private void Update() {
			if (cooldownTimer < GetHardCooldown()) {
				cooldownTimer += Time.deltaTime;
			} else {
				delayStacks = 0;
                transform.parent.Find("Limbs/ArmStuff/ShieldStone/Canvas/Image").GetComponent<Image>().color = Colour.New(0.3, 0.6, 1);
            }
		}
	
		private void OnDestroy()
		{
			Block componentInParent = GetComponentInParent<Block>();
			componentInParent.SuperFirstBlockAction -= Go;
			transform.parent.Find("Limbs/ArmStuff/ShieldStone/Canvas/Image").GetComponent<Image>().color = Colour.New(0.8, 0.8, 0.8);
            transform.parent.Find("Limbs/ArmStuff/ShieldStone/Canvas/Image").localScale /= 1.2f;
        }
	
		public void Go(BlockTrigger.BlockTriggerType triggerType)
		{
			if(cooldownTimer < softCooldown) return;
			StartCoroutine(DelayMove(triggerType, base.transform.position));
			if(cooldownTimer < GetHardCooldown()) {
				delayStacks++;
			}
			cooldownTimer = -delay * delayStacks;
            transform.parent.Find("Limbs/ArmStuff/ShieldStone/Canvas/Image").GetComponent<Image>().color = Colour.New(0.8, 0.8, 0.8);
        }
	
		private IEnumerator DelayMove(BlockTrigger.BlockTriggerType triggerType, Vector3 beforePos)
		{
			if(!(triggerType == BlockTrigger.BlockTriggerType.Default || triggerType == BlockTrigger.BlockTriggerType.Empower)) yield break;
			List<Player> players = PlayerManager.instance.players.Where(p=> p.teamID != data.player.teamID && !p.data.dead).OrderBy(p => Vector2.Distance(transform.position,p.transform.position)).ToList();
			if(!data.player.HasCard("Gauntlet")) {
				players = new List<Player>() { players[0] };
			}
			foreach(Player player in players) {
				if(triggerType == BlockTrigger.BlockTriggerType.Empower) {
					yield return new WaitForSeconds(0f);
				}
				Vector3 position = player.transform.position;
				Vector3 position2 = player.transform.position;

				if(triggerType == BlockTrigger.BlockTriggerType.Empower) {
					position2 = beforePos;
				} else {
					int num = (int)distance * level.attackLevel;
					float num2 = distance * (float)level.attackLevel / (float)num;
					for(int i = 0; i < num; i++) {
						position += num2 * data.aimDirection;
						if(!Physics2D.OverlapCircle(position, 0.5f)) {
							position2 = position;
						}
					}
				}
				for(int j = 0; j < remainParts.Length; j++) {
					remainParts[j].transform.position = player.transform.root.position;
					remainParts[j].Play();
				}
                player.GetComponent<PlayerCollision>().IgnoreWallForFrames(2);
                player.transform.root.position = position2;
				for(int k = 0; k < parts.Length; k++) {
					parts[k].transform.position = position2;
					parts[k].Play();
				}
                player.data.playerVel.velocity = Vector2.zero;
                player.data.sinceGrounded = 0f;
			}
		}
	}
}
