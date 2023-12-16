using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Jugador pj;
    
    public Enemigo enemigo;

    public List<Boid> minionsAliados;
    public List<Boid> minionsEnemigos;

    public List<Nodo> nodos = new List<Nodo>();
    [SerializeField] LayerMask _maskWall;
    public void Awake()
    {
        if (Instance == null)
            Instance = this;

        
    }

    public void Start()
    {
        
    }

    public void Update()
    {
        
        ConfigurarCamino();
            
            
        

    }

    public void ConfigurarCamino()
    {
        

        foreach (Nodo nodo in nodos)
        {
            nodo.nodosVecinos.Clear();
            foreach(Nodo nodo1 in nodos)
            {
                if (nodo1 == nodo) continue;
                
                if (InLineOfSight(nodo.transform.position, nodo1.transform.position))
                {
                    nodo.nodosVecinos.Add(nodo1);
                }
            }
            
        }
    }
    public bool InLineOfSight(Vector3 start, Vector3 end)
    {
        var dir = end - start;

        return !Physics.Raycast(start, dir, dir.magnitude, _maskWall);
    }

    public Nodo GetMinNode(Vector3 position)
    {
        print("Funciono");
        Nodo minNode = null;
        float minDist = Mathf.Infinity;

        for (int i = 0; i < nodos.Count; i++)
        {
            if (InLineOfSight(nodos[i].transform.position, position))
            {
                if (Vector3.Distance(nodos[i].transform.position, position) < minDist)
                {

                    minNode = nodos[i];
                    minDist = Vector3.Distance(nodos[i].transform.position, position);

                }
            }
        }

        return minNode;
    }

    public List<Nodo> CalculateAStar(Nodo startingNode, Nodo goalNode)
    {
        PriorityQueue<Nodo> frontier = new PriorityQueue<Nodo>();
        frontier.Enqueue(startingNode, 0);

        Dictionary<Nodo, Nodo> cameFrom = new Dictionary<Nodo, Nodo>();
        cameFrom.Add(startingNode, null);

        Dictionary<Nodo, int> costSoFar = new Dictionary<Nodo, int>();
        costSoFar.Add(startingNode, 0);

        while (frontier.Count > 0)
        {
            Nodo current = frontier.Dequeue();

            if (current == goalNode)
            {
                List<Nodo> path = new List<Nodo>();

                while (current != startingNode)
                {
                    path.Add(current);
                    current = cameFrom[current];
                }

                path.Reverse();
                return path;
            }

            foreach (var item in current.nodosVecinos)
            {
                

                int newCost = costSoFar[current] + item.cost;
                float priority = newCost + Vector3.Distance(item.transform.position, goalNode.transform.position);

                if (!costSoFar.ContainsKey(item))
                {
                    if (!frontier.ContainsKey(item))
                        frontier.Enqueue(item, priority);
                    cameFrom.Add(item, current);
                    costSoFar.Add(item, newCost);
                }
                else if (costSoFar[item] > newCost)
                {
                    if (!frontier.ContainsKey(item))
                        frontier.Enqueue(item, priority);
                    cameFrom[item] = current;
                    costSoFar[item] = newCost;
                }
            }
        }
        return new List<Nodo>();
    }

    public List<Nodo> CalculateThetaStar(Nodo startingNode, Nodo goalNode)
    {
        var listNode = CalculateAStar(startingNode, goalNode);

        int current = 0;

        while (current + 2 < listNode.Count)
        {
            if (InLineOfSight(listNode[current].transform.position, listNode[current + 2].transform.position))
            {
                listNode.RemoveAt(current + 1);
            }
            else
                current++;
        }

        return listNode;
    }

}
