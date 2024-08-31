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

    Canvas _canvas;
    Transform _mapParent;
    PlayerController _pc;

    /// <summary>
    /// Main Camera
    /// </summary>
    public FollowCamara mc { get; private set; }
    int _monsterCnt = 0;
    List<Vector2> _monSpawnPos;
    Vector2 _playerInitPos;
    GAMESTATE _state = GAMESTATE.GAMEINIT;

    public GAMESTATE GameState { get { return _state; } }

    //UI
    MapSelectWindow _msw;
    C_EnhanceSelectWindow _cew;
    Transform _dwnd;

    private void Awake()
    {
        _uniqueInstance = this;
    }

    private void Start()
    {
        GameInit();
    }

    private void Update()
    {
        if (_state == GAMESTATE.GAMEPLAY)
            GamePlay();
    }

    public void GameInit()
    {
        _state = GAMESTATE.GAMEINIT;
        _monSpawnPos = new List<Vector2>();

        _pc = Instantiate(_playerPrefab, transform.position, transform.rotation).GetComponent<PlayerController>();
        _pc.InitSet();
        _pc.gameObject.SetActive(false);

        mc = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<FollowCamara>();
        mc.Init();

        GameObject go = new GameObject("MapsRoot");
        _mapParent = go.transform;

        _canvas = GameObject.Find("Canvas").GetComponent<Canvas>();

        for (int i = 0; i < _maps.Length; i++)
        {
            GameObject map = Instantiate(_maps[i], _mapParent);
            map.SetActive(false);
        }

        GameReady();
    }

    public void GameReady()
    {
        _state = GAMESTATE.GAMEREADY;
        if (_msw == null)
        {
            _msw = Instantiate(_mapSelectWnd, _canvas.transform).GetComponent<MapSelectWindow>();
            _msw.InitWindow();
        }
        _msw.OpenWindow();
    }

    public void GameStart()
    {
        _state = GAMESTATE.GAMESTART;
        //몬스터 스폰 초기화, 몬스터 생성\
        for (int i = 0; i < _monSpawnPos.Count; i++)
        {
            MonsterController mc = Instantiate(_monstersPrefab[0], _monSpawnPos[i], 
                Quaternion.identity).GetComponent<MonsterController>();
            mc.InitSet(_pc.transform, _mapCount, _curDepth);
        }
        //맵에 따른 플레이어 위치 설정
        _pc.transform.position = _playerInitPos;
        _pc.transform.rotation = Quaternion.identity;
        mc.Follow(_pc.transform.position);
        _pc.gameObject.SetActive(true);
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
        _pc.gameObject.SetActive(false);
        _monSpawnPos.Clear();
        if (_curDepth >= TOTALDEPTH)
            GameEnd();
        else
        {
            if (_cew == null)
            {
                _cew = Instantiate(_EnhanceSelectWnd, _canvas.transform).GetComponent<C_EnhanceSelectWindow>();
                _cew.InitSet();
            }
            _cew.OpenWindow();
            GameReady();
        }
    }

    public void GameEnd()
    {
        //Destroy(_mapParent.gameObject);
        //Destroy(_pc.transform.gameObject);
        _state = GAMESTATE.GAMEEND;
        Debug.Log("GameEnd");
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

    public void SetDamageWindow(int damage)
    {
        TextMeshProUGUI tmp = _canvas.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        tmp.text = damage.ToString();
    }
}
