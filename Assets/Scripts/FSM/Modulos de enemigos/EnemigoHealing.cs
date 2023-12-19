using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemigoHealing : IState
{
    FSM _fsm;

    Enemigo _me;

    Renderer _renderer;
    Color _color;

    float _currCooldown;

    public EnemigoHealing(FSM fsm, Enemigo me, Renderer renderer, Color color)
    {
        _fsm = fsm;
        _me = me;
        _renderer = renderer;
        _color = color;

    }

    public void OnEnter()
    {

    }

    public void OnExit()
    {
        _renderer.material.color = _color;
        _me.TakeDamage(-10);
    }

    public void OnUpdate()
    {
        _currCooldown += Time.deltaTime;

        if (_currCooldown > 10f)
        {
            _fsm.ChangeState("Patrol");
        }
    }
}


