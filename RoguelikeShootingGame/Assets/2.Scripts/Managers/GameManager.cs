using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineEnum;
using TMPro;
using System;

public class GameManager : MonoBehaviour
{
    static GameManager _uniqueInstance;
    public static GameManager Instance { get { return _uniqueInstance; } }

    const int TOTALDEPTH = 1000;
    int _curDepth = 0;
    int _mapCount = 0;

    GameObject _playerPrefab;
    GameObject _monstersPrefab;
    GameObject[] _maps;

    Canvas _canvas;
    Transform _mapParent;
    PlayerController _player;
    Map _mapData;

    /// <summary>
    /// Main Camera
    /// </summary>
    public FollowCamara MC { get; private set; }
    GAMESTATE _state = GAMESTATE.GAMETITLE;

    public GAMESTATE GameState { get { return _state; } }
    public Transform MapParent { get { return _mapParent; } }
    public PlayerController Player { get { return _player; } }
    public Canvas CanvasUI { get { return _canvas; } }
    public int MapSelectCount { get { return _mapCount; } }
    public int CurDepth { get { return _curDepth; } }

    private void Awake()
    {
        _uniqueInstance = this;
        SetInit();
        Debug.Log("GameManager Awake");
    }

    void SetInit()
    {
        _playerPrefab = Resources.Load<GameObject>("Characters/Player");
        Console.WriteLine(_playerPrefab.name);
        _monstersPrefab = Resources.Load<GameObject>("Characters/Monster1");
        _maps = Resources.LoadAll<GameObject>("Maps");
    }

    private void Start()
    {
        GameInitialize();
    }

    private void Update()
    {
        if (_state == GAMESTATE.GAMEPLAY)
            GamePlay();
    }

    // 
    // 1. 게임 시작 전
    //   - 타이틀 화면 UI 만들고
    //   - _state = GAMESTATE.GAMETITLE;
    // 2. 게임 초기화
    //   - _state = GAMESTATE.GAMEINIT;
    //   - 타이틀 UI 비활성화
    //   - 타이틀 외 모든 UI 생성
    //   - 플레이어 캐릭터 생성
    //   - 메인 카메라 설정
    //   - 맵 설정
    public void GameInitialize()
    {
        DontDestroyOnLoad(gameObject);

        _state = GAMESTATE.GAMEINIT;
        //   - 타이틀 UI 비활성화
        //   - 타이틀 외 모든 UI 생성
        Debug.Log("UI Create");
        UIManager.Instance.CreateUIs();
        //   - 플레이어 캐릭터 생성
        _player = Instantiate(_playerPrefab, transform.position, transform.rotation).GetComponent<PlayerController>();
        _player.InitSet(UIManager.Instance.PlayerWindow);
        _player.gameObject.SetActive(false);

        //   - UI 초기화
        _canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        UIManager.Instance.InitializeUIs(_player);
        //   - 메인 카메라 설정
        MC = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<FollowCamara>();
        MC.Init();
        //   - 맵 설정 : 맵 생성시 부모 오브젝트 설정
        GameObject go = new GameObject("MapsRoot");
        _mapParent = go.transform;

        GameReady();
    }

    public void GameReady()
    {
        _state = GAMESTATE.GAMEREADY;

        UIManager.Instance.OpenWindow(UIENUM.MAPSELECTWINDOW);
    }

    public void GameStart()
    {
        _state = GAMESTATE.GAMESTART;
        //맵 설정 가져오기
        _mapData = _mapParent.GetChild(0).GetComponent<Map>();

        //맵에 따른 플레이어 위치 설정
        UIManager.Instance.OpenWindow(UIENUM.PLAYERWINDOW);
        if (_mapData.JamCount > 0)
            UIManager.Instance.OpenWindow(UIENUM.GETITEMWINDOW, _mapData.JamCount);

        _player.transform.position = _mapParent.GetChild(0).GetChild(2).position;
        _player.transform.rotation = Quaternion.identity;
        _player.InitStateAndDir();
        MC.Follow(_player.transform.position);
        _player.gameObject.SetActive(true);
        _state = GAMESTATE.GAMEPLAY;
    }

    public void GamePlay()
    {
        if (_mapData.IsClear)
        {
            _mapCount++;
            _curDepth += _mapData.Depth;
            GameResult();
        }
    }

    public void GameResult()
    {
        _state = GAMESTATE.GAMERESULT;
        _player.gameObject.SetActive(false);
        if (_curDepth >= TOTALDEPTH)
            GameEnd(true);
        else
        {
            UIManager.Instance.OpenWindow(UIENUM.ENHANCESELECTWINDOW);
        }
    }

    public void GameEnd(bool isSuccess)
    {
        _state = GAMESTATE.GAMEEND;

        UIManager.Instance.OpenWindow(isSuccess);
    }

    public int SetDistance()
    {
        int d = UnityEngine.Random.Range(50, 121);
        if (TOTALDEPTH - _curDepth < d)
            d = TOTALDEPTH - _curDepth;
        return d;
    }

    public bool IsWalkableNodePos(Vector3 pos)
    {
        Node node = _mapData.MapGrid.NodeFromWorldPoint(pos);
        return node._walkable;
    }
}
