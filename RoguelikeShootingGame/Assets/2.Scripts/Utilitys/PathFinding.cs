using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineEnum;

public class PathFinding : MonoBehaviour
{
    //[SerializeField] Transform _start;
    //[SerializeField] Transform _target;

    //bool _isMove = false;

    Gride _grid;

    public List<Node> _path { get; set; }

    //void Awake()
    //{
    //    //_grid = GetComponent<Gride>();
    //    GetGrid(GetComponent<Gride>());
    //}

    //void Update()
    //{
    //    FindPath(_start.position, _target.position);
    //}
    
    public void GetGrid(Gride grid)
    {
        _grid = grid;
        _grid.SetInit();
    }

    public void FindPath(Vector3 startPos, Vector3 targetPos)
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

    public void MoveObject(float speed)
    {
        if (_path.Count != 0)
        {
            if (Vector3.Distance(_path[0]._worldPosition, transform.position) > 0.1f)
            {
                Vector3 dir = (_path[0]._worldPosition - transform.position).normalized;
                Debug.Log(dir);
                transform.position += speed * Time.deltaTime * dir;
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
        _path = path;
        string pathStr = string.Empty;
        foreach (Node node in _path)
        {
            pathStr += node._worldPosition + "\n";
        }
        Debug.Log(pathStr);
    }

    int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA._gridX - nodeB._gridX);
        int dstY = Mathf.Abs(nodeA._gridY - nodeB._gridY);

        //if (dstX > dstY)
        //    return 14 * dstY + 10 * (dstX - dstY);
        //return 14 * dstX + 10 * (dstY - dstX);
        return 10 * dstX + 10 * dstY;
    }
}
