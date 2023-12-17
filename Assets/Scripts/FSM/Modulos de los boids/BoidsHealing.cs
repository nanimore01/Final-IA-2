using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidsHealing : IState
{
    FSM _fsm;

    Boid _me;

    Renderer _renderer;
    Color _color;

    float _currCooldown;

    public BoidsHealing(FSM fsm, Boid me, Renderer renderer, Color color)
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

        if(_currCooldown > 10f)
        {
            _fsm.ChangeState("Follow leader");
        }
    }
}
