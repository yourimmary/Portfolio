using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleWindow : MonoBehaviour
{
    public void ClickStartButton()
    {
        GameManager.Instance.GameInitialize();
        gameObject.SetActive(false);
    }

    public void ClickQuitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
