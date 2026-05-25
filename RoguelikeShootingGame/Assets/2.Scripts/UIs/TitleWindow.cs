using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DefineEnum;

public class TitleWindow : MonoBehaviour
{
    [SerializeField] Animator _animator;

    AsyncOperation _oper;

    private void Start()
    {
        ReadyLoadScene();
        StartCoroutine(ActiveButtons());
    }

    public void ClickStartButton()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        SoundManager.Instance.sfxPlay(SOUNDENUM.UIButtonClick);

        //_oper = SceneManager.LoadSceneAsync("GameScene");
        //_oper.allowSceneActivation = false;

        StartCoroutine(LoadGameScene());
    }

    public void ClickQuitButton()
    {
        SoundManager.Instance.sfxPlay(SOUNDENUM.UIButtonClick);

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    void ReadyLoadScene()
    {
        _oper = SceneManager.LoadSceneAsync("GameScene");
        _oper.allowSceneActivation = false;
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
