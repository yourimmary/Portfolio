using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineEnum;

public class UIManager : MonoBehaviour
{
    static UIManager _uniqueInstance;
    public static UIManager Instance { get { return _uniqueInstance; } }
    GameObject _mapSelectWindow;
    /*[SerializeField] */GameObject _hpBarWndPrefab;
    /*[SerializeField] */GameObject _damageWndPrefab;
    /*[SerializeField] */GameObject _findMarkWndPrefab;
    /*[SerializeField] */GameObject _playerWndPrefab;
    /*[SerializeField] */GameObject _itemWndPrefab;
    /*[SerializeField] */GameObject _enhanceSelectWndPrefab;
    /*[SerializeField] */GameObject _resultWndPrefab;

    Canvas _canvas;
    //MapSelectWindow _mapWnd;
    MapSelectWin _mapWnd;
    ResultWindow _resultWnd;
    Transform _mHpWnd;
    Transform _markWnd;
    C_EnhanceSelectWindow _enhanceWnd;
    PlayerWindow _playerWnd;
    GetItemWindow _getItemWnd;

    public PlayerWindow PlayerWindow { get { return _playerWnd; } }
    public Transform HpBarParent { get { return _mHpWnd; } }

    private void Awake()
    {
        _uniqueInstance = this;
        SetInit();
        Debug.Log("UI Manager Awake");
        //DontDestroyOnLoad(gameObject);
    }

    void SetInit()
    {
        _mapSelectWindow = Resources.Load<GameObject>("UIs/MapSelectWin");
        _hpBarWndPrefab = Resources.Load<GameObject>("UIs/HPBar");
        _damageWndPrefab = Resources.Load<GameObject>("UIs/DamageWindow");
        _findMarkWndPrefab = Resources.Load<GameObject>("UIs/FindPlayerMark");
        _playerWndPrefab = Resources.Load<GameObject>("UIs/PlayerWindow");
        _itemWndPrefab = Resources.Load<GameObject>("UIs/GetItemWindow");
        _enhanceSelectWndPrefab = Resources.Load<GameObject>("UIs/C_EnhanceSelectWindow");
        _resultWndPrefab = Resources.Load<GameObject>("UIs/ResultWindow");
    }

    public void CreateUIs()
    {
        _canvas = GameObject.Find("Canvas").GetComponent<Canvas>();

        _playerWnd = Instantiate(_playerWndPrefab, _canvas.transform).GetComponent<PlayerWindow>();
        _enhanceWnd = Instantiate(_enhanceSelectWndPrefab, _canvas.transform).GetComponent<C_EnhanceSelectWindow>();
        //_mapWnd = Instantiate(_mapSelectWindow, _canvas.transform).GetComponent<MapSelectWindow>();
        _mapWnd = Instantiate(_mapSelectWindow, _canvas.transform).GetComponent<MapSelectWin>();
        _mHpWnd = _canvas.transform.GetChild(0);
        _markWnd = _canvas.transform.GetChild(1);
        _resultWnd = Instantiate(_resultWndPrefab, _canvas.transform).GetComponent<ResultWindow>();
    }

    public void InitializeUIs(PlayerController player)
    {
        //_mapWnd.InitWindow();
        _mapWnd.SetInit();
        _playerWnd.InitSet();
        _enhanceWnd.InitSet(player);

        _playerWnd.gameObject.SetActive(false);
        _enhanceWnd.gameObject.SetActive(false);
        _mapWnd.gameObject.SetActive(false);
        _resultWnd.gameObject.SetActive(false);
    }

    public Transform CreateFindMark()
    {
        Transform martTrans = Instantiate(_findMarkWndPrefab, _markWnd).transform;
        return martTrans;
    }

    public void CreateDamageWindow(int damage, Vector2 position)
    {
        DamageTextWindow wnd = Instantiate(_damageWndPrefab, _canvas.transform).GetComponent<DamageTextWindow>();
        wnd.InitSet(damage, position);
    }

    public void OpenWindow(UIENUM ui, int jamCnt = 0)
    {
        switch (ui)
        {
            case UIENUM.PLAYERWINDOW:
                _playerWnd.gameObject.SetActive(true);
                break;
            case UIENUM.GETITEMWINDOW:
                //_getItemWnd.gameObject.SetActive(true);
                //_getItemWnd.CreateItemIcon(jamCnt);
                _getItemWnd = Instantiate(_itemWndPrefab, _canvas.transform).GetComponent<GetItemWindow>();
                _getItemWnd.SetInit();
                _getItemWnd.CreateItemIcon(jamCnt);
                break;
            case UIENUM.MAPSELECTWINDOW:
                //_mapWnd.OpenWindow();
                _mapWnd.gameObject.SetActive(true);
                _mapWnd.CreateMapSelectUI();
                break;
            case UIENUM.ENHANCESELECTWINDOW:
                _enhanceWnd.OpenWindow();
                break;
        }
    }
    public void OpenWindow(bool success)
    {
        _resultWnd.gameObject.SetActive(true);
        _resultWnd.SetResult(success, GameManager.Instance.Player.DIR);
        _resultWnd.WindowOperate();
        //if (success)
        //{
        //    _resultWnd.GetChild(0).gameObject.SetActive(true);
        //    _resultWnd.GetChild(1).gameObject.SetActive(false);
        //}
        //else
        //{
        //    _resultWnd.GetChild(0).gameObject.SetActive(false);
        //    _resultWnd.GetChild(1).gameObject.SetActive(true);
        //}
    }

    public void CloseWindow(UIENUM ui)
    {
        switch (ui)
        {
            case UIENUM.PLAYERWINDOW:
                _playerWnd.gameObject.SetActive(false);
                break;
            case UIENUM.GETITEMWINDOW:
                //Transform childRemove = _getItemWnd.transform.GetChild(0).GetChild(0);
                //int count = childRemove.childCount;

                //for (int i = 0; i < count; i++)
                //{
                //    Destroy(childRemove.GetChild(0).gameObject);
                //}
                //_getItemWnd.ResetChildIndex();
                //_getItemWnd.gameObject.SetActive(false);
                if (_getItemWnd != null)
                    Destroy(_getItemWnd.gameObject);
                break;
            case UIENUM.MAPSELECTWINDOW:
                //_mapWnd.CloseWindow();
                _mapWnd.gameObject.SetActive(false);
                break;
        }
    }

    public void CloseWindow(ENHANCETYPE t, float val)
    {
        _enhanceWnd.CloseWindow(t, val);
    }

    public void DestroyAllUIs()
    {
        Destroy(_mapWnd.gameObject);
        Destroy(_resultWnd.gameObject);
        Destroy(_enhanceWnd.gameObject);
        //Destroy(_itemWindow.gameObject);
        Destroy(_playerWnd.gameObject);
    }
}
