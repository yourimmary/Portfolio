using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleWindow : MonoBehaviour
{
    [SerializeField] Animator _animator;

    AsyncOperation _oper;

    private void Start()
    {
        StartCoroutine(ActiveButtons());
    }

    public void ClickStartButton()
    {
        transform.GetChild(0).gameObject.SetActive(false);

        _oper = SceneManager.LoadSceneAsync("GameScene");
        _oper.allowSceneActivation = false;

        StartCoroutine(LoadGameScene());
    }

    public void ClickQuitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    IEnumerator ActiveButtons()
    {
        yield return new WaitForSeconds(3.0f);

        transform.GetChild(0).gameObject.SetActive(true);
    }

    IEnumerator LoadGameScene()
    {
        _animator.SetBool("GameStart", true);

        yield return new WaitForSeconds(1.2f);

        _oper.allowSceneActivation = true;
    }
}
