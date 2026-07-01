using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MapInfo
{
    string _mapName;
    string _mapDesc;

    public string GetName()
    {
        return _mapName;
    }

    public string GetDesc()
    {
        return _mapDesc;
    }

    public MapInfo(string name, string desc)
    {
        _mapName = name;
        _mapDesc = desc;
    }
}

public static class MapInfoText
{
    static readonly Dictionary<string, MapInfo> mapInfotxt;

    public static MapInfo GetMap(string objectName)
    {
        return mapInfotxt[objectName];
    }

    static MapInfoText()
    {
        mapInfotxt = new Dictionary<string, MapInfo>();

        StreamReader srTestData = new StreamReader(Path.Combine(Application.dataPath, "Resources", "MapInfomationText.csv"));
        TextAsset txtAsset = Resources.Load<TextAsset>("MapInfomationText.csv");

        srTestData.ReadLine();

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

            MapInfo info = new MapInfo(dataValue[1], dataValue[2]);

            mapInfotxt.Add(dataValue[0], info);
        }
    }
}
