using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DefineEnum;
using UnityEngine.UI;
using System.Net.Http.Headers;
using System;
using System.ComponentModel.Design;

public class ResultWindow : MonoBehaviour
{
    int _aniIndex = 0;
    bool _isSuccess;
    bool _isPlayAni;
    float _time = 0;
    string _spriteName;
    readonly string _dieCommonText = "DeathSprite";
    CHARACTERDIR _chDir;
    Transform _loaddingImg;
    Image _chImg;
    Sprite[] _sprite;

    private void OnEnable()
    {
        _loaddingImg = transform.GetChild(2);
    }

    private void Update()
    {
        if (_isPlayAni)
        {
            if (_time <= 0)
            {
                if (_aniIndex < _sprite.Length)
                {
                    _chImg.sprite = _sprite[_aniIndex];
                    _aniIndex++;
                    if (_isSuccess && _aniIndex == _sprite.Length)
                        _aniIndex = 0;
                }
            }
            _time += Time.deltaTime;
            if (_time >= 0.2f)
                _time = 0;
        }
    }

    public void SetResult(bool isSuccess, CHARACTERDIR dir)
    {
        _isSuccess = isSuccess;
        _chDir = dir;
    }

    public void WindowOperate()
    {
        if (_isSuccess)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(1).gameObject.SetActive(false);
            _chImg = transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>();
            _sprite = Resources.LoadAll<Sprite>("Sprites/Excite");
            _isPlayAni = true;
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(true);
            _chImg = transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<Image>();
            switch (_chDir)
            {
                case CHARACTERDIR.UP:
                    _spriteName = _dieCommonText.Insert(5, " Back ");
                    break;
                case CHARACTERDIR.DOWN:
                    _spriteName = _dieCommonText.Insert(5, " Front ");
                    break;
                case CHARACTERDIR.LEFT:
                    _spriteName = _dieCommonText.Insert(5, " Side ");
                    _chImg.GetComponent<RectTransform>().localScale = new Vector3(-1, 1, 1);
                    break;
                case CHARACTERDIR.RIGHT:
                    _spriteName = _dieCommonText.Insert(5, " Side ");
                    break;
            }
            _sprite = Resources.LoadAll<Sprite>("Sprites/" + _spriteName);
            _isPlayAni = true;
        }
    }

    public void ClickGoTitleButton()
    {
        _loaddingImg.gameObject.SetActive(true);
        SceneManager.LoadScene("TitleScene");
    }
}
