using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : Entity
{
    [Header("Stats de boid")]
    [SerializeField] int _grupo;
    List<Boid> _team = new List<Boid>();
    Transform _leader, _enemys;
    [SerializeField] float _separationRadius;
    public List<Nodo> path;

    void Start()
    {
        switch(_grupo)
        {
            case 0:
                _renderer.material.color = Color.green;
                _leader = GameManager.Instance.pj.transform;
                _team = GameManager.Instance.minionsAliados;
                GameManager.Instance.minionsAliados.Add(this);
                break;
            case 1:
                _renderer.material.color = Color.red;
                _leader = GameManager.Instance.enemigo.transform;
                _team = GameManager.Instance.minionsEnemigos;
                GameManager.Instance.minionsEnemigos.Add(this);
                break;
        }

        _fsm = new FSM();

        _fsm.CreateState("Follow leader", new BoidsFollowLeader(_fsm, this, _team, _leader, _velocity, _maxVelocity, _maxForce, transform, _viewRadius, _viewAngle, _separationRadius));
        _fsm.CreateState("Attack", new BoidsAttack(_fsm, transform, _enemys, _velocity, _maxVelocity, _maxForce, _viewRadius, _viewAngle, _hp));


        _fsm.ChangeState("Follow leader");
    }

    
    void Update()
    {
        _fsm.Execute();
    }

    public void SetTarget(Transform target)
    {
        _enemys = target;
    }
    public void SetPath(List<Nodo> newPath)
    {

        path.Clear();

        foreach (var item in newPath)
        {
            print("Agrego item a la lista");
            path.Add(item);
        }

        
    }
}
