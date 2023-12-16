using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemigo : Entity
{
    [Header("Stats de enemigo")]
    public Nodo[] patrullaje;
    public List<Nodo> path;

    void Start()
    {
        _fsm = new FSM();

        _fsm.CreateState("Patrol", new EnemigoPatrol(_fsm, patrullaje, _velocity, _maxVelocity, _maxForce, transform, _viewRadius, _viewAngle));
        

        _fsm.ChangeState("Patrol");
    }

    // Update is called once per frame
    void Update()
    {
        _fsm.Execute();
    }
    public void SetPath(List<Nodo> newPath)
    {

        path.Clear();

        foreach (var item in newPath)
        {
            print("Agrego item a la lista");
            path.Add(item);
        }

        if (newPath == null)
        {
            _fsm.ChangeState("Idle");
        }
    }
}
