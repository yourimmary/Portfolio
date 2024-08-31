using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineEnum;

public class C_EnhanceSelectWindow : MonoBehaviour
{
    const int _subCount = 3;
    [SerializeField] GameObject _subWindow;

    List<ENHANCETYPE> _subEnhanceType;
    List<SubEnhanceWindow> _subEnhance;

    public void InitSet()
    {
        _subEnhanceType = new List<ENHANCETYPE>();
        _subEnhance = new List<SubEnhanceWindow>();
        for (int i = 0; i < _subCount; i++)
        {
            GameObject go = Instantiate(_subWindow, transform.GetChild(0).GetChild(0));
            _subEnhance.Add(go.GetComponent<SubEnhanceWindow>());
        }
        gameObject.SetActive(false);
    }

    public void OpenWindow()
    {
        while (_subEnhanceType.Count < _subCount)
        {
            ENHANCETYPE type = (ENHANCETYPE)Random.Range(0, (int)ENHANCETYPE.MAX);
            if (!_subEnhanceType.Contains(type)) _subEnhanceType.Add(type);
        }

        int n = 0;
        foreach (SubEnhanceWindow i in _subEnhance)
            i.InitSet(_subEnhanceType[n++], 10);
        gameObject.SetActive(true);
    }

    public void CloseWindow()
    {
        _subEnhanceType.Clear();
        gameObject.SetActive(false);
    }
}
