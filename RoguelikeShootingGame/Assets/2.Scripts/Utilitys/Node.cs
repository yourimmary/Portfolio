using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public bool _walkable { get; set; }
    public Vector3 _worldPosition { get; set; }

    public int _gridX { get; set; }
    public int _gridY { get; set; }
    public Node _parent { get; set; }

    public int _gCost { get; set; }
    public int _hCost { get; set; }
    public int _fCost
    {
        get { return _gCost + _hCost; }
    }


    public Node(bool isWalk, Vector3 worldPos, int gridX, int gridY)
    {
        _walkable = isWalk;
        _worldPosition = worldPos;
        _gridX = gridX;
        _gridY = gridY;
    }
}

