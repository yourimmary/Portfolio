using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    [SerializeField] Transform _start;
    [SerializeField] Transform _target;

    Gride _grid;

    void Awake()
    {
        _grid = GetComponent<Gride>();
    }

    void Update()
    {
        FindPath(_start.position, _target.position);
    }

    void FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Node startNode = _grid.NodeFromWorldPoint(startPos);
        Node targetNode = _grid.NodeFromWorldPoint(targetPos);

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);

        bool isExit = false;
        while (openSet.Count > 0 || !isExit)
        {
            Node node = openSet[0];
            for (int n = 1; n < openSet.Count; n++)
            {
                if (openSet[n]._fCost <= node._fCost/* || openSet[n]._fCost == node._fCost*/)
                {
                    if (openSet[n]._hCost < node._hCost)
                        node = openSet[n];
                }
            }

            openSet.Remove(node);
            closedSet.Add(node);
            if (node == targetNode)
            {
                RetracePath(startNode, targetNode);
                isExit = true;
                break;
            }

            List<Node> neighbours = _grid.GetNeighbours(node);
            foreach (Node neighbour in neighbours/*_grid.GetNeighbours(node)*/)
            {
                if (!neighbour._walkable || closedSet.Contains(neighbour))
                    continue;

                int newCostToNeighbour = node._gCost + GetDistance(node, neighbour);
                if (newCostToNeighbour < neighbour._gCost || !openSet.Contains(neighbour))
                {
                    neighbour._gCost = newCostToNeighbour;
                    neighbour._hCost = GetDistance(neighbour, targetNode);
                    neighbour._parent = node;

                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                }
            }
        }
    }

    void RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;
        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode._parent;
        }
        path.Reverse();
        _grid._path = path;
    }

    int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA._gridX - nodeB._gridX);
        int dstY = Mathf.Abs(nodeA._gridY - nodeB._gridY);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }
}
