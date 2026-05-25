using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetItemWindow : MonoBehaviour
{
    int childIndex = 0;

    Transform _groupLayout;

    public void SetInit()
    {
        _groupLayout = transform.GetChild(0).GetChild(0);
    }

    public void CreateItemIcon(int count)
    {
        GameObject prefab = Resources.Load<GameObject>("UIs/ItemIcon");

        for (int i = 0; i < count; i++)
        {
            Instantiate(prefab, _groupLayout);
        }
    }

    public void ResetChildIndex()
    {
        childIndex = 0;
    }

    public void FindJamChangeUI()
    {
        Image jamImg = _groupLayout.GetChild(childIndex).GetComponentInChildren<Image>();
        jamImg.color = Color.white;
        childIndex++;
    }
}
