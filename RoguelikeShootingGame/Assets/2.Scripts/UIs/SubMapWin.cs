using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SubMapWin : MonoBehaviour
{
    [SerializeField] Sprite _enterSprite;
    [SerializeField] Sprite _exitSprite;

    Image _subMapImg;
    TextMeshProUGUI _mapName;
    TextMeshProUGUI _mapDesc;
    TextMeshProUGUI _mapDis;

    int _distance;
    int _listIdx;
    int _mapId;
    bool _isActive;

    void CreateMap()
    {
        GameObject mapPrefab = Resources.Load("Maps/Map" + _mapId) as GameObject;

        if (mapPrefab != null)
        {
            Map mapCs = Instantiate(mapPrefab, GameManager.Instance.MapParent).GetComponent<Map>();
            mapCs.InitMap(_mapName.text, _distance);
            mapCs.CreateMonster();
        }
    }

    public void SetInit(int dis, int idx, int id)
    {
        _distance = dis;
        _listIdx = idx;
        _mapId = id;
        _isActive = true;

        _subMapImg = transform.GetChild(0).GetComponent<Image>();

        _mapName = transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        _mapDesc = transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        _mapDis = transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();

        string obName = "Map" + _mapId;

        _mapName.text = MapInfoText.GetMap(obName).GetName();
        _mapDesc.text = MapInfoText.GetMap(obName).GetDesc();
        _mapDis.text = _distance.ToString() + "M";
    }

    public void ActiveFalse()
    {
        _isActive = false;
    }

    public void SubMapEnterMouse()
    {
        if (_isActive)
        {
            _subMapImg.sprite = _enterSprite;

            transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
            transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
        }
    }

    public void SubMapExitMouse()
    {
        if (_isActive)
        {
            _subMapImg.sprite = _exitSprite;

            transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
            transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
        }
    }

    public void SubMapClick()
    {
        if (_isActive)
        {
            SoundManager.Instance.sfxPlay(DefineEnum.SOUNDENUM.SubMapClick);

            MapSelectWin parentWnd = GetComponentInParent<MapSelectWin>();

            parentWnd.ChangeValue(transform.GetComponent<RectTransform>().anchoredPosition);
            parentWnd.SetActiveFalseMap(_listIdx);

            CreateMap();

            GameManager.Instance.GameStart();

            parentWnd.gameObject.SetActive(false);
        }
    }
}
