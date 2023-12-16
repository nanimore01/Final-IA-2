using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JugadorIdle : IState
{
    FSM _fsm;

    public JugadorIdle(FSM fsm)
    {
        _fsm = fsm;
    }

    public void OnEnter()
    {
        Debug.Log("Idle");
    }

    public void OnExit()
    {
        
    }

    public void OnUpdate()
    {
        
    }
}
