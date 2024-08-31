using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSelectWindow : MonoBehaviour
{
    [SerializeField] GameObject _subWindow;

    List<SubMapSelectWindow> _subWndList;
    int _selectCount = 0;

    public void InitWindow()
    {
        _subWndList = new List<SubMapSelectWindow>();
    }

    public void OpenWindow()
    {
        _selectCount = Random.Range(2, 6);

        List<int> _mapIndexs = new List<int>();
        int cnt = 0;

        while (cnt < _selectCount)
        {
            int idx = Random.Range(1, 6);
            if (!_mapIndexs.Contains(idx))
            {
                _mapIndexs.Add(idx);
                cnt++;
            }
        }

        for (int i = 0; i < _selectCount; i++)
        {
            SubMapSelectWindow smw =
                Instantiate(_subWindow, transform.GetChild(0).GetChild(0)).GetComponent<SubMapSelectWindow>();
            smw.InitSet(_mapIndexs[i], GameManager.Instance.SetDistance());
            _subWndList.Add(smw);
        }

        gameObject.SetActive(true);
    }

    public void CloseWindow()
    {
        for (int i = 0; i < _subWndList.Count; i++)
            Destroy(_subWndList[i].gameObject);
        _subWndList.Clear();
        gameObject.SetActive(false);
    }
}
