using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SubMapSelectWindow : MonoBehaviour
{
    int _id;
    int _distance;
    TextMeshProUGUI _mapName;
    TextMeshProUGUI _mapDesc;
    TextMeshProUGUI _mapDis;

    public void InitSet(int idx)
    {
        _mapName = transform.GetChild(0).GetChild(1).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        _mapDesc = transform.GetChild(0).GetChild(1).GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();
        _mapDis = transform.GetChild(0).GetChild(1).GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>();

        _id = idx + 1;

        _mapName.text = MapInfoText.GetMap("Map" + _id).GetName();
        _mapDesc.text = MapInfoText.GetMap("Map" + _id).GetDesc();
    }

    public void SetDistance(int distance)
    {
        _distance = distance;
        _mapDis.text = _distance + "M Forward";
    }

    public void ClickSelectMap()
    {
        string path = "Maps/Map" + _id;
        GameObject mapObject = Resources.Load(path) as GameObject;
        if (mapObject != null)
        {
            Map mapCs = Instantiate(mapObject, GameManager.Instance.MapParent).GetComponent<Map>();
            mapCs.InitMap(_mapName.text, _distance);
            mapCs.CreateMonster();
        }

        GameManager.Instance.GameStart();
        GetComponentInParent<MapSelectWindow>().CloseWindow();
    }
}
