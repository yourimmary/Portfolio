using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineEnum;

public class EnhanceUtility
{
    static string[] _enhanceTexts = { "공격력 증가", "방어력 증가", "체력 증가", "크리티컬 확률 업", "크리티컬 데미지 업", "추가 공격"};
    static Dictionary<ENHANCETYPE, float> _enhanceDegrees = new Dictionary<ENHANCETYPE, float>();

    static public string GetEnhanceText(ENHANCETYPE type)
    {
        switch (type)
        {
            case ENHANCETYPE.ATTUP:
            case ENHANCETYPE.ATTUPPERCENT:
                return _enhanceTexts[0];
            case ENHANCETYPE.DEFUP:
            case ENHANCETYPE.DEFUPPERCENT:
                return _enhanceTexts[1];
            case ENHANCETYPE.HPUP:
            case ENHANCETYPE.HPUPPERCENT:
                return _enhanceTexts[2];
            case ENHANCETYPE.CRITICALUP:
                return _enhanceTexts[3];
            case ENHANCETYPE.CRITICALDAMAGEUP:
                return _enhanceTexts[4];
            case ENHANCETYPE.ADDITIONALATTACK:
                return _enhanceTexts[5];
            default:
                return "대응되는 텍스트가 없습니다.";
        }
    }

    static public string DescribeEnhanceString(ENHANCETYPE type, float num)
    {
        switch (type)
        {
            case ENHANCETYPE.ATTUP:
                return string.Format("공격력 {0} 증가", num);
            case ENHANCETYPE.DEFUP:
                return string.Format("방어력 {0} 증가", num);
            case ENHANCETYPE.HPUP:
                return string.Format("체력 {0} 증가", num);
            case ENHANCETYPE.ATTUPPERCENT:
                return string.Format("공격력 {0}% 증가", num);
            case ENHANCETYPE.DEFUPPERCENT:
                return string.Format("방어력 {0}% 증가", num);
            case ENHANCETYPE.HPUPPERCENT:
                return string.Format("체력 {0}% 증가", num);
            case ENHANCETYPE.CRITICALUP:
                return string.Format("크리티컬 확률 {0}% 증가", num);
            case ENHANCETYPE.CRITICALDAMAGEUP:
                return string.Format("크리티컬 데미지 {0}% 증가", num);
            case ENHANCETYPE.ADDITIONALATTACK:
                return string.Format("공격력 {0}%의 추가 공격", num);
            default:
                return "대응되는 캐릭터 강화가 없습니다.";
        }
    }

    static public void IncreaseRateGetEnhance(ENHANCETYPE type, float degree)
    {
        if (_enhanceDegrees.ContainsKey(type)) _enhanceDegrees[type] += degree;
        else _enhanceDegrees.Add(type, degree);
    }

    static public float GetDegree(ENHANCETYPE type)
    {
        if (_enhanceDegrees.ContainsKey(type))
            return _enhanceDegrees[type];
        else
            return 0;
    }

}
