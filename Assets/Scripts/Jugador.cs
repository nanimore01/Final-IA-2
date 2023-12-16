using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Jugador : Entity
{
    [Header("Stats de jugador")]
    [SerializeField] LayerMask _lm;
    public List<Nodo> path;
    public GameObject target;

    void Start()
    {
        _fsm = new FSM();

        _fsm.CreateState("Walk", new JugadorWalk(_fsm, target, _velocity, _maxVelocity, _maxForce, transform));
        _fsm.CreateState("Idle", new JugadorIdle(_fsm));
        _fsm.CreateState("Go to path", new JugadorGoPath(_fsm, path, transform, _velocity, _maxVelocity, _maxForce, this, target));

        _fsm.ChangeState("Idle");
    }

    // Update is called once per frame
    void Update()
    {
        _fsm.Execute();

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, _lm))
            {
                
                target.transform.position = hit.point;
                target.SetActive(true);

                if(GameManager.Instance.InLineOfSight(transform.position, target.transform.position))
                {
                    _fsm.ChangeState("Walk");
                }
                else
                {
                    
                    _fsm.ChangeState("Go to path");
                }
            }
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
