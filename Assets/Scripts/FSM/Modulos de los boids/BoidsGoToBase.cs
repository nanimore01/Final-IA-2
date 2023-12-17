using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class BoidsGoToBase : IState
{
    FSM _fsm;
    Transform _transform;
    Boid _me;
    Nodo _base;
    List<Nodo> _path;
    Vector3 _velocity;
    float _maxVelocity;
    float _maxForce;
    float _viewRadius;
    float _viewAngle;
    List<Boid> _enemyTeam;

    public BoidsGoToBase(FSM fsm, Transform transform, Nodo wbase, List<Nodo> path, Vector3 velocity, float maxVelocity, float maxForce, Boid me, List<Boid> enemyTeam)
    {
        _fsm = fsm;
        _transform = transform;
        _base = wbase;
        _path = path;
        _velocity = velocity;
        _maxVelocity = maxVelocity;
        _maxForce = maxForce;
        _me = me;
        _enemyTeam = enemyTeam;
    }

    public void OnEnter()
    {
        SetPath(GameManager.Instance.CalculateThetaStar(GameManager.Instance.GetMinNode(_transform.position), _base ));
    }

    public void OnExit()
    {
        
    }

    public void OnUpdate()
    {
        if (_path.Count > 0)
        {
            var dir = _path[0].transform.position - _transform.position;

            AddForce(Seek(_path[0].transform.position));

            if (dir.magnitude <= 0.5f)
            {
                _path.RemoveAt(0);
            }
        }

        _transform.position += _velocity * Time.deltaTime;
        _transform.forward = _velocity;

        if( _path.Count == 0 )
        {
            _fsm.ChangeState("Healing");
        }

        foreach (Boid boid in _enemyTeam)
        {
            if (InFOV(boid.transform))
            {
                _me.SetTarget(boid.transform);
                _fsm.ChangeState("Escape");
            }
        }

    }

    public void SetPath(List<Nodo> newPath)
    {

        _path.Clear();

        foreach (var item in newPath)
        {

            _path.Add(item);
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
