using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class JugadorGoPath : IState
{
    FSM _fsm;

    List<Nodo> _path;

    Jugador _pj;
    GameObject _target;
    Transform _transform;
    Vector3 _velocity;
    float _maxVelocity;
    float _maxForce;
    
    public JugadorGoPath(FSM fsm, List<Nodo> path, Transform transform, Vector3 velocity, float maxVelocity, float maxForce, Jugador pj, GameObject target)
    {
        _fsm = fsm;
        _path = path;
        _transform = transform;
        _velocity = velocity;
        _maxVelocity = maxVelocity;
        _maxForce = maxForce;
        _pj = pj;
        _target = target;
        
    }

    public void OnEnter()
    {
        _pj.SetPath(GameManager.Instance.CalculateThetaStar(GameManager.Instance.GetMinNode(_transform.position), GameManager.Instance.GetMinNode(_target.transform.position)));
    }

    public void OnExit()
    {
        
    }

    public void OnUpdate()
    {
        if (_path.Count == 0)
        {
            _fsm.ChangeState("Idle");

        }

        if (_path.Count > 0)
        {
            var dir = _path[0].transform.position - _transform.position;

            AddForce(Seek(_path[0].transform.position));

            if (GameManager.Instance.InLineOfSight(_transform.position, _path[0].transform.position) == false)
            {
                _pj.SetPath(GameManager.Instance.CalculateThetaStar(GameManager.Instance.GetMinNode(_transform.position), GameManager.Instance.GetMinNode(_target.transform.position)));
            }

            if (dir.magnitude <= 0.5f)
            {
                _path.RemoveAt(0);
            }
        }

        _transform.position += _velocity * Time.deltaTime;
        _transform.forward = _velocity;
        
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
