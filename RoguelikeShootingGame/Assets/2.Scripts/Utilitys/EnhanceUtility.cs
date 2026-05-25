using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using DefineEnum;

public static class EnhanceUtility
{
    static float _totalAdditionalAttackPercent = 0;
    static readonly Dictionary<ENHANCETYPE, string> typeText;
    static readonly Dictionary<ENHANCETYPE, string> strFormat;

    static public string GetEnhanceText(ENHANCETYPE type)
    {
        return typeText[type];
    }

    static public string DescribeEnhanceString(ENHANCETYPE type, float num)
    {
        return string.Format(strFormat[type], num);
    }

    static public void IncreaseAdditionalAttackUp(float value)
    {
        _totalAdditionalAttackPercent += value;
    }

    static public float GetAAP()
    {
        return _totalAdditionalAttackPercent;
    }

    static EnhanceUtility()
    {
        typeText = new Dictionary<ENHANCETYPE, string>();
        strFormat = new Dictionary<ENHANCETYPE, string>();

        StreamReader srTestData = new StreamReader(Path.Combine(Application.dataPath, "Resources", "EnhanceText.csv"));

        srTestData.ReadLine();

        int cnt = 0;
        bool endOfFile = false;
        while (!endOfFile)
        {
            string dataString = srTestData.ReadLine();
            if (dataString == null)
            {
                endOfFile = true;
                break;
            }
            string[] dataValue = dataString.Split('|');

            typeText.Add((ENHANCETYPE)cnt, dataValue[0]);
            strFormat.Add((ENHANCETYPE)cnt, dataValue[1]);

            cnt++;
        }
    }
}
