using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DefineEnum;

public class SubEnhanceWindow : MonoBehaviour
{
    float _value;
    ENHANCETYPE _enhanceType;
    Image _enhanceIcon;
    TextMeshProUGUI _enhanceName;
    TextMeshProUGUI _enhanceDesc;

    public void InitSet(ENHANCETYPE type, float value)
    {
        _enhanceIcon = transform.GetChild(0).GetComponent<Image>();
        _enhanceName = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        _enhanceDesc = transform.GetChild(2).GetComponent<TextMeshProUGUI>();

        _enhanceType = type;
        _value = value;

        _enhanceName.text = EnhanceUtility.GetEnhanceText(_enhanceType);
        _enhanceDesc.text = EnhanceUtility.DescribeEnhanceString(_enhanceType, _value);
    }
    
    public void ClickSelectEnhance()
    {
        if (_enhanceType == ENHANCETYPE.ADDITIONALATTACK)
            EnhanceUtility.IncreaseAdditionalAttackUp(_value);
        GameManager.Instance.GameReady();
        GetComponentInParent<C_EnhanceSelectWindow>().CloseWindow(_enhanceType, _value);
    }
}
