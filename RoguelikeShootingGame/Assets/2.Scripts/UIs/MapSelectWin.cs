using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum ContentChild
{
    Line,
    SubMap
}

public class MapSelectWin : MonoBehaviour
{
    [SerializeField] RectTransform _distanceTrans;
    [SerializeField] Transform _subMapParent;

    [SerializeField] GameObject _subPrefab;
    [SerializeField] GameObject _connectLine;

    RectTransform _contentTrans;
    CanvasScaler _canvasScaler;

    Vector2 _distancePos;
    Vector2 _standPos;

    int[] _xStartPos = { -175, -350, -525, -700 };

    List<GameObject> MapList;
    List<int> MapIdList;

    int SelectCnt = 0;
    float _contentWidth = 0;

    int SetDistance()
    {
        int dis = Random.Range(50, 121);

        if (1000 - GameManager.Instance.CurDepth < dis)
            dis = 1000 - GameManager.Instance.CurDepth;

        return dis;
    }

    void SetMapIdList()
    {
        int Max = Resources.LoadAll<GameObject>("Maps").Length + 1;

        while (MapIdList.Count < SelectCnt)
        {
            int id = Random.Range(1, Max);

            if (!MapIdList.Contains(id)) MapIdList.Add(id);
        }
    }

    public void SetInit()
    {
        MapList = new List<GameObject>();
        MapIdList = new List<int>();

        _contentTrans = transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<RectTransform>();

        _distancePos = _distanceTrans.anchoredPosition;
        _standPos = new Vector2(0, 15);

        _canvasScaler = GameObject.Find("Canvas").GetComponent<CanvasScaler>();
        _contentWidth = (_canvasScaler.referenceResolution.x < 1700) ? 1700 : _canvasScaler.referenceResolution.x;
    }

    public void CreateMapSelectUI()
    {
        if (GameManager.Instance.CurDepth < 1000)
        {
            int TotalAdd = 0;
            int idx = 0;

            SelectCnt = Random.Range(2, 6);
            SetMapIdList();

            for (int n = 0; n < SelectCnt; n++)
            {
                //subMapUI 생성
                GameObject subMap = Instantiate(_subPrefab, _contentTrans.GetChild((int)ContentChild.SubMap));

                int dis = SetDistance();
                TotalAdd += dis;

                RectTransform subMapTrans = subMap.GetComponent<RectTransform>();
                subMapTrans.anchoredPosition = new Vector2(_standPos.x + _xStartPos[SelectCnt - 2] + (350 * n), (dis + GameManager.Instance.CurDepth) * 10);

                TextMeshProUGUI disTxt = subMap.GetComponentInChildren<TextMeshProUGUI>();
                disTxt.text = dis.ToString();

                SubMapWin mapFunc = subMap.GetComponent<SubMapWin>();
                mapFunc.SetInit(dis, n, MapIdList[idx]);
                idx++;

                //Line 생성
                GameObject line = Instantiate(_connectLine, _contentTrans.GetChild((int)ContentChild.Line));
                line.GetComponent<RectTransform>().anchoredPosition = _standPos;

                RectTransform lineTrans = line.transform.GetChild(0).GetComponent<RectTransform>();

                Vector2 subVec = subMap.GetComponent<RectTransform>().anchoredPosition - _standPos;
                lineTrans.sizeDelta = new Vector2(subVec.magnitude, lineTrans.sizeDelta.y);

                float lineAngle = Vector2.Angle(Vector2.right, subVec.normalized);

                line.GetComponent<RectTransform>().Rotate(Vector3.forward, lineAngle);

                MapList.Add(subMap);
            }

            //content 세로, 가로 길이
            float Hight = (float)TotalAdd / SelectCnt;
            float sizeDeltaY = _standPos.y + Hight * 2 * 10 + 100;

            if (_standPos != new Vector2(0, 15))
            {
                if (_contentWidth / 2 - Mathf.Abs(_standPos.x) < _canvasScaler.referenceResolution.x / 2)
                    _contentWidth += (-_xStartPos[SelectCnt - 2] + 200) * 2;
            }

            _contentTrans.sizeDelta = new Vector2(_contentWidth, (sizeDeltaY > 11000) ? 11000 : sizeDeltaY);

            //카메라 위치
            float CameraY = (_contentTrans.sizeDelta.y / 2) - (Hight * 10 + _standPos.y);
            _contentTrans.anchoredPosition = new Vector2(-_standPos.x, CameraY);
        }
    }

    public void PrintContentPosition()
    {
        Vector2 xAxis = _distancePos - _contentTrans.anchoredPosition;
        _distanceTrans.anchoredPosition = new Vector2(xAxis.x, 0);
    }

    public void SetActiveFalseMap(int idx)
    {
        for (int n = 0; n < MapList.Count; n++)
        {
            MapList[n].GetComponentInChildren<SubMapWin>().ActiveFalse();
            
            if (n != idx)
            {
                MapList[n].GetComponentInChildren<Image>().color = Color.gray;
            }
        }

        MapList.Clear();
        MapIdList.Clear();
    }

    public void ChangeValue(Vector2 pos)
    {
        _standPos = pos;
    }
}
