using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gride : MonoBehaviour
{
    [SerializeField] LayerMask _unwalkabbleMask;
    [SerializeField] Vector2 _gridWorldSize;
    [SerializeField] float _nodeRidius;

    Node[,] _grid;

    float _nodeDiameter;
    int _gridSizeX, _gridSizeY;
    int[,] _neighbourNodeIndex = { { -1, 0 }, { 1, 0 }, { 0, -1 }, { 0, 1 } };

    //public List<Node> _path { get; set; }


    //void Awake()
    //{
    //    //_nodeDiameter = _nodeRidius * 2;
    //    //_gridSizeX = Mathf.RoundToInt(_gridWorldSize.x / _nodeDiameter);
    //    //_gridSizeY = Mathf.RoundToInt(_gridWorldSize.y / _nodeDiameter);

    //    //CreateGrid();

    //    SetInit();
    //}

    public void SetInit()
    {
        _nodeDiameter = _nodeRidius * 2;
        _gridSizeX = Mathf.RoundToInt(_gridWorldSize.x / _nodeDiameter);
        _gridSizeY = Mathf.RoundToInt(_gridWorldSize.y / _nodeDiameter);

        CreateGrid();
    }

    void CreateGrid()
    {
        _grid = new Node[_gridSizeX, _gridSizeY];
        //Vector3 worldBottomLeft = transform.position - (Vector3.right * _gridWorldSize.x / 2)
        //                                             - (Vector3.forward * _gridWorldSize.y / 2);
        Vector3 worldBottomLeft = transform.position - (Vector3.right * _gridWorldSize.x / 2)
                                                     - (Vector3.up * _gridWorldSize.y / 2);

        for (int x = 0; x < _gridSizeX; x++)
        {
            for (int y = 0; y < _gridSizeY; y++)
            {
                //Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * _nodeDiameter + _nodeRidius)
                //                                + Vector3.forward * (y * _nodeDiameter + _nodeRidius);
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * _nodeDiameter + _nodeRidius)
                                                + Vector3.up * (y * _nodeDiameter + _nodeRidius);
                //bool isWalk = !(Physics.CheckSphere(worldPoint, _nodeRidius, _unwalkabbleMask));
                bool isWalk;
                Collider2D collider = Physics2D.OverlapCircle(worldPoint, _nodeRidius, _unwalkabbleMask);
                if (collider == null)
                    isWalk = true;
                else
                {
                    if (collider.CompareTag("Wall") || collider.CompareTag("Water"))
                        isWalk = false;
                    else
                        isWalk = true;
                }
                _grid[x, y] = new Node(isWalk, worldPoint, x, y);
            }
        }
    }

    public List<Node> GetNeighbours(Node stdNode)
    {
        List<Node> neighbours = new List<Node>();
        //for (int x = -1; x <= 1; x++)
        //{
        //    for (int y = -1; y <= 1; y++)
        //    {
        //        if (x == 0 && y == 0)
        //            continue;

        //        int checkX = stdNode._gridX + x;
        //        int checkY = stdNode._gridY + y;

        //        if (checkX >= 0 && checkX < _gridSizeX && checkY >= 0 && checkY < _gridSizeY)
        //        {
        //            neighbours.Add(_grid[checkX, checkY]);
        //        }
        //    }
        //}

        for (int n = 0; n < 4; n++)
        {
            int x = stdNode._gridX + _neighbourNodeIndex[n, 0];
            int y = stdNode._gridY + _neighbourNodeIndex[n, 1];

            if (x >= 0 && x < _gridSizeX && y >= 0 && y < _gridSizeY)
                neighbours.Add(_grid[x, y]);
        }

        return neighbours;
    }

    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x + _gridWorldSize.x / 2) / _gridWorldSize.x;
        //float percentY = (worldPosition.z + _gridWorldSize.y / 2) / _gridWorldSize.y;
        float percentY = (worldPosition.y + _gridWorldSize.y / 2) / _gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((_gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((_gridSizeY - 1) * percentY);

        return _grid[x, y];
    }

    void OnDrawGizmos()
    {
        //Gizmos.DrawWireCube(transform.position, new Vector3(_gridWorldSize.x, 1, _gridWorldSize.y));
        Gizmos.DrawWireCube(transform.position, new Vector3(_gridWorldSize.x, _gridWorldSize.y, 1));
        if (_grid != null)
        {
            foreach (Node node in _grid)
            {
                Gizmos.color = (node._walkable) ? Color.white : Color.red;
                //if (_path != null)
                //{
                //    if (_path.Contains(node))
                //        Gizmos.color = Color.black;
                //}
                Gizmos.DrawCube(node._worldPosition, Vector3.one * (_nodeDiameter - 0.05f));
            }
        }
    }
}
