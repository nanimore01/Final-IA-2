using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    protected FSM _fsm;

    [Header("Vida")]
    [SerializeField]public int hp;
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
    [SerializeField] protected int _damage;
    [SerializeField] protected float _cooldown;
    
    public void TakeDamage(int damage)
    {
        hp -= damage;
        StartCoroutine(CambioDeColores());
    }

    public IEnumerator CambioDeColores()
    {
        _renderer.material.color = Color.white;
        yield return new WaitForSeconds(0.1f);
        _renderer.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        _renderer.material.color = Color.white;
    }
}
