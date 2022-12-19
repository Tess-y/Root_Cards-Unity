using System.Collections;
using UnboundLib.GameModes;
using UnityEngine;

public class HealthCurse : MonoBehaviour
{
    float _Health = 0;
    private Player _player;
    public void Start()
    {
        if (_player == null) _player = gameObject.GetComponent<Player>();
        GameModeManager.AddHook(GameModeHooks.HookPointEnd, gm => reset());
    }
    public void Cull(float HP)
    {
        _player.data.maxHealth += _Health;
        _Health = Mathf.Min(_Health + HP, _player.data.maxHealth);
        _player.data.maxHealth -= _Health;
    }
    public IEnumerator reset()
    {
        _player.data.maxHealth += _Health;
        _Health = 0;
        yield break;
    }
}
