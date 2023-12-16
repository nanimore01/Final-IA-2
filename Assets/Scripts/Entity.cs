using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    protected FSM _fsm;

    [Header("Vida")]
    [SerializeField]protected int _hp;
    [SerializeField]protected int _hpMax;
    [SerializeField]protected Renderer _renderer;
    [Header("Movimiento")]
    protected Vector3 _velocity;
    [SerializeField] protected float _maxVelocity;
    [SerializeField] protected float _maxForce;
    [Header("Vision")]
    [SerializeField] protected float _viewRadius;
    [SerializeField] protected float _viewAngle;
    [Header("Daño")]
    [SerializeField] protected int damage;
    [SerializeField] protected int Cooldown;
    
    public void TakeDamage(int damage)
    {
        _hp -= damage;
    }
}
