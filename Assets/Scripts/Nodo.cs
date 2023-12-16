using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nodo : MonoBehaviour
{
    public List<Nodo> nodosVecinos = new List<Nodo>();
    public int cost = 1;

    void Start()
    {
        GameManager.Instance.nodos.Add(this);

        
    }

    public void Update()
    {
        //ConstruirCamino();
    }

    public void ConstruirCamino()
    {
        nodosVecinos.Clear();

        foreach(Nodo item in GameManager.Instance.nodos) 
        {
            if (item == this) continue;

            if(GameManager.Instance.InLineOfSight(transform.position, item.transform.position))
            {
                nodosVecinos.Add(item);
            }
        }
    }

}
