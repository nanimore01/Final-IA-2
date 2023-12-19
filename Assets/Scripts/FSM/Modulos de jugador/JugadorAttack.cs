using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class JugadorAttack : IState
{
    FSM _fsm;

    Jugador _me;

    Transform _transform;


    Vector3 _velocity;
    float _maxVelocity;
    float _maxForce;
    float _viewRadius;
    float _viewAngle;
    
    int _dmg;
    float _cooldownTime;
    float _currCooldown;

    public JugadorAttack(FSM fsm, Jugador me, Transform transform, Vector3 velocity, float maxVelocity, float maxForce, float viewRadius, float viewAngle, int dmg, float cooldownTime)
    {
        _fsm = fsm;
        _me = me;
        _transform = transform;
        _velocity = velocity;
        _maxVelocity = maxVelocity;
        _maxForce = maxForce;
        _viewRadius = viewRadius;
        _viewAngle = viewAngle;
        _dmg = dmg;
        _cooldownTime = cooldownTime;
        
    }

    public void OnEnter()
    {
        Debug.Log("A atacar");
    }

    public void OnExit()
    {

    }

    public void OnUpdate()
    {

        if (Vector3.Distance(_transform.position, GameManager.Instance.enemigo.transform.position) <= 0.5f)
        {
            _currCooldown += Time.deltaTime;

            if (_currCooldown > _cooldownTime)
            {
                _currCooldown = 0;
                GameManager.Instance.enemigo.TakeDamage(_dmg);
            }

        }
        else
        {
            AddForce(Seek(GameManager.Instance.enemigo.transform.position));

            _transform.position += _velocity * Time.deltaTime;
            _transform.forward = _velocity;
        }

        if (GameManager.Instance.enemigo.hp <= 0 || !InFOV(GameManager.Instance.enemigo.transform))
        {
            _fsm.ChangeState("Idle");
        }


    }
    void AddForce(Vector3 dir)
    {
        _velocity += dir;

        _velocity = Vector3.ClampMagnitude(_velocity, _maxVelocity);
    }
    Vector3 Seek(Vector3 dir)
    {
        var desired = dir - _transform.position;
        desired.Normalize();
        desired *= _maxVelocity;

        var steering = desired - _velocity;
        steering = Vector3.ClampMagnitude(steering, _maxForce);

        return steering;
    }
    public bool InFOV(Transform obj)
    {
        var dir = obj.position - _transform.position;

        if (dir.magnitude < _viewRadius)
        {
            if (Vector3.Angle(_transform.forward, dir) <= _viewAngle * 0.5f)
            {
                return GameManager.Instance.InLineOfSight(_transform.position, obj.position);
            }
        }

        return false;
    }
}
