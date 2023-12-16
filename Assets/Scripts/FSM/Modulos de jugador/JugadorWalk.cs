using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class JugadorWalk : IState
{
    FSM _fsm;

    
    GameObject _target;
    Transform _transform;
    Vector3 _velocity;
    float _maxVelocity;
    float _maxForce;

    public JugadorWalk(FSM fsm, GameObject target, Vector3 velocity, float maxVelocity, float maxForce, Transform transform)
    {
        _fsm = fsm;
        _target = target;
        _velocity = velocity;
        _maxVelocity = maxVelocity;
        _maxForce = maxForce;
        _transform = transform;
        
    }

    public void OnEnter()
    {
        
    }

    public void OnExit()
    {
    }

    public void OnUpdate()
    {
        AddForce(Seek(_target.transform.position));

        _transform.position += _velocity * Time.deltaTime;
        _transform.forward = _velocity;

        if(GameManager.Instance.InLineOfSight(_transform.position, _target.transform.position) == false) 
        {
            _fsm.ChangeState("Go to path");
        }

        if(Vector3.Distance(_transform.position, _target.transform.position) <= 0.5f)
        {
            _target.SetActive(false);
            _fsm.ChangeState("Idle");
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

}
