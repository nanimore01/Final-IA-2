using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidsFollowLeader : IState
{
    FSM _fsm;
    Boid _me;
    
    List<Boid> _boids;
    Transform _leader;
    Vector3 _velocity;
    float _maxVelocity;
    float _maxForce;
    Transform _transform;
    float _viewRadius;
    float _viewAngle;
    float _separationRadius;

    public BoidsFollowLeader(FSM fsm, Boid me, List<Boid> boids, Transform leader, Vector3 velocity, float maxVelocity, float maxForce, Transform transform, float viewRadius, float viewAngle, float separationRadius)
    {
        _fsm = fsm;
        _me = me;
        _boids = boids;
        _leader = leader;
        _velocity = velocity;
        _maxVelocity = maxVelocity;
        _maxForce = maxForce;
        _transform = transform;
        _viewRadius = viewRadius;
        _viewAngle = viewAngle;
        _separationRadius = separationRadius;
    }

    public void OnEnter()
    {
        
    }

    public void OnExit()
    {
        
    }

    public void OnUpdate()
    {
        LeaderFollowing();

        foreach(Boid boid in GameManager.Instance.minionsEnemigos)
        {
            if (InFOV(boid.transform))
                _me.SetTarget(boid.transform);
                _fsm.ChangeState("Attack");
        }
    }

    void LeaderFollowing()
    {
        AddForce(Arrive(_leader.position));
        AddForce(Separation(_boids, _separationRadius));

        _transform.position += _velocity * Time.deltaTime;
        _transform.forward = _velocity;
    }

    void AddForce(Vector3 dir)
    {
        _velocity += dir;

        _velocity = Vector3.ClampMagnitude(_velocity, _maxVelocity);
    }
    Vector3 Arrive(Vector3 dir)
    {
        float dist = Vector3.Distance(_transform.position, dir);

        if (dist > _viewRadius)
            return Seek(dir);

        var desired = dir - _transform.position;
        desired.Normalize();
        desired *= (_maxVelocity * (dist / _viewRadius));

        var steering = desired - _velocity;
        steering = Vector3.ClampMagnitude(steering, _maxForce);

        return steering;
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

    Vector3 Separation(List<Boid> boids, float radius)
    {
        Vector3 desired = Vector3.zero;

        foreach (var item in boids)
        {
            var dir = item.transform.position - _transform.position;
            if (dir.magnitude > radius || item == _me)
                continue;

            desired -= dir;
        }

        if (desired == Vector3.zero)
            return desired;

        desired.Normalize();
        desired *= _maxVelocity;

        return CalculateSteering(desired);
    }

    Vector3 CalculateSteering(Vector3 desired)
    {
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
