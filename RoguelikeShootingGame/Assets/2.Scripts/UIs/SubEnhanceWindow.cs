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
    ENHANCEGRADE _grade;
    Image _enhanceIcon;
    TextMeshProUGUI _enhanceName;
    TextMeshProUGUI _enhanceDesc;

    [SerializeField] Sprite[] _iconImgs;

    float ValueOfGrade(ENHANCEGRADE grade)
    {
        float val = 0;

        switch (grade)
        {
            case ENHANCEGRADE.GOLD:
                val = 30;
                break;
            case ENHANCEGRADE.SILVER:
                val = 20;
                break;
            case ENHANCEGRADE.BRONZE:
                val = 10;
                break;
        }

        return val;
    }

    void IconSpriteOfType(ENHANCETYPE type)
    {
        switch (type)
        {
            case ENHANCETYPE.ATTUP:
            case ENHANCETYPE.ATTUPPERCENT:
            case ENHANCETYPE.CRITICALDAMAGEUP:
                _enhanceIcon.sprite = _iconImgs[0];
                break;
            case ENHANCETYPE.DEFUP:
            case ENHANCETYPE.DEFUPPERCENT:
                _enhanceIcon.sprite = _iconImgs[1];
                break;
            case ENHANCETYPE.HPUP:
            case ENHANCETYPE.HPUPPERCENT:
                _enhanceIcon.sprite = _iconImgs[2];
                break;
            case ENHANCETYPE.CRITICALUP:
                _enhanceIcon.sprite = _iconImgs[3];
                break;
            case ENHANCETYPE.ADDITIONALATTACK:
                _enhanceIcon.sprite = _iconImgs[4];
                break;
        }
    }

    public void InitSet(ENHANCETYPE type, ENHANCEGRADE grade)
    {
        _enhanceIcon = transform.GetChild(0).GetComponent<Image>();
        _enhanceName = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        _enhanceDesc = transform.GetChild(2).GetComponent<TextMeshProUGUI>();

        _enhanceType = type;
        _grade = grade;
        _value = ValueOfGrade(_grade);

        IconSpriteOfType(_enhanceType);
        _enhanceName.text = EnhanceUtility.GetEnhanceText(_enhanceType);
        _enhanceDesc.text = EnhanceUtility.DescribeEnhanceString(_enhanceType, _value);
    }
    
    public void ClickSelectEnhance()
    {
        SoundManager.Instance.sfxPlay(SOUNDENUM.SubEnhanceClick);
        if (_enhanceType == ENHANCETYPE.ADDITIONALATTACK)
            EnhanceUtility.IncreaseAdditionalAttackUp(_value);
        GameManager.Instance.GameReady();
        GetComponentInParent<C_EnhanceSelectWindow>().CloseWindow(_enhanceType, _value);
    }
}
