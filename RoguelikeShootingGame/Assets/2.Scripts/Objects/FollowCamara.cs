using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamara : MonoBehaviour
{
    Vector3 _offset;

    public void Init()
    {
        _offset = transform.position;
    }

    public void Follow(Vector3 pos)
    {
        transform.position = pos + _offset;
    }
}
