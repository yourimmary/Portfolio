using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineEnum;

public class UIManager : MonoBehaviour
{
    static UIManager _uniqueInstance;
    public static UIManager Instance { get { return _uniqueInstance; } }

    //[SerializeField] GameObject _titleWindow;
    [SerializeField] GameObject _mapSelectWindow;
    [SerializeField] GameObject _hpBarWindow;
    [SerializeField] GameObject _damageWindow;
    [SerializeField] GameObject _findMarkWindow;
    [SerializeField] GameObject _playerWindow;
    [SerializeField] GameObject _enhanceSelectWindow;
    //[SerializeField] GameObject _resultWindow;

    Canvas _canvas;
    MapSelectWindow _mapWnd;
    //Transform _resultWnd;
    Transform _mHpWnd;
    Transform _markWnd;
    C_EnhanceSelectWindow _enhanceWnd;
    PlayerWindow _playerWnd;

    public PlayerWindow PlayerWindow { get { return _playerWnd; } }
    public Transform HpBarParent { get { return _mHpWnd; } }

    private void Awake()
    {
        _uniqueInstance = this;
    }

    public void CreateUIs()
    {
        _canvas = GameObject.Find("Canvas").GetComponent<Canvas>();

        _playerWnd = Instantiate(_playerWindow, _canvas.transform).GetComponent<PlayerWindow>();
        _enhanceWnd = Instantiate(_enhanceSelectWindow, _canvas.transform).GetComponent<C_EnhanceSelectWindow>();
        _mapWnd = Instantiate(_mapSelectWindow, _canvas.transform).GetComponent<MapSelectWindow>();
        _mHpWnd = _canvas.transform.GetChild(0);
        _markWnd = _canvas.transform.GetChild(1);
        //_resultWnd = Instantiate(_resultWindow, _canvas.transform).transform;
    }

    public void InitializeUIs(PlayerController player)
    {
        _mapWnd.InitWindow();
        _playerWnd.InitSet();
        _enhanceWnd.InitSet(player);

        _playerWnd.gameObject.SetActive(false);
        _enhanceWnd.gameObject.SetActive(false);
        _mapWnd.gameObject.SetActive(false);
        //_resultWnd.gameObject.SetActive(false);
    }

    public Transform CreateFindMark()
    {
        Transform martTrans = Instantiate(_findMarkWindow, _markWnd).transform;
        return martTrans;
    }

    public void CreateDamageWindow(int damage, Vector2 position)
    {
        DamageTextWindow wnd = Instantiate(_damageWindow, _canvas.transform).GetComponent<DamageTextWindow>();
        wnd.InitSet(damage, position);
    }

    public void OpenWindow(UIENUM ui)
    {
        switch (ui)
        {
            case UIENUM.PLAYERWINDOW:
                _playerWnd.gameObject.SetActive(true);
                break;
            case UIENUM.MAPSELECTWINDOW:
                _mapWnd.OpenWindow();
                break;
            case UIENUM.ENHANCESELECTWINDOW:
                _enhanceWnd.OpenWindow();
                break;
        }
    }
    //public void OpenWindow(bool success)
    //{
    //    _resultWnd.gameObject.SetActive(true);
    //    if (success)
    //    {
    //        _resultWnd.GetChild(0).gameObject.SetActive(true);
    //        _resultWnd.GetChild(1).gameObject.SetActive(false);
    //    }
    //    else
    //    {
    //        _resultWnd.GetChild(0).gameObject.SetActive(false);
    //        _resultWnd.GetChild(1).gameObject.SetActive(true);
    //    }
    //}

    public void CloseWindow(UIENUM ui)
    {
        switch (ui)
        {
            case UIENUM.PLAYERWINDOW:
                _playerWnd.gameObject.SetActive(false);
                break;
            case UIENUM.MAPSELECTWINDOW:
                _mapWnd.CloseWindow();
                break;
            //case UIENUM.RESULTWINDOW:
            //    _resultWnd.gameObject.SetActive(false);
            //    break;
        }
    }
    public void CloseWindow(ENHANCETYPE t, float val)
    {
        _enhanceWnd.CloseWindow(t, val);
    }
}
