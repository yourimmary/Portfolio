using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultWindow : MonoBehaviour
{
    public void ClickRestartButton()
    {
        Debug.Log("ReStart");
        GameManager.Instance.GameReady();
    }

    public void ClickGoTitleButton()
    {
        Debug.Log("Go Title");
    }
}
