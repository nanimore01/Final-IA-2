using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Boid : Entity
{
    [Header("Stats de boid")]
    [SerializeField] int _grupo;
    [SerializeField]List<Boid> _team = new List<Boid>();
    [SerializeField]List<Boid> _enemyTeam = new List<Boid>();
    Transform _leader;
    public Nodo baseDeEquipo;
    [SerializeField]public Color color;
    public Transform enemys;
    [SerializeField] float _separationRadius;
    public List<Nodo> path;

    void Start()
    {
        _hp = _hpMax;
        switch(_grupo)
        {
            case 0:
                GameManager.Instance.minionsAliados.Add(this);
                _renderer.material.color = Color.green;
                _leader = GameManager.Instance.pj.transform;
                _team = GameManager.Instance.minionsAliados;
                _enemyTeam = GameManager.Instance.minionsEnemigos;

                break;
            case 1:
                GameManager.Instance.minionsEnemigos.Add(this);
                _renderer.material.color = Color.red;
                _leader = GameManager.Instance.enemigo.transform;
                _team = GameManager.Instance.minionsEnemigos;
                _enemyTeam = GameManager.Instance.minionsAliados;
                
                break;
        }

        _fsm = new FSM();

        _fsm.CreateState("Follow leader", new BoidsFollowLeader(_fsm, this, _team, _leader, _velocity, _maxVelocity, _maxForce, transform, _viewRadius, _viewAngle, _separationRadius, _enemyTeam));
        _fsm.CreateState("Attack", new BoidsAttack(_fsm, transform, this, _velocity, _maxVelocity, _maxForce, _viewRadius, _viewAngle, _hp, _cooldown));
        _fsm.CreateState("Go to base", new BoidsGoToBase(_fsm, transform, baseDeEquipo, path, _velocity, _maxVelocity, _maxForce, this, _enemyTeam));
        _fsm.CreateState("Escape", new BoidsEscape(_fsm, this, transform, _velocity, _maxVelocity, _maxForce, _viewRadius, _viewAngle, _renderer));
        _fsm.CreateState("Healing", new BoidsHealing(_fsm, this, _renderer, color));
        

        _fsm.ChangeState("Follow leader");
    }

    
    void Update()
    {
        _fsm.Execute();
    }

    public void SetTarget(Transform target)
    {
        enemys = target;
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
