using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BoidsGetPath : IState
{
    FSM _fsm;

    Transform _leader;
    Vector3 _velocity;
    float _maxVelocity;
    float _maxForce;
    Transform _transform;
    List<Nodo> _path;

    public BoidsGetPath(FSM fsm, Transform leader, Vector3 velocity, float maxVelocity, float maxForce, Transform transform, List<Nodo> path)
    {
        _fsm = fsm;
        _leader = leader;
        _velocity = velocity;
        _maxVelocity = maxVelocity;
        _maxForce = maxForce;
        _transform = transform;
        _path = path;
    }

    public void OnEnter()
    {
        SetPath(GameManager.Instance.CalculateThetaStar(GameManager.Instance.GetMinNode(_transform.position), GameManager.Instance.GetMinNode(_leader.position)));
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

            if (GameManager.Instance.InLineOfSight(_transform.position, _path[0].transform.position) == false)
            {
                SetPath(GameManager.Instance.CalculateThetaStar(GameManager.Instance.GetMinNode(_transform.position), GameManager.Instance.GetMinNode(_leader.position)));
            }

            if (dir.magnitude <= 0.5f)
            {
                _path.RemoveAt(0);
            }
        }

        _transform.position += _velocity * Time.deltaTime;
        _transform.forward = _velocity;

        if (_path.Count == 0)
        {
            _fsm.ChangeState("Follow leader");
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
    public void SetPath(List<Nodo> newPath)
    {
        _path.Clear();
        foreach (var item in newPath)
        {
            _path.Add(item);
        }
    }
}
