using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemigo : Entity
{
    [Header("Stats de enemigo")]
    public Nodo[] patrullaje;
    public List<Nodo> path;
    [SerializeField] public Color color;
    public Nodo baseDeEquipo;
    public int prueba;
    void Start()
    {
        hp = _hpMax;

        _fsm = new FSM();

        _fsm.CreateState("Patrol", new EnemigoPatrol(_fsm, patrullaje, _velocity, _maxVelocity, _maxForce, transform, _viewRadius, _viewAngle));
        _fsm.CreateState("Attack", new EnemigoAttack(_fsm, this, transform, _velocity, _maxVelocity, _maxForce, _viewRadius, _viewAngle, hp, _damage, _cooldown));
        _fsm.CreateState("Go to base", new EnemigoBackToBase(_fsm, transform, baseDeEquipo, path, _velocity, _maxVelocity, _maxForce, _viewRadius, _viewAngle));
        _fsm.CreateState("Healing", new EnemigoHealing(_fsm, this, _renderer, color));

        _fsm.ChangeState("Patrol");
    }

    // Update is called once per frame
    void Update()
    {
        _fsm.Execute();

        if (hp <= 0)
        {
            _fsm.ChangeState("Go to base");
            _renderer.material.color = Color.black;
        }

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
