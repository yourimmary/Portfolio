using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSelectWindow : MonoBehaviour
{
    [SerializeField] GameObject _subWindow;

    List<SubMapSelectWindow> _subWndList;
    /// <summary>
    /// 플레이어가 선택한 맵을 저장하는 리스트
    /// </summary>
    List<int> _selectedMapList;
    List<int> _mapIndexs;

    int _selectCount = 0;

    public void InitWindow()
    {
        _subWndList = new List<SubMapSelectWindow>();
        _mapIndexs = new List<int>();

        for (int i = 1; i <= 10; i++)
        {
            SubMapSelectWindow smw = 
                Instantiate(_subWindow, transform.GetChild(0).GetChild(0)).GetComponent<SubMapSelectWindow>();
            smw.InitSet(i);
            _subWndList.Add(smw);
        }
    }

    public void OpenWindow()
    {
        _selectCount = Random.Range(2, 6);

        int cnt = 0;

        while (cnt < _selectCount)
        {
            int idx = Random.Range(0, 10);
            if (!_mapIndexs.Contains(idx))
            {
                _mapIndexs.Add(idx);
                cnt++;
            }
        }

        for (int i = 0; i < _selectCount; i++)
        {
            _subWndList[_mapIndexs[i]].gameObject.SetActive(true);
            _subWndList[_mapIndexs[i]].SetDistance(GameManager.Instance.SetDistance());
        }

        gameObject.SetActive(true);
    }

    public void CloseWindow()
    {
        for (int i = 0; i < _mapIndexs.Count; i++)
            _subWndList[_mapIndexs[i]].gameObject.SetActive(false);
        _mapIndexs.Clear();
        gameObject.SetActive(false);
    }
}
