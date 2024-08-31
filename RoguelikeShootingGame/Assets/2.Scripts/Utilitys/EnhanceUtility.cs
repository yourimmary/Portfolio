using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineEnum;

public class EnhanceUtility
{
    static string[] _enhanceTexts = { "���ݷ� ����", "���� ����", "ü�� ����", "ũ��Ƽ�� Ȯ�� ��", "ũ��Ƽ�� ������ ��", "�߰� ����"};
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
                return "�����Ǵ� �ؽ�Ʈ�� �����ϴ�.";
        }
    }

    static public string DescribeEnhanceString(ENHANCETYPE type, float num)
    {
        switch (type)
        {
            case ENHANCETYPE.ATTUP:
                return string.Format("���ݷ� {0} ����", num);
            case ENHANCETYPE.DEFUP:
                return string.Format("���� {0} ����", num);
            case ENHANCETYPE.HPUP:
                return string.Format("ü�� {0} ����", num);
            case ENHANCETYPE.ATTUPPERCENT:
                return string.Format("���ݷ� {0}% ����", num);
            case ENHANCETYPE.DEFUPPERCENT:
                return string.Format("���� {0}% ����", num);
            case ENHANCETYPE.HPUPPERCENT:
                return string.Format("ü�� {0}% ����", num);
            case ENHANCETYPE.CRITICALUP:
                return string.Format("ũ��Ƽ�� Ȯ�� {0}% ����", num);
            case ENHANCETYPE.CRITICALDAMAGEUP:
                return string.Format("ũ��Ƽ�� ������ {0}% ����", num);
            case ENHANCETYPE.ADDITIONALATTACK:
                return string.Format("���ݷ� {0}%�� �߰� ����", num);
            default:
                return "�����Ǵ� ĳ���� ��ȭ�� �����ϴ�.";
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
