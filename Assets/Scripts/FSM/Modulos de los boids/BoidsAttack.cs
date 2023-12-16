using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BoidsAttack : IState
{
    FSM _fsm;

    Transform _transform, _target;
    
    Vector3 _velocity;
    float _maxVelocity;
    float _maxForce;
    float _viewRadius;
    float _viewAngle;
    float _vida;

    public BoidsAttack(FSM fsm, Transform transform, Transform target, Vector3 velocity, float maxVelocity, float maxForce, float viewRadius, float viewAngle, float vida)
    {
        _fsm = fsm;
        _transform = transform;
        _target = target;
        _velocity = velocity;
        _maxVelocity = maxVelocity;
        _maxForce = maxForce;
        _viewRadius = viewRadius;
        _viewAngle = viewAngle;
        _vida = vida;
    }

    public void OnEnter()
    {
        
    }

    public void OnExit()
    {
        
    }

    public void OnUpdate()
    {
        AddForce(Seek(_target.position));

        _transform.position += _velocity * Time.deltaTime;
        _transform.forward = _velocity;

        if (InFOV(_target) == false)
            _fsm.ChangeState("Follow leader");

        if (_vida > 0)
            _fsm.ChangeState("");
            
        
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
