using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindingItem : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.Instance.CanvasUI.GetComponentInChildren<GetItemWindow>().FindJamChangeUI();
            Destroy(gameObject);
        }
    }
}
