using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DefineEnum;

public class ResultWindow : MonoBehaviour
{
    public void ClickRestartButton()
    {
        Debug.Log("ReStart");
        SceneManager.LoadScene("SampleScene");
    }

    public void ClickGoTitleButton()
    {
        Debug.Log("Go Title");
        SceneManager.LoadScene("TitleScene");
    }
}
