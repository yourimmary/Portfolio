using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineEnum;
using TMPro;

public class GameManager : MonoBehaviour
{
    static GameManager _uniqueInstance;

    public static GameManager Instance { get { return _uniqueInstance; } }
    
    const int TOTALDEPTH = 1000;
    int _curDepth = 0;
    int _mapCount = 0;

    [SerializeField] GameObject _playerPrefab;
    [SerializeField] GameObject[] _monstersPrefab;
    [SerializeField] GameObject[] _maps;
    [Header("UI")]
    [SerializeField] GameObject _mapSelectWnd;
    [SerializeField] GameObject _EnhanceSelectWnd;
    [SerializeField] GameObject _resultWnd;
    [SerializeField] GameObject _damageWnd;
    [SerializeField] GameObject _playerWnd;
    [SerializeField] GameObject _FindMark;

    Canvas _canvas;
    Transform _mapParent;
    PlayerController _player;
    Gride _mapGrid;

    /// <summary>
    /// Main Camera
    /// </summary>
    public FollowCamara MC { get; private set; }
    int _monsterCnt = 0;
    List<Vector2> _monSpawnPos;
    Vector2 _playerInitPos;
    GAMESTATE _state = GAMESTATE.GAMETITLE;

    public GAMESTATE GameState { get { return _state; } }

    //UI
    //MapSelectWindow _msw;
    //C_EnhanceSelectWindow _cew;
    //Transform _rwnd;    //결과 윈도우
    //Transform _mHpWnd;
    //Transform _markWnd;

    private void Awake()
    {
        _uniqueInstance = this;
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
        _monSpawnPos = new List<Vector2>();
        //   - 타이틀 UI 비활성화
        //   - 타이틀 외 모든 UI 생성
        UIManager.Instance.CreateUIs();
        //   - 플레이어 캐릭터 생성
        _player = Instantiate(_playerPrefab, transform.position, transform.rotation).GetComponent<PlayerController>();
        _player.InitSet(UIManager.Instance.PlayerWindow);
        _player.gameObject.SetActive(false);

        //   - UI 초기화
        UIManager.Instance.InitializeUIs(_player);
        //   - 메인 카메라 설정
        MC = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<FollowCamara>();
        MC.Init();
        //   - 맵 설정
        GameObject go = new GameObject("MapsRoot");
        _mapParent = go.transform;

        for (int i = 0; i < _maps.Length; i++)
        {
            GameObject map = Instantiate(_maps[i], _mapParent);
            map.SetActive(false);
        }

        GameReady();
    }
    // 3. 게임 준비
    //   - _state = GAMESTATE.GAMEREADY;
    //   - 게임에 필수적인 값 초기화
    //      - 총 깊이 = 0;
    //      - 플레이어 캐릭터 스테이터스 초기화

    // 4. 게임 시작
    //   - _state = GAMESTATE.GAMESTART;
    //   - 맵 선택 UI 활성화
    //   - 맵 선택 후 해당 맵 활성화
    //   - 플레이어 캐릭터 위치 조정, 카메라 위치 조정
    //   - 몬스터 스폰
    // 5. 게임 플레이 중
    //   - 조건을 클리어 하면 다음 단계로 진행
    // 6. 게임 결과
    //   - 캐릭터 강화 UI 활성화
    //   - 캐릭터 강화 선택 후 게임 시작으로 다시 돌아감
    // 7. 게임 끝
    //   - 총 깊이가 1000m가 되면 게임이 끝남
    //   - 게임 최종 결과를 보여주는 UI 활성화
    //   - ReStart 버튼 : 게임 준비 단계부터 다시 시작
    //   - Title 버튼 : 게임 타이틀 단계부터 다시 시작
    //   - End 버튼 : 애플리케이션을 종료
    // 

    //public void GameInit()
    //{
    //    _state = GAMESTATE.GAMEINIT;
    //    _monSpawnPos = new List<Vector2>();
    //    _canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
    //    //_mHpWnd = _canvas.transform.GetChild(0);
    //    //_markWnd = _canvas.transform.GetChild(1);
    //    GameObject pw = Instantiate(_playerWnd, _canvas.transform);
    //    _player = Instantiate(_playerPrefab, transform.position, transform.rotation).GetComponent<PlayerController>();
    //    _player.InitSet(pw.GetComponent<PlayerWindow>());
    //    _player.gameObject.SetActive(false);
    //    MC = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<FollowCamara>();
    //    MC.Init();
    //    GameObject go = new GameObject("MapsRoot");
    //    _mapParent = go.transform;
    //    for (int i = 0; i < _maps.Length; i++)
    //    {
    //        GameObject map = Instantiate(_maps[i], _mapParent);
    //        map.SetActive(false);
    //    }
    //    GameReady();
    //}

    public void GameReady()
    {
        _state = GAMESTATE.GAMEREADY;

        UIManager.Instance.OpenWindow(UIENUM.MAPSELECTWINDOW);
    }

    public void GameStart()
    {
        _state = GAMESTATE.GAMESTART;
        //몬스터 스폰 초기화, 몬스터 생성\
        for (int i = 0; i < _monSpawnPos.Count; i++)
        {
            Transform mark = UIManager.Instance.CreateFindMark();
            MonsterController mc = Instantiate(_monstersPrefab[0], _monSpawnPos[i], 
                Quaternion.identity).GetComponent<MonsterController>();
            mc.InitSet(_player.transform, _mapCount, _curDepth, UIManager.Instance.HpBarParent, mark);
            if (_mapGrid == null) Debug.Log("mapGrid null");
            mc.PathFind.GetGrid(_mapGrid);
        }
        //맵에 따른 플레이어 위치 설정
        UIManager.Instance.OpenWindow(UIENUM.PLAYERWINDOW);
        _player.transform.position = _playerInitPos;
        _player.transform.rotation = Quaternion.identity;
        MC.Follow(_player.transform.position);
        _player.gameObject.SetActive(true);
        _state = GAMESTATE.GAMEPLAY;
    }

    public void GamePlay()
    {
        if (_monsterCnt <= 0)
            GameResult();
    }

    public void GameResult()
    {
        _state = GAMESTATE.GAMERESULT;
        ActiveMap(-1, false);
        _player.gameObject.SetActive(false);
        _monSpawnPos.Clear();
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

    //

    /// <summary>
    /// 맵을 껐다 키는 함수.
    /// 맵 id가 -1이라면 맵 전부 적용
    /// </summary>
    public void ActiveMap(int mapId, bool isActive, int depth = 0)
    {
        if (mapId == -1)
        {
            for (int i = 0; i < _mapParent.childCount; i++)
                _mapParent.GetChild(i).gameObject.SetActive(isActive);
        }
        else
        {
            _mapParent.GetChild(mapId - 1).gameObject.SetActive(isActive);
            if (isActive)
            {
                _mapCount++;
                _curDepth += depth;
                _playerInitPos = _mapParent.GetChild(mapId - 1).GetChild(2).position;
                _monsterCnt = _mapParent.GetChild(mapId - 1).GetChild(1).childCount;
                _mapGrid = _mapParent.GetChild(mapId - 1).GetComponent<Gride>();
                for (int i = 0; i < _monsterCnt; i++)
                    _monSpawnPos.Add(_mapParent.GetChild(mapId - 1).GetChild(1).GetChild(i).position);
            }
        }
    }

    public int SetDistance()
    {
        int d = Random.Range(50, 121);
        if (TOTALDEPTH - _curDepth < d)
            d = TOTALDEPTH - _curDepth;
        return d;
    }

    public void DecreaseMonsterCount()
    {
        _monsterCnt -= 1;
    }
}
