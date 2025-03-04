using ModdingUtils.MonoBehaviours;
using RootCore;
using UnityEngine;

namespace RootAdvancedCards {
    internal class AntiJump: ReversibleEffect {
	    private bool pressed = false;
	    private bool fliped = false;
	    private float grounded = .25f;
	    private float lastY = 0f;
	
	    public override void OnStart() {
	        base.OnStart();
	        SetLivesToEffect(int.MaxValue);
	        gravityModifier.gravityForce_mult=-1;
	        gunStatModifier.gravity_mult=-1;
	    }
	
	
	    public override void OnUpdate() {
	        //RootCards.Debug(lastY + "," + player.gameObject.transform.position.y + "(" + grounded + ")");
	        if(System.Math.Round(lastY, 1)==System.Math.Round(player.gameObject.transform.position.y, 1)) {
	            grounded-=Time.deltaTime;
	        } else
	            grounded=.5f;
	
	        if(grounded<=0) {
	            Core.Debug("auto flipping!");
	            if(fliped) {
	                ClearModifiers();
	            } else {
	                ApplyModifiers();
	            }
	            grounded=.5f;
	            fliped=!fliped;
	            player.data.jump.Jump(forceJump: true, 2f);
	        }
	        if(base.data.input.jumpIsPressed) {
	            if(!pressed) {
	                if(fliped) {
	                    ClearModifiers();
	                } else {
	                    ApplyModifiers();
	                }
	                fliped=!fliped;
	                player.data.jump.Jump(forceJump: true, 1f);
	            }
	            pressed=true;
	        } else {
	            pressed=false;
	        }
	
	        lastY=player.gameObject.transform.position.y;
	    }
	}
}
