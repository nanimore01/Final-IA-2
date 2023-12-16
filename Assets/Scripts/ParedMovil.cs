using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ParedMovil : MonoBehaviour
{
    public GameObject[] movimiento;
    int _currWaypoint = 0;
    Vector3 _velocity;
    [SerializeField]float _maxVelocity;
    [SerializeField]float _maxForce;
    public void Update()
    {
        AddForce(Seek(movimiento[_currWaypoint].transform.position));

        if (Vector3.Distance(movimiento[_currWaypoint].transform.position, transform.position) <= 0.5f)
        {
            _currWaypoint++;

            if (_currWaypoint >= movimiento.Length)
                _currWaypoint = 0;
        }
        transform.position += _velocity * Time.deltaTime;
        transform.forward = _velocity;

    }

    Vector3 Seek(Vector3 dir)
    {
        var desired = dir - transform.position;
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
