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
    //Transform _rwnd;    //��� ������
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
    // 1. ���� ���� ��
    //   - Ÿ��Ʋ ȭ�� UI �����
    //   - _state = GAMESTATE.GAMETITLE;
    // 2. ���� �ʱ�ȭ
    //   - _state = GAMESTATE.GAMEINIT;
    //   - Ÿ��Ʋ UI ��Ȱ��ȭ
    //   - Ÿ��Ʋ �� ��� UI ����
    //   - �÷��̾� ĳ���� ����
    //   - ���� ī�޶� ����
    //   - �� ����
    public void GameInitialize()
    {
        DontDestroyOnLoad(gameObject);

        _state = GAMESTATE.GAMEINIT;
        _monSpawnPos = new List<Vector2>();
        //   - Ÿ��Ʋ UI ��Ȱ��ȭ
        //   - Ÿ��Ʋ �� ��� UI ����
        UIManager.Instance.CreateUIs();
        //   - �÷��̾� ĳ���� ����
        _player = Instantiate(_playerPrefab, transform.position, transform.rotation).GetComponent<PlayerController>();
        _player.InitSet(UIManager.Instance.PlayerWindow);
        _player.gameObject.SetActive(false);

        //   - UI �ʱ�ȭ
        UIManager.Instance.InitializeUIs(_player);
        //   - ���� ī�޶� ����
        MC = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<FollowCamara>();
        MC.Init();
        //   - �� ����
        GameObject go = new GameObject("MapsRoot");
        _mapParent = go.transform;

        for (int i = 0; i < _maps.Length; i++)
        {
            GameObject map = Instantiate(_maps[i], _mapParent);
            map.SetActive(false);
        }

        GameReady();
    }
    // 3. ���� �غ�
    //   - _state = GAMESTATE.GAMEREADY;
    //   - ���ӿ� �ʼ����� �� �ʱ�ȭ
    //      - �� ���� = 0;
    //      - �÷��̾� ĳ���� �������ͽ� �ʱ�ȭ

    // 4. ���� ����
    //   - _state = GAMESTATE.GAMESTART;
    //   - �� ���� UI Ȱ��ȭ
    //   - �� ���� �� �ش� �� Ȱ��ȭ
    //   - �÷��̾� ĳ���� ��ġ ����, ī�޶� ��ġ ����
    //   - ���� ����
    // 5. ���� �÷��� ��
    //   - ������ Ŭ���� �ϸ� ���� �ܰ�� ����
    // 6. ���� ���
    //   - ĳ���� ��ȭ UI Ȱ��ȭ
    //   - ĳ���� ��ȭ ���� �� ���� �������� �ٽ� ���ư�
    // 7. ���� ��
    //   - �� ���̰� 1000m�� �Ǹ� ������ ����
    //   - ���� ���� ����� �����ִ� UI Ȱ��ȭ
    //   - ReStart ��ư : ���� �غ� �ܰ���� �ٽ� ����
    //   - Title ��ư : ���� Ÿ��Ʋ �ܰ���� �ٽ� ����
    //   - End ��ư : ���ø����̼��� ����
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
        //���� ���� �ʱ�ȭ, ���� ����\
        for (int i = 0; i < _monSpawnPos.Count; i++)
        {
            Transform mark = UIManager.Instance.CreateFindMark();
            MonsterController mc = Instantiate(_monstersPrefab[0], _monSpawnPos[i], 
                Quaternion.identity).GetComponent<MonsterController>();
            mc.InitSet(_player.transform, _mapCount, _curDepth, UIManager.Instance.HpBarParent, mark);
            if (_mapGrid == null) Debug.Log("mapGrid null");
            mc.PathFind.GetGrid(_mapGrid);
        }
        //�ʿ� ���� �÷��̾� ��ġ ����
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
    /// ���� ���� Ű�� �Լ�.
    /// �� id�� -1�̶�� �� ���� ����
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
