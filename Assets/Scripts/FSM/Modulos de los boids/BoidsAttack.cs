using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BoidsAttack : IState
{
    FSM _fsm;

    Boid _me;

    Transform _transform;

    List<Boid> _enemyTeam;
    Vector3 _velocity;
    float _maxVelocity;
    float _maxForce;
    float _viewRadius;
    float _viewAngle;
    float _vida;
    int _dmg;
    float _cooldownTime;
    float _currCooldown;
    public BoidsAttack(FSM fsm, Transform transform, Boid me, Vector3 velocity, float maxVelocity, float maxForce, float viewRadius, float viewAngle, float vida, float cooldownTime, int dmg, List<Boid> enemyTeam)
    {
        _fsm = fsm;
        _transform = transform;
        _me = me;
        _velocity = velocity;
        _maxVelocity = maxVelocity;
        _maxForce = maxForce;
        _viewRadius = viewRadius;
        _viewAngle = viewAngle;
        _vida = vida;
        _cooldownTime = cooldownTime;
        _dmg = dmg;
        _enemyTeam = enemyTeam;
    }

    public void OnEnter()
    {
        
    }

    public void OnExit()
    {
        
    }

    public void OnUpdate()
    {
        if (InFOV(_me.enemys.transform) == false)
        {
            _fsm.ChangeState("Follow leader");
        }   
        if (Vector3.Distance(_transform.position,_me.enemys.transform.position) <= 0.5f)
        {
            _currCooldown += Time.deltaTime;

            if(_currCooldown > _cooldownTime)
            {
                _currCooldown = 0;
                _me.enemys.GetComponent<Boid>().TakeDamage(_dmg);
            }
            
        }
        else
        {
            AddForce(Seek(_me.enemys.transform.position));

            _transform.position += _velocity * Time.deltaTime;
            _transform.forward = _velocity;
        }
        
        if(_me.enemys.GetComponent<Boid>().hp <= 0)
        {
            foreach (Boid boid in _enemyTeam)
            {
                if (InFOV(boid.transform) && boid.hp > 0)
                {
                    _me.SetTarget(boid.transform);
                }
            }
        }

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

    void AddForce(Vector3 dir)
    {
        _velocity += dir;

        _velocity = Vector3.ClampMagnitude(_velocity, _maxVelocity);
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
