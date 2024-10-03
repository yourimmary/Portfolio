using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SubMapSelectWindow : MonoBehaviour
{
    [SerializeField] bool _isExtra;

    int _id;
    int _distance;
    Image _mapImg;
    TextMeshProUGUI _mapName;
    TextMeshProUGUI _mapDesc;

    public void InitSet(int idx)
    {
        if (_isExtra)
        {
            _mapImg = transform.GetChild(0).GetChild(1).GetChild(0).GetChild(0).GetComponent<Image>();
            _mapName = transform.GetChild(0).GetChild(1).GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();
            _mapDesc = transform.GetChild(0).GetChild(1).GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>();

            _id = idx + 1;

            //_mapImg.sprite = 
            _mapName.text = "Map" + _id;
        }
        else
        {
            _mapImg = transform.GetChild(0).GetComponent<Image>();
            _mapName = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            _mapDesc = transform.GetChild(2).GetComponent<TextMeshProUGUI>();

            _id = idx + 1;

            //_mapImg.sprite = 
            _mapName.text = "Map" + _id;

            //gameObject.SetActive(false);
        }
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
