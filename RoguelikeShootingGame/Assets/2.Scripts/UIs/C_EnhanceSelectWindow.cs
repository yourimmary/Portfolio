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

    [Header("µî±Þ È®·ü")]
    [SerializeField] int _goldRate = 10;
    [SerializeField] int _silverRate = 30;

    PlayerController _player;

    ENHANCEGRADE RandomEnhanceGrade()
    {
        int randomWeight = Random.Range(1, 101);

        if (randomWeight <= _goldRate)
            return ENHANCEGRADE.GOLD;
        else if (randomWeight <= _silverRate)
            return ENHANCEGRADE.SILVER;
        else
            return ENHANCEGRADE.BRONZE;
    }

    public void InitSet(PlayerController pc)
    {
        _subEnhanceType = new List<ENHANCETYPE>();
        _subEnhance = new List<SubEnhanceWindow>();
        for (int i = 0; i < _subCount; i++)
        {
            GameObject go = Instantiate(_subWindow, transform.GetChild(0).GetChild(0));
            _subEnhance.Add(go.GetComponent<SubEnhanceWindow>());
        }

        _player = pc;
        gameObject.SetActive(false);
    }

    public void OpenWindow()
    {
        Destroy(GameManager.Instance.MapParent.GetChild(0).gameObject);

        while (_subEnhanceType.Count < _subCount)
        {
            ENHANCETYPE type = (ENHANCETYPE)Random.Range(0, (int)ENHANCETYPE.MAX);
            if (!_subEnhanceType.Contains(type)) _subEnhanceType.Add(type);
        }

        int n = 0;
        foreach (SubEnhanceWindow i in _subEnhance)
        {
            ENHANCEGRADE eg = RandomEnhanceGrade();
            i.InitSet(_subEnhanceType[n++], eg);
        }
        gameObject.SetActive(true);
    }

    public void CloseWindow(ENHANCETYPE type, float value)
    {
        _player.IncreasePlayerStatus(type, value);
        _subEnhanceType.Clear();
        gameObject.SetActive(false);
    }
}
