using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemigoPatrol : IState
{
    FSM _fsm;

    Nodo[] _patrol;
    Vector3 _velocity;
    float _maxVelocity;
    float _maxForce;
    Transform _transform;
    int _currWaypoint = 0;

    float _viewRadius;
    float _viewAngle;

    public EnemigoPatrol(FSM fsm, Nodo[] patrol, Vector3 velocity, float maxVelocity, float maxForce, Transform transform, float viewRadius, float viewAngle)
    {
        _fsm = fsm;
        _patrol = patrol;
        _velocity = velocity;
        _maxVelocity = maxVelocity;
        _maxForce = maxForce;
        _transform = transform;
        _viewRadius = viewRadius;
        _viewAngle = viewAngle;
    }

    public void OnEnter()
    {
        
    }

    public void OnExit()
    {
        
    }

    public void OnUpdate()
    {
        AddForce(Seek(_patrol[_currWaypoint].transform.position));

        if (Vector3.Distance(_patrol[_currWaypoint].transform.position, _transform.position) <= 0.5f)
        {
            _currWaypoint++;

            if (_currWaypoint >= _patrol.Length)
                _currWaypoint = 0;
        }

        _transform.position += _velocity * Time.deltaTime;
        _transform.forward = _velocity;

        //if (InFOV(GameManager.Instance.pj.transform))
            //_fsm.ChangeState("");

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
