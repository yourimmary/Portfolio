using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SubMapSelectWindow : MonoBehaviour
{
    int _id;
    int _distance;
    Image _mapImg;
    TextMeshProUGUI _mapName;
    TextMeshProUGUI _mapDesc;

    public void InitSet(int idx)
    {
        _mapImg = transform.GetChild(0).GetComponent<Image>();
        _mapName = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        _mapDesc = transform.GetChild(2).GetComponent<TextMeshProUGUI>();

        _id = idx;

        //_mapImg.sprite = 
        _mapName.text = "Map" + idx;

        gameObject.SetActive(false);
    }

    public void SetDistance(int distance)
    {
        _distance = distance;
        _mapDesc.text = _distance + "M Forward";
    }

    public void ClickSelectMap()
    {
        GameManager.Instance.ActiveMap(_id, true, _distance);
        GameManager.Instance.GameStart();
        GetComponentInParent<MapSelectWindow>().CloseWindow();
    }
}
