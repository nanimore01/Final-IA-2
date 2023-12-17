using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BoidsEscape : IState
{
    FSM _fsm;

    Boid _me;

    Transform _transform;
    Vector3 _velocity;
    float _maxVelocity;
    float _maxForce;
    float _viewRadius;
    float _viewAngle;
    Renderer _renderer;
    
    public BoidsEscape(FSM fsm, Boid me, Transform transform, Vector3 velocity, float maxVelocity, float maxForce, float viewRadius, float viewAngle, Renderer renderer)
    {
        _fsm = fsm;
        _me = me;
        _transform = transform;
        _velocity = velocity;
        _maxVelocity = maxVelocity;
        _maxForce = maxForce;
        _viewRadius = viewRadius;
        _viewAngle = viewAngle;
        _renderer = renderer;

    }

    public void OnEnter()
    {
        _renderer.material.color = Color.black;
    }

    public void OnExit()
    {
        
    }

    public void OnUpdate()
    {
        AddForce(Flee(_me.enemys.transform.position));

        _transform.position += _velocity * Time.deltaTime;
        _transform.forward = _velocity;

        if (InFOV(_me.enemys.transform) == false)
            _fsm.ChangeState("Go to base");

        

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

    Vector3 Flee(Vector3 dir)
    {
        return -Seek(dir);
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
