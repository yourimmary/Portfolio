using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteLayerSort : MonoBehaviour
{
    SpriteRenderer sr;

    private void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();

        sr.sortingOrder = 0;
    }

    private void Update()
    {
        if (GameManager.Instance.Player.PosY < transform.position.y)
            sr.sortingOrder = -1 * SetSortingOrder();
        else
            sr.sortingOrder = SetSortingOrder();
    }

    int SetSortingOrder()
    {
        int order = 0;

        if (transform.CompareTag("Monster"))
            order = 1;
        else if (transform.CompareTag("Wall"))
            order = 2;

        return order;
    }
}
