using ModdingUtils.MonoBehaviours;
using UnityEngine;

internal class WitchTime : ReversibleEffect
{
    public float time_remaning;
    private bool running;
    public override void OnStart()
    {
        base.OnStart();
        this.characterStatModifiersModifier.movementSpeed_mult = 2;
        this.gravityModifier.gravityForce_mult = 4;
        this.characterStatModifiersModifier.jump_mult = 1.6f;
        this.gunStatModifier.projectielSimulatonSpeed_mult = 2;
        this.gunAmmoStatModifier.reloadTimeMultiplier_mult = .5f;
        this.gunStatModifier.attackSpeed_mult = 2;
        this.blockModifier.cdMultiplier_mult = 2;
        this.blockModifier.cdAdd_mult = 2;
        this.time_remaning = player.data.stats.GetRootData().witchTimeDuration;
        TimeHandler.instance.baseTimeScale /= 2;
        ApplyModifiers();
        this.running = true;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (running)
        {
            RootCards.Debug(time_remaning);
            if (time_remaning < 0 )this.Destroy();
            time_remaning -= Time.deltaTime;
        }
    }

    public override void OnOnDisable()
    {
        base.OnOnDisable();
        this.Destroy();
    }

    public override void OnOnDestroy()
    {
        base.OnOnDestroy();
        TimeHandler.instance.baseTimeScale = UnityEngine.Mathf.Clamp(TimeHandler.instance.baseTimeScale*2,0,0.85f);                                                                                                                                                
    }
}